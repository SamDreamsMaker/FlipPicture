using Aspose.Imaging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class FlitPicture : MonoBehaviour
{
    [SerializeField]
    GameObject message;
    static string subFolderToFlip = "toFlip";
    static string subFolderFlipped= "flipped";
    [SerializeField]
    Toggle rescale1820Toggle;
    [SerializeField]
    Button flipButton;
    public void FlipPictures()
    {
        /*
        string licenseFile = Path.Combine(Application.streamingAssetsPath, "Aspose.Total.lic");
        License license = new License();
        license.SetLicense(licenseFile);
        */

        flipButton.interactable = false;
        DisableMessage();
        List<string> extensions = new List<string>();
        extensions.Add("*.png");
        extensions.Add("*.jpg");
        extensions.Add("*.jpeg");
        extensions.Add("*.bmp");
        extensions.Add("*.tiff");
        extensions.Add("*.apng");
        extensions.Add("*.jpeg2000");
        extensions.Add("*.gif");

        if (Directory.Exists(Application.streamingAssetsPath)) {
            string worldsFolder = Path.Combine(Application.streamingAssetsPath, subFolderToFlip);
            Debug.Log(worldsFolder);
            DirectoryInfo d = new DirectoryInfo(worldsFolder);
            foreach (string e in extensions) {
                foreach (FileInfo file in d.GetFiles(e, SearchOption.AllDirectories)) {
                    string destination = file.FullName.Replace(subFolderToFlip, subFolderFlipped).Replace(file.Name,"");
                    ReversePixels(file.FullName, destination, file.Name);
                }
            }
        }
        message.SetActive(true);
        flipButton.interactable = true;
        Invoke("DisableMessage", 5.0f);
    }
    void DisableMessage() {
        message.SetActive(false);
    }
    void Flip(string fileLocation, string destination, string nameFile,string extension)
    {
        using (Aspose.Imaging.Image image = Aspose.Imaging.Image.Load(fileLocation)) {
            image.RotateFlip(RotateFlipType.RotateNoneFlipX);
            if (rescale1820Toggle.isOn) {
                image.ResizeWidthProportionally(1820);
            }
            image.Save(destination);
        }
    }

    void ReversePixels(string fileLocation, string destination, string fileName) {
        byte[] fileData = File.ReadAllBytes(fileLocation);

        // Créer la texture à partir des données brutes
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(fileData);

        // Obtenir les pixels de la texture
        UnityEngine.Color[] pixels = texture.GetPixels();

        // Inverser l'ordre des pixels horizontalement
        for (int y = 0; y < texture.height; y++) {
            Array.Reverse(pixels, y * texture.width, texture.width);
        }

        // Recréer la texture avec les pixels inversés
        Texture2D flippedTexture = new Texture2D(texture.width, texture.height);
        flippedTexture.SetPixels(pixels);
        flippedTexture.Apply();

        // Sauvegarder la texture inversée en JPG
        byte[] jpgData = flippedTexture.EncodeToJPG();
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);

        if (!Directory.Exists(destination)) {
            // Créer le dossier s'il n'existe pas
            Directory.CreateDirectory(destination);
        }
        string filePath = Path.Combine(destination, fileNameWithoutExtension+".jpeg");
        File.WriteAllBytes(filePath, jpgData);
    }

}
