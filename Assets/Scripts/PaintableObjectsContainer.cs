using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Painter3D
{
    public class PaintableObjectsContainer : MonoBehaviour
    {

        [SerializeField] private Button cleanButton;
        [SerializeField] private Button saveButton;
        [SerializeField] private Button loadButton;

        private static List<PaintableObject> paintableObjects = new List<PaintableObject>();
        private string fileDirectory;
        private string filePath;

        private void Awake()
        {
            fileDirectory = Application.persistentDataPath + "/SavedDrawedTextures";

            cleanButton.onClick.AddListener(CleanTheDrawing);
            saveButton.onClick.AddListener(SaveTheDrawing);
            loadButton.onClick.AddListener(LoadSavedDrawing);
        }


        public static void AddPaintableObjectToList(PaintableObject paintableObject)
        {
            paintableObjects.Add(paintableObject);
        }
        public static void RemovePaintableObjectToList(PaintableObject paintableObject)
        {
            paintableObjects.Remove(paintableObject);
        }

        public void CleanTheDrawing()
        {
            foreach (var paintableObj in paintableObjects)
            {
                paintableObj.Clear();
            }
        }

        public void SaveTheDrawing()
        {
            CheckFilesDirectory();

            foreach (var paintableObj in paintableObjects)
            {
                if (paintableObj.IsObjectWasPainted)
                {

                    Texture2D texture = paintableObj.GetPaintedTexture();
                    filePath = $"{fileDirectory}/{paintableObj.gameObject.GetInstanceID()}.png";
                    File.WriteAllBytes(filePath, texture.EncodeToPNG());
                }
            }
        }

        private void CheckFilesDirectory()
        {
            if (!Directory.Exists(fileDirectory))
            {
                Directory.CreateDirectory(fileDirectory);
            }
            //Debug.Log(fileDirectory);
        }

        public void LoadSavedDrawing()
        {
            CheckFilesDirectory();

            foreach (var paintableObj in paintableObjects)
            {
                filePath = $"{fileDirectory}/{paintableObj.gameObject.GetInstanceID()}.png";

                if (File.Exists(filePath))
                {
                    Texture2D texture = paintableObj.GetPaintedTexture();
                    texture.LoadImage(File.ReadAllBytes(filePath));
                    texture.Apply(false);
                    paintableObj.SetPaintedTexture(texture);
                }
                //Texture2D texture = paintableObj.GetPaintedTexture();

                //File.WriteAllBytes($"{fileDirectory}/{paintableObj.gameObject.GetInstanceID()}.png", texture.EncodeToPNG());
            }
        }

        private void OnDestroy()
        {
            cleanButton.onClick.RemoveListener(CleanTheDrawing);
            saveButton.onClick.RemoveListener(SaveTheDrawing);
            loadButton.onClick.RemoveListener(LoadSavedDrawing);

            paintableObjects.Clear();
        }
    }
}