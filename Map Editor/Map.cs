using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.IO.Packaging;


namespace Map_Editor
{
    public class Map
    {
        XmlWriter writer;

        public Map()
        {

        }
        public bool Load()
        {
            return false;
        }
        public bool Save()
        {
            Stream str;
            XmlWriterSettings settings = new XmlWriterSettings();
            
            Directory.CreateDirectory(MapEditor.mapName);
            str = File.Create(MapEditor.mapName + "//" + MapEditor.mapName + ".textures");
            settings.Indent = true;
            // settings.NewLineOnAttributes = true;
            //settings.ConformanceLevel = ConformanceLevel.Auto;
            writer = XmlWriter.Create(str, settings);


            writer.WriteStartDocument();

            #region Sprites
            writer.WriteStartElement("Sprites");

            #region Textures
            writer.WriteStartElement("Textures");
            foreach (TileInfo item in MapEditor.TileList)
            {

                writer.WriteStartElement("Texture");
                writer.WriteStartAttribute("Index");
                writer.WriteValue(MapEditor.TileList.IndexOf(item));
                writer.WriteEndAttribute();
                writer.WriteStartAttribute("Collision");
                writer.WriteValue(item.collision);
                writer.WriteEndAttribute();
                writer.WriteValue(item.texture.Name);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            #endregion

            #region Objects
            writer.WriteStartElement("Objects");
            foreach (TileInfo item in MapEditor.ObjectList)
            {
                writer.WriteStartElement("Object");
                writer.WriteStartAttribute("Index");
                writer.WriteValue(MapEditor.ObjectList.IndexOf(item));
                writer.WriteEndAttribute();
                writer.WriteStartAttribute("Collision");
                writer.WriteValue(item.collision);
                writer.WriteEndAttribute();
                writer.WriteValue(item.texture.Name);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            #endregion

            #region Characters
            writer.WriteStartElement("Characters");
            foreach (CharacterInfo item in MapEditor.GlobalCharacterList)
            {
                writer.WriteStartElement("Character");
                writer.WriteStartAttribute("Index");
                writer.WriteValue(MapEditor.GlobalCharacterList.IndexOf(item));
                writer.WriteEndAttribute();
                writer.WriteStartAttribute("Category");
                writer.WriteValue(item.category.ToString());
                writer.WriteEndAttribute();
                writer.WriteStartAttribute("PosX");
                writer.WriteValue(item.startingPosition.X);
                writer.WriteEndAttribute();
                writer.WriteStartAttribute("PosY");
                writer.WriteValue(item.startingPosition.Y);
                writer.WriteEndAttribute();
                writer.WriteValue(item.name);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            #endregion

            writer.WriteEndDocument();
            writer.Close();

            #endregion

            AddFileToZip(MapEditor.mapName + "//" + MapEditor.mapName + ".map", MapEditor.mapName + "//" + MapEditor.mapName + ".textures");
            //zipStream.CreatePart(new Uri(MapEditor.mapName + "//" + MapEditor.mapName + ".textures"), "text");

            str = File.Create(MapEditor.mapName + "//" + MapEditor.mapName + ".events");

            writer = XmlWriter.Create(str, settings);

            writer.WriteStartDocument();

            #region Events
            writer.WriteStartElement("Events");
            foreach (EventInfo item in MapEditor.GlobalEventList)
            {
                writer.WriteStartElement("Event");
                writer.WriteStartAttribute("Name"); writer.WriteValue(item.name);
                writer.WriteStartAttribute("Category"); writer.WriteValue(item.category.ToString());
                writer.WriteStartAttribute("Time"); writer.WriteValue(item.time);
                writer.WriteStartAttribute("TileX"); writer.WriteValue(item.tileIndex.X);
                writer.WriteStartAttribute("TileY"); writer.WriteValue(item.tileIndex.Y);
                writer.WriteStartElement("Variable"); writer.WriteValue(item.variable);
                writer.WriteEndElement();
                writer.WriteValue(item.value);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();

            #endregion

            AddFileToZip(MapEditor.mapName + "//" + MapEditor.mapName + ".map", MapEditor.mapName + "//" + MapEditor.mapName + ".events");

            //zipStream.CreatePart(new Uri(MapEditor.mapName + "//" + MapEditor.mapName + ".events"), "text");

            str = File.Create(MapEditor.mapName + "//" + MapEditor.mapName + ".tmap");

            writer = XmlWriter.Create(str, settings);

            writer.WriteStartDocument();

            #region Map
            writer.WriteStartElement("Map");
            {
                {
                    writer.WriteStartElement("Tiles");
                    {
                        writer.WriteStartAttribute("X");
                        writer.WriteValue(MapEditor.numberOfTiles.X);
                        writer.WriteEndAttribute();
                    }
                    {
                        writer.WriteStartAttribute("Y");
                        writer.WriteValue(MapEditor.numberOfTiles.Y);
                        writer.WriteEndAttribute();
                    }
                    writer.WriteEndElement();
                }
                {
                    writer.WriteStartElement("TileMap");

                    for (int i = 0; i < MapEditor.numberOfTiles.X; i++)
                    {
                        writer.WriteStartElement("Row");

                        for (int j = 0; j < MapEditor.numberOfTiles.Y; j++)
                        {
                            foreach (TileInfo item in MapEditor.TileList)
                            {
                                if (MapEditor.TileMap[i, j] != null)
                                {
                                    if (item.texture == MapEditor.TileMap[i, j])
                                    {
                                        writer.WriteValue(MapEditor.TileList.IndexOf(item));
                                        writer.WriteValue("");
                                    }
                                }
                                else
                                {
                                    writer.WriteValue(-1);
                                    writer.WriteValue("");
                                }

                            }
                           
                        }

                        writer.WriteEndElement();
                    }
                }
                {
                    writer.WriteStartElement("ObjectMap");
                    {

                        for (int i = 0; i < MapEditor.numberOfTiles.X; i++)
                        {
                            writer.WriteStartElement("Row");

                            for (int j = 0; j < MapEditor.numberOfTiles.Y; j++)
                            {
                                foreach (TileInfo item in MapEditor.ObjectList)
                                {
                                    if (MapEditor.ObjectMap[i, j] != null)
                                    {
                                        if (item.texture == MapEditor.ObjectMap[i, j])
                                        {
                                            writer.WriteValue(MapEditor.ObjectList.IndexOf(item));
                                            writer.WriteValue("");
                                        }
                                    }
                                    else
                                    {
                                        writer.WriteValue(-1);
                                        writer.WriteValue("");
                                    }

                                }
                                
                            }

                            writer.WriteEndElement();
                        }
                    }
                }

                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            //writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
            #endregion

            AddFileToZip(MapEditor.mapName + "//" + MapEditor.mapName + ".map", MapEditor.mapName + "//" + MapEditor.mapName + ".tmap");

            //zipStream.CreatePart(new Uri(MapEditor.mapName + "//" + MapEditor.mapName + ".map"), "text");

            System.Windows.Forms.MessageBox.Show("Map Saved!", "Information", System.Windows.Forms.MessageBoxButtons.OK);

            
            

            return false;
        }

        private const long BUFFER_SIZE = 4096;

        private static void AddFileToZip(string zipFilename, string fileToAdd)
        {
            using (Package zip = System.IO.Packaging.Package.Open(zipFilename, FileMode.OpenOrCreate))
            {
                string destFilename = ".\\" + Path.GetFileName(fileToAdd);
                Uri uri = PackUriHelper.CreatePartUri(new Uri(destFilename, UriKind.Relative));
                if (zip.PartExists(uri))
                {
                    zip.DeletePart(uri);
                }
                PackagePart part = zip.CreatePart(uri, "", CompressionOption.Normal);
                using (FileStream fileStream = new FileStream(fileToAdd, FileMode.Open, FileAccess.Read))
                {
                    using (Stream dest = part.GetStream())
                    {
                        CopyStream(fileStream, dest);
                    }
                }
            }
        }

        private static void CopyStream(System.IO.FileStream inputStream, System.IO.Stream outputStream)
        {
            long bufferSize = inputStream.Length < BUFFER_SIZE ? inputStream.Length : BUFFER_SIZE;
            byte[] buffer = new byte[bufferSize];
            int bytesRead = 0;
            long bytesWritten = 0;
            while ((bytesRead = inputStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                outputStream.Write(buffer, 0, bytesRead);
                bytesWritten += bufferSize;
            }
        }
    }
}
