using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;

public class MainScript : MonoBehaviour
{
    public static List<Wallprops> WallpropsList = new List<Wallprops>();
    public List<Wallprops> PolyList1 = new List<Wallprops>();
    public List<Wallprops> PolyList2 = new List<Wallprops>();
    public Texture2D wallTex;
    //  bool LTisbigger = new bool();
    public static float NewWallY;
    public static float NewWallX;
    public static GameObject NewWall;
    public PhysicsMaterial2D JumpMaterial;

    Vector3 newSize;
    void CreateNewWAll()
    {
        NewWall = new GameObject();
        SpriteRenderer sr = NewWall.AddComponent<SpriteRenderer>();
        sr.sprite = Sprite.Create(wallTex, new Rect(0.0f, 0.0f, wallTex.width, wallTex.height), new Vector2(0.5f, 0.5f));
        sr.color = Color.black;


        if (NewWallY == 0)
        {
            NewWallY = StopWallRD.CollisionPos.y;
        }
        if (NewWallX == 0)
        {
            NewWallX = StopWallRD.CollisionPos.x;
        }

        if (Horizontal)
        {

            float fLTWallX = StopWallLT.objectposition.x - 0.7f;
            float fRDWallX = StopWallRD.objectposition.x + 0.7f;

            newSize = new Vector3(0.77f, fLTWallX - fRDWallX, 0);
            NewWall.transform.eulerAngles = new Vector3(0, 0, 90);
            NewWall.transform.position = new Vector2((fLTWallX + fRDWallX) / 2, NewWallY);

        }
        else
        {
            newSize = new Vector3(0.77f, (StopWallLT.objectposition.y + 0.7f) - (StopWallRD.objectposition.y - 0.7f), 0);
            NewWall.transform.position = new Vector2(NewWallX, ((StopWallLT.objectposition.y + 0.7f) + (StopWallRD.objectposition.y - 0.7f)) / 2);

        }

        NewWall.AddComponent<BoxCollider2D>();


        NewWall.GetComponent<BoxCollider2D>().sharedMaterial = JumpMaterial;

        NewWall.AddComponent<Overlapping2>();


        var scale = new Vector3(2, newSize.y / 7.4f, 0);

        NewWall.transform.localScale = scale;
        NewWall.name = Convert.ToString(WallpropsList.Count);

        NewWall.layer = 11;



    }
    public class Wallprops
    {
        public GameObject Wall;
        public float ChainPos;
        public Vector2 Corner1;
        public Vector2 Corner2;

        public Wallprops(GameObject wall, float chainPos, Vector2 corner1, Vector2 corner2)
        {
            Wall = wall;
            Corner1 = corner1;
            Corner2 = corner2;
            ChainPos = chainPos;
        }
    }
    void AssignValues(GameObject wall)
    {
        Vector3 wallposition = wall.transform.position;
        Vector3 Extents = wall.GetComponent<SpriteRenderer>().bounds.extents;
        if (string.Equals(wall.tag, "TopWalls", StringComparison.OrdinalIgnoreCase))
        {
            var ChainPos = Convert.ToSingle(wall.name);
            var Corner1 = new Vector2(wallposition.x - Extents.x, wallposition.y - Extents.y);
            var Corner2 = new Vector2(wallposition.x + Extents.x, wallposition.y - Extents.y);
            WallpropsList.Add(new Wallprops(wall, ChainPos, Corner1, Corner2));
        }
        else if (string.Equals(wall.tag, "LeftWalls", StringComparison.OrdinalIgnoreCase))
        {
            var ChainPos = Convert.ToSingle(wall.name);
            var Corner1 = new Vector2(wallposition.x + Extents.x, wallposition.y - Extents.y);
            var Corner2 = new Vector2(wallposition.x + Extents.x, wallposition.y + Extents.y);
            WallpropsList.Add(new Wallprops(wall, ChainPos, Corner1, Corner2));
        }
        else if (string.Equals(wall.tag, "RightWalls", StringComparison.OrdinalIgnoreCase))
        {
            var ChainPos = Convert.ToSingle(wall.name);
            var Corner1 = new Vector2(wallposition.x - Extents.x, wallposition.y + Extents.y);
            var Corner2 = new Vector2(wallposition.x - Extents.x, wallposition.y - Extents.y);
            WallpropsList.Add(new Wallprops(wall, ChainPos, Corner1, Corner2));
        }
        else if (string.Equals(wall.tag, "DownWalls", StringComparison.OrdinalIgnoreCase))
        {
            var ChainPos = Convert.ToSingle(wall.name);
            var Corner1 = new Vector2(wallposition.x + Extents.x, wallposition.y + Extents.y);
            var Corner2 = new Vector2(wallposition.x - Extents.x, wallposition.y + Extents.y);
            WallpropsList.Add(new Wallprops(wall, ChainPos, Corner1, Corner2));
        }
    }
    public List<Wallprops> DeleteList = new List<Wallprops>();
    public class MyComparer : IComparer<float>
    {
        public int Divider { get; set; }
        public MyComparer(int divider) { Divider = divider; }

