using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public class RatePopup : MonoBehaviour
{
    public GameObject Button1;
    public GameObject Button2;
    public GameObject Button3;
    public GameObject Text;
    bool Likedgame = false;
    bool DislikeGame = false;
    bool WantWrite = false;
    public GameObject Input;

    // Use this for initialization
    void Start()
    {
        GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        GetComponent<Canvas>().worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Btn1()
    {
        if (!DislikeGame && !Likedgame)
        {
            Text.GetComponent<Text>().text = "I would appreciate it, if you could give me a review";
            Button1.GetComponentInChildren<Text>().text = "Okay";
            Button2.GetComponentInChildren<Text>().text = "Maybe Later";
            Button3.SetActive(true);
            Button3.GetComponentInChildren<Text>().text = "Never show again";
            Likedgame = true;
        }
        else if (Likedgame)
        {
            Application.OpenURL("https://play.google.com/store/apps/details?id=com.EnterRavement.Splitterz&reviewId=0");
            Destroy(gameObject);
            PlayerPrefs.SetInt("RateMenu", 1);
        }
        else if (DislikeGame)
        {
            WantWrite = true;
            Input.SetActive(true);
            Text.SetActive(false);
            Button1.SetActive(false);
            Button2.GetComponentInChildren<Text>().text = "Send";
            Button3.SetActive(true);
            Button3.GetComponentInChildren<Text>().text = "Cancel";
            // write improvements


        }


    }

    public void Btn2()
    {
        if (!Likedgame && !DislikeGame)
        {

            Text.GetComponent<Text>().text = "Would you like to tell me what you did not like?";
            DislikeGame = true;
        }
        else if (Likedgame && !WantWrite)
        {
            // maybe later
            Destroy(gameObject);
            PlayerPrefs.SetInt("RateMenu", 2);
        }
        else if (WantWrite && DislikeGame)
        {
            if (PlayerPrefs.GetInt("RateMenu") == 1)
            {
                Destroy(gameObject);
            }
            PlayerPrefs.SetInt("RateMenu", 1);
            if (Input.GetComponent<InputField>().text != "" | Input.GetComponent<InputField>().text != " ")
            {
                SendEmails(Input.GetComponent<InputField>().text);
            }


        }
        else if (DislikeGame && !WantWrite)
        {
            Destroy(gameObject);
            PlayerPrefs.SetInt("RateMenu", 1);
        }



    }


    public void Btn3()
    {

        Destroy(gameObject);
        PlayerPrefs.SetInt("RateMenu", 1);

    }


    private MailMessage mail = new MailMessage();

    void SendEmails(string text)
    {
        Button2.GetComponentInChildren<Text>().text = "loading...";
        mail.From = new MailAddress("enterravementfeedback@gmail.com");
        mail.To.Add("enterravement@gmail.com");

        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
        smtpServer.Port = 587;//GIVE CORRECT PORT HERE
        mail.Subject = "Feedback Splitterz";
        mail.Body = text;
        smtpServer.Credentials = new System.Net.NetworkCredential("enterravementfeedback@gmail.com", "feedback") as ICredentialsByHost;
        smtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback =
        delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        { return true; };
        smtpServer.Send(mail);
        //smtpServer.SendAsync(mail)

        Destroy(gameObject);
    }


}
