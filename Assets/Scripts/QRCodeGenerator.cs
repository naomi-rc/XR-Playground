using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using ZXing.QrCode;
using UnityEngine.UI;
using TMPro;
using System;
using System.IO;

public class QRCodeGenerator : MonoBehaviour
{
    [SerializeField] private RawImage rawImageReceiver;
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TMP_InputField bioInputField;
    [SerializeField] private TMP_InputField avatarIDInputField;
    [SerializeField] private TMP_InputField linkedInInputField;
    [SerializeField] private TMP_InputField websteInputField;
    [SerializeField] private GameObject toastGameObject;

    private Texture2D encodedTexture;
    private string downloadsFolderPath;
    BusinessCardInfo businessCard;

    private DateTime startTime;
    private bool QRCodepressed;
    private int DOWNLOAD_PRESS_DELAY = 2;


    void Start()
    {
        downloadsFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        downloadsFolderPath = Path.Combine(downloadsFolderPath, "Downloads");

        Debug.Log("Downloads Folder Path: " + downloadsFolderPath);

        encodedTexture = new Texture2D(256, 256);
    }
    private void Update()
    {
        if (QRCodepressed)
        {
            System.TimeSpan timeSpan = System.DateTime.Now - startTime;

            if (timeSpan.Seconds > DOWNLOAD_PRESS_DELAY)
            {
                DownloadQRCodeAsImage();
            }
        }
    }

    private Color32[] Encode(string encodingText, int width, int height)
    {
        BarcodeWriter writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = height,
                Width = width
            }
        };

        return writer.Write(encodingText);
    }

    private void EncodeTextToQRCode()
    {
        if (string.IsNullOrEmpty(nameInputField.text))
        {
            ShowInputError(nameInputField, "Please enter a name.");
            return;
        }

        RemoveInputErrors(nameInputField);

        businessCard = new BusinessCardInfo(
            nameInputField.text,
            bioInputField.text,
            avatarIDInputField.text,
            linkedInInputField.text,
            websteInputField.text
            );

        string businessCardInfoText = businessCard.SaveToString();
        
        Debug.Log("Object info: " + businessCard.m_name + " " + businessCard.m_bio + " " + businessCard.m_avatarID + " " + businessCard.m_linkedIn + " " + businessCard.m_website);
        Debug.Log("JSON info: " + businessCardInfoText);

        Color32[] convertPixelToTexture = Encode(businessCardInfoText, encodedTexture.width, encodedTexture.height);
        encodedTexture.SetPixels32(convertPixelToTexture);
        encodedTexture.Apply();

        rawImageReceiver.texture = encodedTexture;
    }

    public void OnClickEncode()
    {
        EncodeTextToQRCode();
    }

    private void ShowInputError(TMP_InputField inputField, string errorMessage)
    {
        TMP_Text placeholder = inputField.placeholder.GetComponent<TMP_Text>();
        placeholder.text = errorMessage;
        inputField.GetComponentInParent<Image>().color = Color.red;
    }

    private void RemoveInputErrors(TMP_InputField inputField)
    {
        inputField.GetComponentInParent<Image>().color = Color.white;
    }
    
    public void OnPressQRCode()
    {
        startTime = System.DateTime.Now;
        QRCodepressed = true;
    }

    public void DownloadQRCodeAsImage()
    {
        QRCodepressed = false;
        byte[] pgnBytes = encodedTexture.EncodeToPNG();

        string pathName = downloadsFolderPath + "\\QRCode.png";
        Debug.Log("PATHNAME" + pathName);

        if (!File.Exists(pathName))
            File.WriteAllBytes(pathName, pgnBytes);
        else
            File.Delete(pathName);


        if (File.Exists(pathName))
        {            
            toastGameObject.GetComponentInChildren<TMP_Text>().text = "QR code downloaded successfully!";
            toastGameObject.GetComponent<Image>().color = Color.green;
            toastGameObject.SetActive(true);
            Invoke("DeactivateToastMessage", 3);
            Debug.Log("QR code downloaded successfully!");
        }
        else
        {
            toastGameObject.GetComponentInChildren<TMP_Text>().text = "QR code failed to download.";
            toastGameObject.GetComponent<Image>().color = Color.red;
            toastGameObject.SetActive(true);
            Invoke("DeactivateToastMessage", 3);
            Debug.LogWarning("QR code failed to download");
        }
    }

    private void DeactivateToastMessage()
    {
        toastGameObject.SetActive(false);
    }
}