        public int Compare(float x, float y)
        {
            if (x < Divider && y > Divider) return 1;
            if (x > Divider && y < Divider) return -1;
            return x.CompareTo(y);
        }
    }
    public static bool NumberPossible;
    List<float> CurrentlyFilledArea = new List<float>();
    public Vector3 TotalArea = new Vector3();
    float FilledAreafloat = new float();
    public GameObject FillTop;
    public GameObject FillDown;
    public GameObject FillLeft;
    public GameObject FillRight;
    public float FilledArea = new float();
    double LastPercentage = new double();
    GameObject HigherWallLT;
    GameObject HigherWallRD;
    public float CalculatePoints(float PercentageFilled)
    {
        float points = PercentageFilled * (1 + 0.2f * (0.2f * PercentageFilled));
        return points;
    }
    bool Horizontal = false;
    Collider2D[] LTOverlappingWalls;
    Collider2D[] RDOverlappingWalls;
    Vector2 LTPointNewWall = new Vector2();
    Vector2 RDPointNewWall = new Vector2();
    List<GameObject> EnemiesAlive = new List<GameObject>();
    public static bool FillWholeField = false;
    public GameObject PercentageAdd;
    public GameObject PointsAdd;
    public int PointsAdded;

    public float CalculatePolygonArea(Vector2[] Vertices)
    {
        float[] x = new float[Vertices.Length];
        float[] y = new float[Vertices.Length];

        for (int i = 0; i < Vertices.Length; i++)
        {
            x[i] = Vertices[i].x;
            y[i] = Vertices[i].y;
        }


        if ((x == null) || (y == null)) return 0;  // auf leere Argumente testen
        int n = Math.Min(x.Length, y.Length);        // Anzahl der Ecken des Polygons
        if (n < 3) return 0;                       // ein Polygon hat mindestens drei Eckpunkte
        float a = 0;                              // Variable fuer Flaeche des Polygons
        for (int i = 0; i < n; i++)
        {                                           // Schleife zwecks Summenbildung
            a += (y[i] + y[(i + 1) % n]) * (x[i] - x[(i + 1) % n]);
        }
        return Math.Abs(a / 2);                    // Flaecheninhalt zurueckliefern
    }

    IEnumerator StopAnimation(float seconds, GameObject Filling)
    {
        yield return new WaitForSecondsRealtime(seconds);
        Filling.GetComponentInChildren<Animator>().speed = 0;
    }
    /*
       public string[] arr = { "top", "down", "left", "right" };

       void update_subfunc()
       {

       }

      
    void update_instantiert(int direction,Vector2 PP)
    {
        if (direction == 0)
        {
            GameObject t = FillTop;
        }
        if(direction)
        GameObject t = FillTop;
        Instantiate(t, new Vector2(PP.x, PP.y - 1.4f), Quaternion.identity);
        if (FillWholeField)
        {
            Instantiate(FillTop, PP, Quaternion.identity);
        }
        NewWall.AddComponent<StopWallRD>();
        NewWall.tag = "DownWalls";
    } */


