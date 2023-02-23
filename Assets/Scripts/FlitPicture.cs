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
    [SerializeField]
    Text workingOnText;
    public void FlipPictures()
    {
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
            string directoryToExplore = Path.Combine(Application.streamingAssetsPath, subFolderToFlip);
            if (!Directory.Exists(directoryToExplore)) Directory.CreateDirectory(directoryToExplore);
            DirectoryInfo d = new DirectoryInfo(directoryToExplore);
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
    public void FlipProcess() {
        StartCoroutine(FlipPicturesAsync());
    }
    IEnumerator FlipPicturesAsync() {
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
            string directoryToExplore = Path.Combine(Application.streamingAssetsPath, subFolderToFlip);
            if (!Directory.Exists(directoryToExplore)) Directory.CreateDirectory(directoryToExplore);
            DirectoryInfo d = new DirectoryInfo(directoryToExplore);
            foreach (string e in extensions) {
                foreach (FileInfo file in d.GetFiles(e, SearchOption.AllDirectories)) {
                    UpdateWorkingOnText("Creating file " + file.Name);
                    yield return new WaitForSeconds(Time.deltaTime);
                    string destination = file.FullName.Replace(subFolderToFlip, subFolderFlipped).Replace(file.Name, "");
                    ReversePixels(file.FullName, destination, file.Name);
                }
            }
        }
        UpdateWorkingOnText("");
        message.SetActive(true);
        flipButton.interactable = true;
        Invoke("DisableMessage", 5.0f);
    }
    void DisableMessage() {
        message.SetActive(false);
    }
    void UpdateWorkingOnText(string text) {
        workingOnText.text = text;
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
        Texture2D flippedTexture;
        if (rescale1820Toggle.isOn) {
            flippedTexture = ResizeTextureKeepRatio(texture, 1820);
        } else {
            flippedTexture = new Texture2D(texture.width, texture.height);
            flippedTexture.SetPixels(pixels);
            flippedTexture.Apply();
        }

        // Sauvegarder la texture inversée en JPG
        byte[] jpgData = flippedTexture.EncodeToJPG();
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);

        if (!Directory.Exists(destination)) Directory.CreateDirectory(destination);
        string filePath = Path.Combine(destination, fileNameWithoutExtension+".jpeg");
        File.WriteAllBytes(filePath, jpgData);
        Destroy(texture);
        Destroy(flippedTexture);
    }
    public static Texture2D ResizeTextureKeepRatio(Texture2D texture, int targetWidth) {
        Texture2D flippedTexture = new Texture2D(texture.width, texture.height);
        for (int i = 0; i < texture.height; i++) {
            for (int j = 0; j < texture.width; j++) {
                Color pixel = texture.GetPixel(j, i);
                flippedTexture.SetPixel(texture.width - j - 1, i, pixel);
            }
        }
        flippedTexture.Apply();

        int targetHeight = (int)((float)flippedTexture.height / (float)flippedTexture.width * (float)targetWidth);
        Texture2D result = new Texture2D(targetWidth, targetHeight, TextureFormat.RGBA32, false);
        float incX = ((float)1 / flippedTexture.width) * ((float)flippedTexture.width / (float)targetWidth);
        float incY = ((float)1 / flippedTexture.height) * ((float)flippedTexture.width / (float)targetWidth);
        for (int i = 0; i < result.height; ++i) {
            for (int j = 0; j < result.width; ++j) {
                Color newColor = flippedTexture.GetPixelBilinear((float)j / (float)result.width, (float)i / (float)result.height);
                result.SetPixel(j, i, newColor);
            }
        }
        result.Apply();
        Destroy(flippedTexture);
        return result;
    }
}
