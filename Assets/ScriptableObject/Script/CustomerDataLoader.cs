// #if UNITY_EDITOR
// using UnityEditor;
// using UnityEngine;
// using Newtonsoft.Json;
// using System.IO;
//
// public class CustomerDataJsonImporter
// {
//     [MenuItem("Bakery/Import Customer JSON")]
//     public static void ImportCustomerJson()
//     {
//         string jsonPath = "Assets/Resources/JSON/customerData.json";
//         string[] assetPaths = Directory.GetFiles("Assets/Resources/CustomerData/", "*.asset");
//
//         string jsonText = File.ReadAllText(jsonPath);
//         CustomerJsonData[] jsonDatas = JsonConvert.DeserializeObject<CustomerJsonData[]>(jsonText);
//
//         foreach (var json in jsonDatas)
//         {
//             CustomerData data = ScriptableObject.CreateInstance<CustomerData>();
//             data.customerName = json.customerName;
//             data.desiredBreadId = json.desiredBreadId;
//             data.quantity = json.quantity;
//             data.wantsToEatIn = json.wantsToEatIn;
//
//             // 에셋 이름: customerName + ID
//             string assetPath = $"Assets/Resources/CustomerData/{json.customerName}_{json.desiredBreadId}.asset";
//             AssetDatabase.CreateAsset(data, assetPath);
//         }
//
//         AssetDatabase.SaveAssets();
//         AssetDatabase.Refresh();
//         Debug.Log("CustomerData SO 생성 완료");
//     }
// }
// #endif