    LayerMask PossibleWalls;

    void Start()
    {
        PossibleWalls = GameObject.Find("Main Camera").GetComponent<ManageWalls>().PossibleWalls;
    }

    void Update()
    {

        if (ManageWalls.AssignmentComplete && swipegame._Character != null)
        {
            Destroy(swipegame._preview);

            ManageWalls.AssignmentComplete = false;

            if (swipegame.Horizontal)
            {
                Horizontal = true;
            }
            else
            {
                Horizontal = false;
            }


            GameObject Player = swipegame._Character;

            Player.tag = "out";
            WallpropsList.Clear();
            foreach (GameObject wall in ManageWalls.ActiveWalls)
                AssignValues(wall);



            EnemiesAlive.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
            int enemycount = new int();
            foreach (GameObject enemy in EnemiesAlive)
            {
                if (enemy.GetComponent<Enemy3Script>() == null && enemy.GetComponent<BombScript>() == null)
                {
                    enemycount += 1;
                }
            }

            if (enemycount == 0)
            {
                FillWholeField = true;
            }


            if (FilledArea == 0)
            {
                Vector2[] ScoreTracker = new Vector2[WallpropsList.Count];

                for (int i = 0; i < WallpropsList.Count; i++)
                {
                    ScoreTracker[i] = WallpropsList[i].Corner2;
                }

                FilledArea = CalculatePolygonArea(ScoreTracker);


            }

            CreateNewWAll();

            Vector3 NewExtents = NewWall.GetComponent<SpriteRenderer>().bounds.extents;
            //get the edge points of newwall
            NewWall.layer = 0;
            Vector3 Newwallposition = NewWall.transform.position;


            if (Horizontal)
            {
                LTPointNewWall = new Vector2(Newwallposition.x - NewExtents.x, Newwallposition.y);
                RDPointNewWall = new Vector2(Newwallposition.x + NewExtents.x, Newwallposition.y);
            }
            else
            {
                LTPointNewWall = new Vector2(Newwallposition.x, Newwallposition.y + NewExtents.y);
                RDPointNewWall = new Vector2(Newwallposition.x, Newwallposition.y - NewExtents.y);
            }

            LTOverlappingWalls = Physics2D.OverlapBoxAll(LTPointNewWall, new Vector2(0.9f, 0.9f), 0, PossibleWalls);

            RDOverlappingWalls = Physics2D.OverlapBoxAll(RDPointNewWall, new Vector2(0.9f, 0.9f), 0, PossibleWalls);
            NewWall.layer = 11;


            if (Horizontal)
            {
                foreach (Collider2D wall in LTOverlappingWalls)
                {
                    if (!string.Equals(wall.gameObject.name, "out", StringComparison.OrdinalIgnoreCase))
                    {

                        if (HigherWallLT == null)
                        {
                            HigherWallLT = wall.gameObject;
                        }

                        if (wall.transform.position.y > HigherWallLT.transform.position.y)
                        {
                            HigherWallLT = wall.gameObject;
                        }
                    }
                }
                foreach (Collider2D wall in RDOverlappingWalls)
                {
                    if (!string.Equals(wall.gameObject.name, "out", StringComparison.OrdinalIgnoreCase))
                    {

                        if (HigherWallRD == null)
                        {
                            HigherWallRD = wall.gameObject;
                        }
                        if (wall.transform.position.y > HigherWallRD.transform.position.y)
                        {
                            HigherWallRD = wall.gameObject;
                        }
                    }
                }
            }
            else
            {
                foreach (Collider2D wall in LTOverlappingWalls)
                {
                    if (!string.Equals(wall.gameObject.name, "out", StringComparison.OrdinalIgnoreCase))
                    {

                        if (HigherWallLT == null)
                        {
                            HigherWallLT = wall.gameObject;
                        }

                        if (wall.transform.position.x < HigherWallLT.transform.position.x)
                        {
                            HigherWallLT = wall.gameObject;
                        }
                    }

                }
                foreach (Collider2D wall in RDOverlappingWalls)
                {
                    if (!string.Equals(wall.gameObject.name, "out", StringComparison.OrdinalIgnoreCase))
                    {
                        if (HigherWallRD == null)
                        {
                            HigherWallRD = wall.gameObject;
                        }

                        if (wall.transform.position.x < HigherWallRD.transform.position.x)
                        {
                            HigherWallRD = wall.gameObject;
                        }
                    }
                }
            }

            int LTNumber = Convert.ToInt32(HigherWallLT.name);
            int RDNumber = Convert.ToInt32(HigherWallRD.name);

            HigherWallLT = null;
            HigherWallRD = null;


            foreach (Wallprops wall in WallpropsList)
            {
                if (LTNumber > RDNumber)
                {
                    if (wall.ChainPos < LTNumber && wall.ChainPos > RDNumber)
                    {
                        PolyList1.Add(wall);
                    }
                    else
                    {
                        PolyList2.Add(wall);
                    }
                }
                else
                {
                    if (wall.ChainPos < RDNumber && wall.ChainPos > LTNumber)
                    {
                        PolyList1.Add(wall);
                    }
                    else
                    {
                        PolyList2.Add(wall);
                    }
                }
            }


            // sorts Polygon ColliderPoints
            if (Horizontal)
            {
                if (PolyList1[0].Corner2.y < PolyList2[0].Corner2.y)
                {
                    if (LTNumber < RDNumber)
                    {

                        PolyList1 = PolyList1.OrderBy(i => i.ChainPos, new MyComparer(LTNumber - 1)).ToList();
                        PolyList2 = PolyList2.OrderBy(i => i.ChainPos, new MyComparer(LTNumber)).ToList();
                    }
                    else
                    {

                        PolyList1 = PolyList1.OrderBy(i => i.ChainPos, new MyComparer(LTNumber)).ToList();
                        PolyList2 = PolyList2.OrderBy(i => i.ChainPos, new MyComparer(LTNumber - 1)).ToList();
                    }
                }
                else
                {
                    if (LTNumber < RDNumber)
                    {

                        PolyList1 = PolyList1.OrderBy(i => i.ChainPos, new MyComparer(LTNumber)).ToList();
                        PolyList2 = PolyList2.OrderBy(i => i.ChainPos, new MyComparer(LTNumber - 1)).ToList();
                    }
                    else
                    {

                        PolyList1 = PolyList1.OrderBy(i => i.ChainPos, new MyComparer(LTNumber - 1)).ToList();
                        PolyList2 = PolyList2.OrderBy(i => i.ChainPos, new MyComparer(LTNumber)).ToList();
                    }
                }
            }
            else
            {
                if (PolyList1[0].Corner2.x < PolyList2[0].Corner2.x)
                {
                    if (LTNumber < RDNumber)
                    {

                        PolyList1 = PolyList1.OrderBy(i => i.ChainPos, new MyComparer(LTNumber)).ToList();
                        PolyList2 = PolyList2.OrderBy(i => i.ChainPos, new MyComparer(LTNumber - 1)).ToList();
                    }
                    else
                    {

                        PolyList1 = PolyList1.OrderBy(i => i.ChainPos, new MyComparer(LTNumber - 1)).ToList();
                        PolyList2 = PolyList2.OrderBy(i => i.ChainPos, new MyComparer(LTNumber)).ToList();
                    }
                }
                else
                {
                    if (LTNumber < RDNumber)
                    {

                        PolyList1 = PolyList1.OrderBy(i => i.ChainPos, new MyComparer(RDNumber)).ToList();
                        PolyList2 = PolyList2.OrderBy(i => i.ChainPos, new MyComparer(RDNumber - 1)).ToList();
                    }
                    else
                    {

                        PolyList1 = PolyList1.OrderBy(i => i.ChainPos, new MyComparer(LTNumber)).ToList();
                        PolyList2 = PolyList2.OrderBy(i => i.ChainPos, new MyComparer(LTNumber - 1)).ToList();
                    }
                }
            }

            // transforms PolyLists in Arrays

            int List1Count = PolyList1.Count;
            int List2Count = PolyList2.Count;

            Vector2[] PolyArray1 = new Vector2[List1Count + 1];
            Vector2[] PolyArray2 = new Vector2[List2Count + 1];


            for (int i = 0; i < List1Count; i++)
            {
                PolyArray1[i] = PolyList1[i].Corner2;
            }

            for (int i = 0; i < List2Count; i++)
            {
                PolyArray2[i] = PolyList2[i].Corner2;
            }



            // adds missing Colliderpoint

            if ((Horizontal && PolyArray1[0].y > PolyArray2[0].y) || (!Horizontal && PolyArray1[0].x >= PolyArray2[0].x))
            {
                PolyArray1[PolyList1.Count] = StopWallLT.CollisionPos;
                PolyArray2[PolyList2.Count] = StopWallRD.CollisionPos;
            }
            else
            {
                PolyArray1[PolyList1.Count] = StopWallRD.CollisionPos;
                PolyArray2[PolyList2.Count] = StopWallLT.CollisionPos;
            }





            //creates PolygonCollider
            //   var Poly1 = new GameObject();
            // Poly1.name = "Poly1";
            // Poly1.AddComponent<PolygonCollider2D>();
            // Poly1.GetComponent<PolygonCollider2D>().isTrigger = true;

            // var Poly2 = new GameObject();
            // Poly2.name = "Poly2";
            // Poly2.AddComponent<PolygonCollider2D>();
            // Poly2.GetComponent<PolygonCollider2D>().isTrigger = true;

            // Poly1.GetComponent<PolygonCollider2D>().points = PolyArray1;
            // Poly2.GetComponent<PolygonCollider2D>().points = PolyArray2;
            // Größeren Flächeninhalt  bestimmen
            float Area1 = CalculatePolygonArea(PolyArray1);
            float Area2 = CalculatePolygonArea(PolyArray2);

            GameObject[] Wallsout = GameObject.FindGameObjectsWithTag("out");

            foreach (GameObject wall in Wallsout)
            {
                wall.layer = 14;
            }



            if (Area1 > Area2)
            {

                Vector2 PlayerPosition = Player.transform.position;


                if (Horizontal)
                {
                    Player.transform.position = new Vector2(PlayerPosition.x, Newwallposition.y);
                    PlayerPosition.y = Newwallposition.y;
                    if (PolyArray1[0].y > PolyArray2[0].y)
                    {
                        //Poly1 is Top, Top is bigger


                        Instantiate(FillDown, new Vector2(PlayerPosition.x, PlayerPosition.y - 1.4f), Quaternion.identity);
                        if (FillWholeField)
                        {
                            Instantiate(FillTop, PlayerPosition, Quaternion.identity);
                        }
                        NewWall.AddComponent<StopWallRD>();
                        NewWall.tag = "DownWalls";
                    }
                    else
                    {
                        //Poly1 is Down, Down is bigger


                        Instantiate(FillTop, new Vector2(PlayerPosition.x, PlayerPosition.y + 1.4f), Quaternion.identity);
                        if (FillWholeField)
                        {
                            Instantiate(FillDown, PlayerPosition, Quaternion.identity);
                        }
                        Player.transform.rotation = Quaternion.Euler(0, 0, 180);
                        NewWall.tag = "TopWalls";
                        NewWall.AddComponent<StopWallLT>();
                    }
                }
                else
                {
                    Player.transform.position = new Vector2(Newwallposition.x, PlayerPosition.y);
                    PlayerPosition.x = Newwallposition.x;
                    if (PolyArray1[0].x < PolyArray2[0].x)
                    {
                        //Poly1 is Left, Left is bigger


                        Instantiate(FillRight, new Vector2(PlayerPosition.x + 1.4f, PlayerPosition.y), Quaternion.identity);
                        if (FillWholeField)
                        {
                            Instantiate(FillLeft, PlayerPosition, Quaternion.identity);
                        }
                        Player.transform.rotation = Quaternion.Euler(0, 0, 90);
                        NewWall.AddComponent<StopWallRD>();
                        NewWall.tag = "RightWalls";
                    }
                    else
                    {
                        //Poly1 is Right, Right is bigger


                        Instantiate(FillLeft, new Vector2(PlayerPosition.x - 1.4f, PlayerPosition.y), Quaternion.identity);
                        if (FillWholeField)
                        {
                            Instantiate(FillRight, PlayerPosition, Quaternion.identity);
                        }
                        NewWall.tag = "LeftWalls";
                        NewWall.AddComponent<StopWallLT>();

                    }

                    //  FillObject.GetComponent<FillColorLayer>().WalltoAttach = NewWall;
                }



                CurrentlyFilledArea.Add(Area2);
                PlayerPrefs.SetFloat("AreaFilled", PlayerPrefs.GetFloat("AreaFilled") + Area2);

                if (FillWholeField)
                {
                    CurrentlyFilledArea.Add(Area1);
                    PlayerPrefs.SetFloat("AreaFilled", PlayerPrefs.GetFloat("AreaFilled") + Area1);
                }

                foreach (Wallprops wall in WallpropsList)
                {
                    if (PolyList2.Contains(wall))
                    {
                        wall.Wall.name = "out";
                        DeleteList.Add(wall);
                        wall.Wall.tag = "out";
                        wall.Wall.layer = 8;
                    }
                }

            }
            //if Poly1 is smaller than Poly2
            else
            {

                Vector2 PlayerPosition = Player.transform.position;
                if (Horizontal)
                {
                    Player.transform.position = new Vector2(PlayerPosition.x, Newwallposition.y);
                    PlayerPosition.y = Newwallposition.y;
                    if (PolyArray1[0].y > PolyArray2[0].y)
                    {
                        //Poly1 is Top, Down is bigger


                        Instantiate(FillTop, new Vector2(PlayerPosition.x, PlayerPosition.y + 1.4f), Quaternion.identity);
                        if (FillWholeField)
                        {
                            Instantiate(FillDown, PlayerPosition, Quaternion.identity);
                        }
                        Player.transform.rotation = Quaternion.Euler(0, 0, 180);
                        NewWall.tag = "TopWalls";
                        NewWall.AddComponent<StopWallLT>();

                    }
                    else
                    {

                        //Poly1 is Down, Top is bigger


                        Instantiate(FillDown, new Vector2(PlayerPosition.x, PlayerPosition.y - 1.4f), Quaternion.identity);
                        if (FillWholeField)
                        {
                            Instantiate(FillTop, PlayerPosition, Quaternion.identity);
                        }
                        NewWall.AddComponent<StopWallRD>();
                        NewWall.tag = "DownWalls";
                    }
                }
                else
                {
                    Player.transform.position = new Vector2(Newwallposition.x, PlayerPosition.y);
                    PlayerPosition.x = Newwallposition.x;
                    if (PolyArray1[0].x < PolyArray2[0].x)
                    {
                        //Poly1 is Left, Right is bigger


                        Instantiate(FillLeft, new Vector2(PlayerPosition.x - 1.4f, PlayerPosition.y), Quaternion.identity);
                        if (FillWholeField)
                        {
                            Instantiate(FillRight, PlayerPosition, Quaternion.identity);
                        }
                        NewWall.tag = "LeftWalls";
                        NewWall.AddComponent<StopWallLT>();

                    }
                    else
                    {
                        //Poly1 is Right, Left is bigger


                        Instantiate(FillRight, new Vector2(PlayerPosition.x + 1.4f, PlayerPosition.y), Quaternion.identity);
                        if (FillWholeField)
                        {
                            Instantiate(FillLeft, PlayerPosition, Quaternion.identity);
                        }
                        Player.transform.rotation = Quaternion.Euler(0, 0, 90);
                        NewWall.AddComponent<StopWallRD>();
                        NewWall.tag = "RightWalls";
                    }
                    //  FillObject.GetComponent<FillColorLayer>().WalltoAttach = NewWall;
                }

                CurrentlyFilledArea.Add(Area1);
                PlayerPrefs.SetFloat("AreaFilled", PlayerPrefs.GetFloat("AreaFilled") + Area1);
                if (FillWholeField)
                {
                    CurrentlyFilledArea.Add(Area2);
                    PlayerPrefs.SetFloat("AreaFilled", PlayerPrefs.GetFloat("AreaFilled") + Area2);
                }

                foreach (Wallprops wall in WallpropsList)
                {
                    if (PolyList1.Contains(wall))
                    {
                        wall.Wall.name = "out";
                        DeleteList.Add(wall);
                        wall.Wall.tag = "out";
                        wall.Wall.layer = 8;
                    }
                }
            }

            // Für den nächsten Einsatz fertig machen

            //Destroy(Poly1);
            //Destroy(Poly2);



            Player.GetComponent<Animator>().SetBool("IsCollided", true);

            AssignValues(NewWall);




            PersistentManagerScript.Instance.CheckAchievements();
            NewWallX = 0;
            NewWallY = 0;
            DeleteList.Clear();


            ManageWalls.ActiveWalls.Clear();
            NumberPossible = true;
            PolyList1.Clear();
            PolyList2.Clear();
            Time.timeScale = 1;
            StopWallLT.TooClosetoWall = false;
            ManageUI.WallsGoSlower = false;
            ManageUI.WallsGoFaster = false;
            EnemiesAlive.Clear();
            swipegame.Leftwall = null;
            swipegame.RightWall = null;
            swipegame.TopWall = null;
            swipegame.DownWall = null;
            StopWallLT.Splitting = false;
            StopWallRD.Splitting = false;



            foreach (float fieldsize in CurrentlyFilledArea)
            {
                FilledAreafloat += fieldsize;
            }


            double v2Volume = FilledAreafloat;
            double PercentageFilled = Math.Round(100 - (100 * Math.Abs(FilledArea - v2Volume) / Math.Max(FilledArea, v2Volume)));

            float PercentageAdded = new float();
            PercentageAdded = Convert.ToSingle(PercentageFilled - LastPercentage);
            PercentageAdd.SetActive(true);
            if (FillWholeField)
            {
                PercentageAdd.GetComponent<Text>().text = "+ " + Convert.ToString(100 - ManageUI.percentageDiff) + "%";
            }
            else
            {
                PercentageAdd.GetComponent<Text>().text = "+ " + Convert.ToString(PercentageAdded) + "%";
            }


            ManageUI.percentageDiff = PercentageFilled;
            FilledAreafloat = 0;
            Destroy(swipegame.WallModul);
            if (CurrentlyFilledArea.Last() < 1100)
            {
                Destroy(Player, 1f);
            }
            else
            {
                Destroy(Player, 1.8f);
            }
            ManageUI Interface = GameObject.Find("Interface").GetComponent<ManageUI>();
            if (!PersistentManagerScript.Instance.NormalMode)
            {
                PointsAdded = Convert.ToInt32(Math.Round(CalculatePoints(PercentageAdded)) * 10);
                LastPercentage = PercentageFilled;
                PointsAdd.SetActive(true);
                PointsAdd.GetComponent<Text>().text = "+ " + Convert.ToString(PointsAdded);
                PersistentManagerScript.Instance.Points += PointsAdded;
                string Points = "Score " + Convert.ToString(PersistentManagerScript.Instance.Points);
                Interface.PointBanner.text = Points;
                Interface.ScoreOnPause.text = Points;
            }

            Interface.FillPercentage.text = Convert.ToString(ManageUI.percentageDiff) + "% Filled";
            if (swipegame._Ice != null)
            {
                swipegame._Ice.GetComponent<IceScript>().Unfreeze();
            }

        }

    }
}