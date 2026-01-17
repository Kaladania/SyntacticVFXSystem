using SnSECS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
//using UnityEngine.Windows;

namespace DataRecorder
{


    public class Recorder : MonoBehaviour
    {
        [SerializeField]
        int _ID = 0;
        string filePath = string.Empty;
        /*[SerializeField]
        int _ID = 0;

        [SerializeField]
        string*/

        public void Start()
        {
            
        }

        public void SetupRecorder(int id)
        {
            filePath = "Assets/Testing Results/" + _ID + ".txt";

            //if a file with the same name already exists, raises an error and alters the file path name

            while (File.Exists(filePath))
            {
                filePath = "Assets/Testing Results/" + _ID + "_" + DateTime.Now.ToShortTimeString() + ".txt";
                filePath = filePath.Replace(":", "-");
                Debug.LogError($"ERROR: A file already exists with this name. ID may be incorrect. Creating a new file with name: {filePath} ");
            }

            WriteToNewFile($"Participant ID: {_ID}");
        }
        
        /// <summary>
        /// Creates a new file and writes to it
        /// </summary>
        /// <param name="text">Text to write to the file</param>
        public void WriteToNewFile(string text)
        {
            if (!File.Exists(filePath))
            {
                var sw = File.CreateText(filePath);
                sw.WriteLineAsync(text + Environment.NewLine);
                sw.Close();
            }
            /*sr.WriteLine(text);
            sr.Close();*/
        }

        /// <summary>
        /// Writes to the pre-existing text file
        /// </summary>
        /// <param name="text">Text to write</param>
        public void WriteToFile(string text)
        {
            using (StreamWriter sw = File.AppendText(filePath))
            {
                sw.WriteLine(text + Environment.NewLine);
            }
        }

        /// <summary>
        /// Converts a combo to a CSV string and writes it to a file
        /// </summary>
        /// <param name="combo">The combo to convert</param>
        public void WriteComboToFile(List<Elements> combo)
        {
            string comboString = string.Empty;
            foreach (Elements element in combo)
            {
                comboString += element.ToString() + ",";
            }

            WriteToFile(comboString);
        }


    }

}
