using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;

public class Shortestpath3 : MonoBehaviour
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
    void CreateNewWAll()
    {
        NewWall = new GameObject();
        var sr = NewWall.AddComponent<SpriteRenderer>();
        sr.sprite = Sprite.Create(wallTex, new Rect(0.0f, 0.0f, wallTex.width, wallTex.height), new Vector2(0.5f, 0.5f));
        sr.color = Color.black;
        var newSize = new Vector3();

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
            newSize = new Vector3(0.77f, (StopWallLT.CollidedWallposition.x - 0.7f) - (StopWallRD.CollidedWallposition.x + 0.7f), 0);
            NewWall.transform.eulerAngles = new Vector3(0, 0, 90);
            NewWall.transform.position = new Vector2(((StopWallLT.CollidedWallposition.x - 0.7f) + (StopWallRD.CollidedWallposition.x + 0.7f)) / 2, NewWallY);

        }
        else
        {
            newSize = new Vector3(0.77f, (StopWallLT.CollidedWallposition.y + 0.7f) - (StopWallRD.CollidedWallposition.y - 0.7f), 0);
            NewWall.transform.position = new Vector2(NewWallX, ((StopWallLT.CollidedWallposition.y + 0.7f) + (StopWallRD.CollidedWallposition.y - 0.7f)) / 2);

        }

        NewWall.AddComponent<BoxCollider2D>();
        NewWall.AddComponent<BoxCollider2D>();
        foreach (BoxCollider2D collider in NewWall.GetComponents<BoxCollider2D>())
        {
            collider.sharedMaterial = JumpMaterial;
        }

        NewWall.GetComponent<BoxCollider2D>().isTrigger = true;

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
        var sr = wall.GetComponent<SpriteRenderer>();
        if (wall.tag == "TopWalls")
        {
            var ChainPos = Convert.ToSingle(wall.name);
            var Corner1 = new Vector2(wall.transform.position.x - sr.bounds.extents.x, wall.transform.position.y - sr.bounds.extents.y);
            var Corner2 = new Vector2(wall.transform.position.x + sr.bounds.extents.x, wall.transform.position.y - sr.bounds.extents.y);
            WallpropsList.Add(new Wallprops(wall, ChainPos, Corner1, Corner2));
        }
        else if (wall.tag == "LeftWalls")
        {
            var ChainPos = Convert.ToSingle(wall.name);
            var Corner1 = new Vector2(wall.transform.position.x + sr.bounds.extents.x, wall.transform.position.y - sr.bounds.extents.y);
            var Corner2 = new Vector2(wall.transform.position.x + sr.bounds.extents.x, wall.transform.position.y + sr.bounds.extents.y);
            WallpropsList.Add(new Wallprops(wall, ChainPos, Corner1, Corner2));
        }
        else if (wall.tag == "RightWalls")
        {
            var ChainPos = Convert.ToSingle(wall.name);
            var Corner1 = new Vector2(wall.transform.position.x - sr.bounds.extents.x, wall.transform.position.y + sr.bounds.extents.y);
            var Corner2 = new Vector2(wall.transform.position.x - sr.bounds.extents.x, wall.transform.position.y - sr.bounds.extents.y);
            WallpropsList.Add(new Wallprops(wall, ChainPos, Corner1, Corner2));
        }
        else if (wall.tag == "DownWalls")
        {
            var ChainPos = Convert.ToSingle(wall.name);
            var Corner1 = new Vector2(wall.transform.position.x + sr.bounds.extents.x, wall.transform.position.y + sr.bounds.extents.y);
            var Corner2 = new Vector2(wall.transform.position.x - sr.bounds.extents.x, wall.transform.position.y + sr.bounds.extents.y);
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
    List<Vector2[]> CurrentlyFilledArea = new List<Vector2[]>();
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




    void Update()
    {

        if (ManageWalls.AssignmentComplete && GameObject.Find("Player(Clone)") != null)
        {
            print("shortespath3");
            ManageWalls.AssignmentComplete = false;

            if (RayCastRotation.LastAxis == Axis.Horizontal)
            {
                Horizontal = true;
            }
            else
            {
                Horizontal = false;
            }


            GameObject Player = GameObject.Find("Player(Clone)");

            Player.tag = "out";
            WallpropsList.Clear();
            foreach (GameObject wall in ManageWalls.ActiveWalls)
                AssignValues(wall);



            EnemiesAlive.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
            int enemycount = new int();
            foreach (GameObject enemy in EnemiesAlive)
            {
                if (enemy.GetComponent<Enemy3Script>() == null)
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


            if (Horizontal)
            {
                LTPointNewWall = new Vector2(NewWall.transform.position.x - NewExtents.x, NewWall.transform.position.y);
                RDPointNewWall = new Vector2(NewWall.transform.position.x + NewExtents.x, NewWall.transform.position.y);

                LTOverlappingWalls = Physics2D.OverlapBoxAll(LTPointNewWall, new Vector2(0.5f, 0.5f), 0, GameObject.Find("Main Camera").GetComponent<ManageWalls>().PossibleWalls);
                RDOverlappingWalls = Physics2D.OverlapBoxAll(RDPointNewWall, new Vector2(0.5f, 0.5f), 0, GameObject.Find("Main Camera").GetComponent<ManageWalls>().PossibleWalls);


            }
            else
            {
                LTPointNewWall = new Vector2(NewWall.transform.position.x, NewWall.transform.position.y + NewExtents.y);
                RDPointNewWall = new Vector2(NewWall.transform.position.x, NewWall.transform.position.y - NewExtents.y);

                LTOverlappingWalls = Physics2D.OverlapBoxAll(LTPointNewWall, new Vector2(0.5f, 0.5f), 0, GameObject.Find("Main Camera").GetComponent<ManageWalls>().PossibleWalls);
                RDOverlappingWalls = Physics2D.OverlapBoxAll(RDPointNewWall, new Vector2(0.5f, 0.5f), 0, GameObject.Find("Main Camera").GetComponent<ManageWalls>().PossibleWalls);
            }


            NewWall.layer = 11;


            if (Horizontal)
            {
                foreach (Collider2D wall in LTOverlappingWalls)
                {
                    if (wall.gameObject.name != "out")
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
                    if (wall.gameObject.name != "out")
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
                    if (wall.gameObject.name != "out")
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
                    if (wall.gameObject.name != "out")
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
                        print("Poly2,RD");
                        PolyList1 = PolyList1.OrderBy(i => i.ChainPos, new MyComparer(LTNumber)).ToList();
                        PolyList2 = PolyList2.OrderBy(i => i.ChainPos, new MyComparer(LTNumber - 1)).ToList();
                    }
                    else
                    {
                        print("Poly2,LT");
                        PolyList1 = PolyList1.OrderBy(i => i.ChainPos, new MyComparer(LTNumber - 1)).ToList();
                        PolyList2 = PolyList2.OrderBy(i => i.ChainPos, new MyComparer(LTNumber)).ToList();
                    }
                }
                else
                {
                    if (LTNumber < RDNumber)
                    {
                        print("Poly1,RD");
                        PolyList1 = PolyList1.OrderBy(i => i.ChainPos, new MyComparer(RDNumber)).ToList();
                        PolyList2 = PolyList2.OrderBy(i => i.ChainPos, new MyComparer(RDNumber - 1)).ToList();
                    }
                    else
                    {
                        print("Poly1,LT");
                        PolyList1 = PolyList1.OrderBy(i => i.ChainPos, new MyComparer(LTNumber)).ToList();
                        PolyList2 = PolyList2.OrderBy(i => i.ChainPos, new MyComparer(LTNumber - 1)).ToList();
                    }
                }
            }

            // transforms PolyLists in Arrays
            Vector2[] PolyArray1 = new Vector2[PolyList1.Count + 1];
            Vector2[] PolyArray2 = new Vector2[PolyList2.Count + 1];

            List<Vector2> PastPolyList1 = new List<Vector2>();
            List<Vector2> PastPolyList2 = new List<Vector2>();


            foreach (Wallprops wall in PolyList1)
            {
                PastPolyList1.Add(wall.Corner2);
            }

            foreach (Wallprops wall in PolyList2)
            {
                PastPolyList2.Add(wall.Corner2);
            }

            // adds missing Colliderpoint
            if (Horizontal)
            {
                if (PolyArray1[0].y > PolyArray2[0].y)
                {
                    PastPolyList1.Add(StopWallLT.CollisionPos);
                    PastPolyList2.Add(StopWallRD.CollisionPos);
                }
                else
                {
                    PastPolyList1.Add(StopWallRD.CollisionPos);
                    PastPolyList2.Add(StopWallLT.CollisionPos);
                }
            }
            else
            {
                if (PolyArray1[0].x < PolyArray2[0].x)
                {
                    PastPolyList1.Add(StopWallRD.CollisionPos);
                    PastPolyList2.Add(StopWallLT.CollisionPos);
                }
                else
                {
                    PastPolyList1.Add(StopWallLT.CollisionPos);
                    PastPolyList2.Add(StopWallRD.CollisionPos);
                }

            }

            for (int i = 0; i < PolyList1.Count + 1; i++)
                PolyArray1[i] = PastPolyList1[i];


            for (int i = 0; i < PolyList2.Count + 1; i++)
                PolyArray2[i] = PastPolyList2[i];


            //creates PolygonCollider
            //var Poly1 = new GameObject();
            //Poly1.name = "Poly1";
            //Poly1.AddComponent<PolygonCollider2D>();
            //Poly1.GetComponent<PolygonCollider2D>().isTrigger = true;

            //var Poly2 = new GameObject();
            //Poly2.name = "Poly2";
            //Poly2.AddComponent<PolygonCollider2D>();
            //Poly2.GetComponent<PolygonCollider2D>().isTrigger = true;

            //Poly1.GetComponent<PolygonCollider2D>().points = PolyArray1;
            //Poly2.GetComponent<PolygonCollider2D>().points = PolyArray2;
            // Größeren Flächeninhalt  bestimmen
            float Area1 = CalculatePolygonArea(PolyArray1);
            float Area2 = CalculatePolygonArea(PolyArray2);



            if (Area1 > Area2)
            {

                Vector2 PlayerPosition = Player.transform.position;


                if (Horizontal)
                {
                    Player.transform.position = new Vector2(PlayerPosition.x, NewWall.transform.position.y);
                    PlayerPosition.y = NewWall.transform.position.y;
                    if (PolyArray1[0].y > PolyArray2[0].y)
                    {
                        //Poly1 is Top, Top is bigger


                        Instantiate(FillDown, new Vector2(PlayerPosition.x, PlayerPosition.y - 0.4f), Quaternion.identity);
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


                        Instantiate(FillTop, new Vector2(PlayerPosition.x, PlayerPosition.y + 0.4f), Quaternion.identity);
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
                    Player.transform.position = new Vector2(NewWall.transform.position.x, PlayerPosition.y);
                    PlayerPosition.x = NewWall.transform.position.x;
                    if (PolyArray1[0].x < PolyArray2[0].x)
                    {
                        //Poly1 is Left, Left is bigger


                        Instantiate(FillRight, new Vector2(PlayerPosition.x + 0.4f, PlayerPosition.y), Quaternion.identity);
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


                        Instantiate(FillLeft, new Vector2(PlayerPosition.x - 0.4f, PlayerPosition.y), Quaternion.identity);
                        if (FillWholeField)
                        {
                            Instantiate(FillRight, PlayerPosition, Quaternion.identity);
                        }
                        NewWall.tag = "LeftWalls";
                        NewWall.AddComponent<StopWallLT>();

                    }

                    //  FillObject.GetComponent<FillColorLayer>().WalltoAttach = NewWall;
                }
                CurrentlyFilledArea.Add(PolyArray2);
                if (FillWholeField)
                {
                    CurrentlyFilledArea.Add(PolyArray1);
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
                //if (Area1 < 1700 || Area2 < 1700)
                //{
                //    StartCoroutine(StopAnimation(1.8f, FillObject));
                //}
            }
            //if Poly1 is smaller than Poly2
            else
            {
                Vector2 PlayerPosition = Player.transform.position;
                if (Horizontal)
                {
                    Player.transform.position = new Vector2(PlayerPosition.x, NewWall.transform.position.y);
                    PlayerPosition.y = NewWall.transform.position.y;
                    if (PolyArray1[0].y > PolyArray2[0].y)
                    {
                        //Poly1 is Top, Down is bigger


                        Instantiate(FillTop, new Vector2(PlayerPosition.x, PlayerPosition.y + 0.4f), Quaternion.identity);
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


                        Instantiate(FillDown, new Vector2(PlayerPosition.x, PlayerPosition.y - 0.4f), Quaternion.identity);
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
                    Player.transform.position = new Vector2(NewWall.transform.position.x, PlayerPosition.y);
                    PlayerPosition.x = NewWall.transform.position.x;
                    if (PolyArray1[0].x < PolyArray2[0].x)
                    {
                        //Poly1 is Left, Right is bigger


                        Instantiate(FillLeft, new Vector2(PlayerPosition.x - 0.4f, PlayerPosition.y), Quaternion.identity);
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


                        Instantiate(FillRight, new Vector2(PlayerPosition.x + 0.4f, PlayerPosition.y), Quaternion.identity);
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
                CurrentlyFilledArea.Add(PolyArray1);
                if (FillWholeField)
                {
                    CurrentlyFilledArea.Add(PolyArray2);
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

            foreach (Wallprops wall in DeleteList)
                WallpropsList.Remove(wall);

            Player.GetComponent<Animator>().SetBool("IsCollided", true);

            AssignValues(NewWall);
            var newProps = WallpropsList[WallpropsList.Count - 1];
            WallpropsList.RemoveAt(WallpropsList.Count - 1);
            WallpropsList.Add(newProps);




            NewWallX = 0;
            NewWallY = 0;
            DeleteList.Clear();
            StopWallLT.IsSplittableLT = true;
            StopWallRD.IsSplittableRD = true;
            StopWallLT.CollidedWallposition = new Vector2(0, 0);
            StopWallRD.CollidedWallposition = new Vector2(0, 0);
            ManageWalls.ActiveWalls.Clear();
            NumberPossible = true;
            PolyList1.Clear();
            PolyList2.Clear();
            Time.timeScale = 1;
            StopWallLT.TooClosetoWall = false;
            ManageUI.WallsGoSlower = false;
            ManageUI.WallsGoFaster = false;
            EnemiesAlive.Clear();
            foreach (Vector2[] area in CurrentlyFilledArea)
            {
                FilledAreafloat += CalculatePolygonArea(area);
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

            PointsAdded = Convert.ToInt32(Math.Round(CalculatePoints(PercentageAdded)) * 10);
            LastPercentage = PercentageFilled;
            PointsAdd.SetActive(true);
            PointsAdd.GetComponent<Text>().text = "+ " + Convert.ToString(PointsAdded);
            PersistentManagerScript.Instance.Points += PointsAdded;
            ManageUI.percentageDiff = PercentageFilled;
            FilledAreafloat = 0;
            Destroy(GameObject.Find("WallModul(Clone)"));
            if (CalculatePolygonArea(CurrentlyFilledArea.Last()) < 1100)
            {
                Destroy(Player, 1.5f);
            }
            else
            {
                Destroy(Player, 2.5f);
            }

            Destroy(GameObject.Find("PreviewHoriz(Clone)"), 0.1f);
        }

    }
}