using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json; // See Third Party Libs/Credits.txt for licensing information for JSON.Net
using Newtonsoft.Json.Linq;

namespace CPU_OS_Simulator.Save_File_Editor
{
    public class SaveFileParser : IDisposable
    {
        private string filePath;
        private JObject obj;
        private StreamReader reader;
        private JsonTextReader jsonReader;
        private JObject obj2;
        private StreamWriter writer;

        public string FilePath
        {
            get { return filePath; }
        }

        public JObject Obj
        {
            get { return obj; }
        }

        public StreamReader Reader
        {
            get { return reader; }
        }

        public JsonTextReader JsonReader
        {
            get { return jsonReader; }
        }

        public JObject Obj2
        {
            get { return obj2; }
        }

        public StreamWriter Writer
        {
            get { return writer; }
        }


        public SaveFileParser(string file)
        {
            filePath = file;
        }
        public void ParseFile(string path)
        {
            obj = JObject.Parse(File.ReadAllText(path, Encoding.UTF8));
            reader = File.OpenText(path);
            jsonReader = new JsonTextReader(reader);
            obj2 = (JObject)JToken.ReadFrom(jsonReader);
            writer = new StreamWriter("Text.json", false);
            string json = obj2.ToString();
            writer.Write(json);
            reader.Close();
            writer.Flush();
            writer.Close();

            MessageBox.Show("reader " + path + " Successfully Parsed");
        }

        public void ParseFileNoWhitespace(string path)
        {
            obj = JObject.Parse(File.ReadAllText(path, Encoding.UTF8));
            reader = File.OpenText(path);
            jsonReader = new JsonTextReader(reader);
            obj2 = (JObject)JToken.ReadFrom(jsonReader);
            writer = new StreamWriter("Text.json", false);
            string json = obj2.ToString();
            json = Regex.Replace(json, "(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+", "$1");
            writer.Write(json);
            reader.Close();
            writer.Flush();
            writer.Close();

            MessageBox.Show("reader " + path + " Successfully Parsed");
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            reader.Dispose();
            writer.Dispose();
        }
    }
}
