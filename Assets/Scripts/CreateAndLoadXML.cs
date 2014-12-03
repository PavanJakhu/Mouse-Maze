using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Security.Cryptography;
using System.IO;
using System.Text;

public class CreateAndLoadXML : MonoBehaviour
{
    public GameObject savedMaze;

    private GameManager gameManager;
    private string _FileLocation, _FileName, _data;
    private MazeData mazeData;

    public struct MazeData
    {
        public Position playerPosition;
        public Position endPosition;
        public Position trapPosition;
        public Position cupPosition;
        public Objects cheesePosition;
        public Objects bombPosition;

        public struct Position
        {
            public float x, y, z;
        }

        public struct Objects
        {
            public Position one;
            public Position two;
            public Position three;
            public Position four;
            public Position five;
        }
    }

    // Use this for initialization
    void Start()
    {
        gameManager = GetComponent<GameManager>();

        _FileLocation = Application.dataPath;
        _FileName = "Game Data/SaveData.xml";

        mazeData = new MazeData();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            PrefabUtility.CreatePrefab("Assets/Prefabs/Saved/Saved_Maze.prefab", gameManager.MazeInstance.gameObject);

            mazeData.playerPosition.x = gameManager.PlayerInstance.transform.position.x;
            mazeData.playerPosition.y = gameManager.PlayerInstance.transform.position.y;
            mazeData.playerPosition.z = gameManager.PlayerInstance.transform.position.z;

            mazeData.endPosition.x = gameManager.EndInstance.transform.position.x;
            mazeData.endPosition.y = gameManager.EndInstance.transform.position.y;
            mazeData.endPosition.z = gameManager.EndInstance.transform.position.z;

            if (gameManager.TrapsInstance != null)
            {
                mazeData.trapPosition.x = gameManager.TrapsInstance.transform.position.x;
                mazeData.trapPosition.y = gameManager.TrapsInstance.transform.position.y;
                mazeData.trapPosition.z = gameManager.TrapsInstance.transform.position.z;
            }

            if (gameManager.CupInstance != null)
            {
                mazeData.cupPosition.x = gameManager.CupInstance.transform.position.x;
                mazeData.cupPosition.y = gameManager.CupInstance.transform.position.y;
                mazeData.cupPosition.z = gameManager.CupInstance.transform.position.z;
            }

            if (gameManager.CheeseInstance[0] != null)
            {
                mazeData.cheesePosition.one.x = gameManager.CheeseInstance[0].transform.position.x;
                mazeData.cheesePosition.one.y = gameManager.CheeseInstance[0].transform.position.y;
                mazeData.cheesePosition.one.z = gameManager.CheeseInstance[0].transform.position.z;
            }

            if (gameManager.CheeseInstance[1] != null)
            {
                mazeData.cheesePosition.two.x = gameManager.CheeseInstance[1].transform.position.x;
                mazeData.cheesePosition.two.y = gameManager.CheeseInstance[1].transform.position.y;
                mazeData.cheesePosition.two.z = gameManager.CheeseInstance[1].transform.position.z;
            }

            if (gameManager.CheeseInstance[2] != null)
            {
                mazeData.cheesePosition.three.x = gameManager.CheeseInstance[2].transform.position.x;
                mazeData.cheesePosition.three.y = gameManager.CheeseInstance[2].transform.position.y;
                mazeData.cheesePosition.three.z = gameManager.CheeseInstance[2].transform.position.z;
            }

            if (gameManager.CheeseInstance[3] != null)
            {
                mazeData.cheesePosition.four.x = gameManager.CheeseInstance[3].transform.position.x;
                mazeData.cheesePosition.four.y = gameManager.CheeseInstance[3].transform.position.y;
                mazeData.cheesePosition.four.z = gameManager.CheeseInstance[3].transform.position.z;
            }

            if (gameManager.CheeseInstance[4] != null)
            {
                mazeData.cheesePosition.five.x = gameManager.CheeseInstance[4].transform.position.x;
                mazeData.cheesePosition.five.y = gameManager.CheeseInstance[4].transform.position.y;
                mazeData.cheesePosition.five.z = gameManager.CheeseInstance[4].transform.position.z;
            }

            if (gameManager.BombInstance[0] != null)
            {
                mazeData.bombPosition.one.x = gameManager.BombInstance[0].transform.position.x;
                mazeData.bombPosition.one.y = gameManager.BombInstance[0].transform.position.y;
                mazeData.bombPosition.one.z = gameManager.BombInstance[0].transform.position.z;
            }

            if (gameManager.BombInstance[1] != null)
            {
                mazeData.bombPosition.two.x = gameManager.BombInstance[1].transform.position.x;
                mazeData.bombPosition.two.y = gameManager.BombInstance[1].transform.position.y;
                mazeData.bombPosition.two.z = gameManager.BombInstance[1].transform.position.z;
            }

            if (gameManager.BombInstance[2] != null)
            {
                mazeData.bombPosition.three.x = gameManager.BombInstance[2].transform.position.x;
                mazeData.bombPosition.three.y = gameManager.BombInstance[2].transform.position.y;
                mazeData.bombPosition.three.z = gameManager.BombInstance[2].transform.position.z;
            }

            if (gameManager.BombInstance[3] != null)
            {
                mazeData.bombPosition.four.x = gameManager.BombInstance[3].transform.position.x;
                mazeData.bombPosition.four.y = gameManager.BombInstance[3].transform.position.y;
                mazeData.bombPosition.four.z = gameManager.BombInstance[3].transform.position.z;
            }

            // Create XML based on the UserData Object! 
            _data = SerializeObject(mazeData);

            // Write to file, resulting XML from  serialization process 
            CreateXML();
        }
    }

    public void OnResumeClick()
    {
        if (GameManager.gameState == GameState.Menu)
        {
            // Load UserData into myData
            LoadXML();

            // Check if data has been loaded
            if (_data.ToString() != "")
            {
                // Cast to type (UserData) so returned object is converted to correct type 
                mazeData = (MazeData)DeserializeObject(_data);

                gameManager.MazeInstance = Instantiate(savedMaze) as Maze;
                gameManager.PlayerInstance = Instantiate(gameManager.playerPrefab) as Player;
                gameManager.PlayerInstance.transform.position = new Vector3(mazeData.playerPosition.x, mazeData.playerPosition.y, mazeData.playerPosition.z);
                gameManager.EndInstance = Instantiate(gameManager.endPrefab) as End;
                gameManager.EndInstance.transform.position = new Vector3(mazeData.endPosition.x, mazeData.endPosition.y, mazeData.endPosition.z);
                gameManager.TrapsInstance = Instantiate(gameManager.trapPrefab) as Traps;
                gameManager.TrapsInstance.transform.position = new Vector3(mazeData.trapPosition.x, mazeData.trapPosition.y, mazeData.trapPosition.z);
                gameManager.CupInstance = Instantiate(gameManager.cupPrefab) as Cup;
                gameManager.CupInstance.transform.position = new Vector3(mazeData.cupPosition.x, mazeData.cupPosition.y, mazeData.cupPosition.z);
                gameManager.CheeseInstance[0] = Instantiate(gameManager.cheesePrefab) as Cheese;
                gameManager.CheeseInstance[0].transform.position = new Vector3(mazeData.cheesePosition.one.x, mazeData.cheesePosition.one.y, mazeData.cheesePosition.one.z);
                gameManager.CheeseInstance[1] = Instantiate(gameManager.cheesePrefab) as Cheese;
                gameManager.CheeseInstance[1].transform.position = new Vector3(mazeData.cheesePosition.two.x, mazeData.cheesePosition.two.y, mazeData.cheesePosition.two.z);
                gameManager.CheeseInstance[2] = Instantiate(gameManager.cheesePrefab) as Cheese;
                gameManager.CheeseInstance[2].transform.position = new Vector3(mazeData.cheesePosition.three.x, mazeData.cheesePosition.three.y, mazeData.cheesePosition.three.z);
                gameManager.CheeseInstance[3] = Instantiate(gameManager.cheesePrefab) as Cheese;
                gameManager.CheeseInstance[3].transform.position = new Vector3(mazeData.cheesePosition.four.x, mazeData.cheesePosition.four.y, mazeData.cheesePosition.four.z);
                gameManager.CheeseInstance[4] = Instantiate(gameManager.cheesePrefab) as Cheese;
                gameManager.CheeseInstance[4].transform.position = new Vector3(mazeData.cheesePosition.five.x, mazeData.cheesePosition.five.y, mazeData.cheesePosition.five.z);
                gameManager.BombInstance[0] = Instantiate(gameManager.bombPrefab) as Bombs;
                gameManager.BombInstance[0].transform.position = new Vector3(mazeData.bombPosition.one.x, mazeData.bombPosition.one.y, mazeData.bombPosition.one.z);
                gameManager.BombInstance[1] = Instantiate(gameManager.bombPrefab) as Bombs;
                gameManager.BombInstance[1].transform.position = new Vector3(mazeData.bombPosition.two.x, mazeData.bombPosition.two.y, mazeData.bombPosition.two.z);
                gameManager.BombInstance[2] = Instantiate(gameManager.bombPrefab) as Bombs;
                gameManager.BombInstance[2].transform.position = new Vector3(mazeData.bombPosition.three.x, mazeData.bombPosition.three.y, mazeData.bombPosition.three.z);
                gameManager.BombInstance[3] = Instantiate(gameManager.bombPrefab) as Bombs;
                gameManager.BombInstance[3].transform.position = new Vector3(mazeData.bombPosition.four.x, mazeData.bombPosition.four.y, mazeData.bombPosition.four.z);

                Destroy(GameObject.Find("Menu Canvas"));
                Instantiate(gameManager.inGameCanvasPrefab);
            }
        }
    }

    /* The following metods came from the referenced URL */
    string UTF8ByteArrayToString(byte[] characters)
    {
        UTF8Encoding encoding = new UTF8Encoding();
        string constructedString = encoding.GetString(characters);
        return (constructedString);
    }

    byte[] StringToUTF8ByteArray(string pXmlString)
    {
        UTF8Encoding encoding = new UTF8Encoding();
        byte[] byteArray = encoding.GetBytes(pXmlString);
        return byteArray;
    }

    // Serialize UserData object of myData 
    string SerializeObject(object pObject)
    {
        string XmlizedString = null;
        MemoryStream memoryStream = new MemoryStream();
        XmlSerializer xs = new XmlSerializer(typeof(MazeData));
        XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
        xs.Serialize(xmlTextWriter, pObject);
        memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
        XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
        return XmlizedString;
    }

    // Deserialize it back into its original form 
    object DeserializeObject(string pXmlizedString)
    {
        XmlSerializer xs = new XmlSerializer(typeof(MazeData));
        MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString));
        //XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8); 
        return xs.Deserialize(memoryStream);
    }

    // Save XML Method
    void CreateXML()
    {
        // Create writer so file can be written to
        StreamWriter writer;

        // Load file info
        FileInfo t = new FileInfo(_FileLocation + "\\" + _FileName);

        // Check if file exists
        if (!t.Exists)
        {
            // Creates or opens a file for writing UTF-8 encoded text
            writer = t.CreateText();
        }
        else
        {
            // Delete old file
            t.Delete();
            // Creates or opens a file for writing UTF-8 encoded text.
            writer = t.CreateText();
        }

        // Encrypt data to write to file
        //_data = Encrypt(_data);

        // Write information to file
        writer.Write(_data);
        writer.Close();

        Debug.Log("File written.");
    }

    // Load XML File
    void LoadXML()
    {
        // Create reader so file can be read
        StreamReader r = File.OpenText(_FileLocation + "\\" + _FileName);

        // Read information from opened file
        string _info = r.ReadToEnd();

        // Decrypt after loading
        //_info = Decrypt(_info);

        // Close file
        r.Close();

        // Store information to parse
        _data = _info;

        Debug.Log("File Read");
    }

    public static string Encrypt(string toEncrypt)
    {
        // Encode a set of characters from specified String into the specified byte array
        // 256-AES key, specified with block and key sizes that may be any multiple of 32 bits
        byte[] keyArray = UTF8Encoding.UTF8.GetBytes("12345678901234567890123456789012");

        // What needs to be converted is stored in the byte[]
        byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

        // Create encruption class
        RijndaelManaged rDel = new RijndaelManaged();

        // Assign key generated above
        rDel.Key = keyArray;

        // http://msdn.microsoft.com/en-us/library/system.security.cryptography.ciphermode.aspx
        rDel.Mode = CipherMode.ECB;

        // Better lang support
        // Specifies type of padding to apply when the message data block is shorter than the full number of bytes needed for a cryptographic operation
        // http://msdn.microsoft.com/en-us/library/system.security.cryptography.paddingmode.aspx
        rDel.Padding = PaddingMode.PKCS7;

        // Create encryptor based on key above
        // http://msdn.microsoft.com/en-us/library/system.security.cryptography.rijndaelmanaged.createencryptor.aspx
        ICryptoTransform cTransform = rDel.CreateEncryptor();

        // Transforms the specified region of the specified byte array.
        // http://msdn.microsoft.com/en-us/library/system.security.cryptography.icryptotransform.transformfinalblock.aspx
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

        // Return created string
        return Convert.ToBase64String(resultArray, 0, resultArray.Length);
    }

    public static string Decrypt(string toDecrypt)
    {
        // Encode a set of characters from specified String into the specified byte array
        // 256-AES key, specified with block and key sizes that may be any multiple of 32 bits
        byte[] keyArray = UTF8Encoding.UTF8.GetBytes("12345678901234567890123456789012");

        // What needs to be converted is stored in the byte[]
        byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

        // Create encruption class
        RijndaelManaged rDel = new RijndaelManaged();

        // Assign key generated above
        rDel.Key = keyArray;

        // http://msdn.microsoft.com/en-us/library/system.security.cryptography.ciphermode.aspx
        rDel.Mode = CipherMode.ECB;

        // Better lang support
        // Specifies type of padding to apply when message data block is shorter than the full number of bytes needed for a cryptographic operation
        rDel.Padding = PaddingMode.PKCS7;

        // Create decryptor based on key above
        ICryptoTransform cTransform = rDel.CreateDecryptor();

        // Transforms the specified region of the specified byte array.
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

        // Return created string
        return UTF8Encoding.UTF8.GetString(resultArray);
    }
}
