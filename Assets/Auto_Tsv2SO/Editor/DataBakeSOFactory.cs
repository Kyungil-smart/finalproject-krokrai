using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace SOBaker
{
    public static class DataBakeSOFactory
    {
        public static void SyncAndBakeData<TMainSO>(TextAsset tsv, string path, int startNum, int endNum = int.MaxValue)
            where TMainSO : ScriptableObject
        {
            startNum--;
            if (startNum <= -1 || endNum < startNum)
            {
                Debug.LogWarning($"[DataBakeSOFactory] 잘 못된 시작과 끝 줄 수를 입력 했습니다. {startNum} / {endNum}");
                return;
            }
            if (tsv == null)
            {
                Debug.LogError("[DataBakeSOFactory] TSV 파일이 Null일 수 없습니다.");
            }

            string childName = typeof(TMainSO).Name;

            if (!Directory.Exists($"{path}/{childName}"))
            {
                Directory.CreateDirectory($"{path}/{childName}");
            }

            string parentAssetPath = $"{path}/{childName}_Root.asset";
            string[] chiledAssetPath =  new string[] { $"{path}/{childName}/" };

            

            AssetDatabase.StartAssetEditing();
            bool _isEditing = true;

            AutoSOGen_ContaineSO containeSO = AssetDatabase.LoadAssetAtPath<AutoSOGen_ContaineSO>(parentAssetPath);

            if (containeSO == null)
            {
                containeSO = ScriptableObject.CreateInstance<AutoSOGen_ContaineSO>(); // 제너릭은 사용 불가
                AssetDatabase.CreateAsset(containeSO, $"{parentAssetPath}");
            }

            string[] tsv_rows = tsv.ToString().Split('\n',StringSplitOptions.RemoveEmptyEntries);

            if (endNum == int.MaxValue)
            {
                endNum = tsv_rows.Length - startNum;
            }
            else if (endNum > tsv_rows.Length)
            {
                endNum = tsv_rows.Length;
                Debug.Log($"[DataBakeSOFactory] 마지막 줄이 tsv의 행 수보다 많을 수 없습니다. tsv의 행수로 고정합니다,");
            }
            else
            {
                endNum -= startNum;
            }

                Debug.Log($"{startNum} / {endNum}");

            containeSO.scriptableObjects = new TMainSO[endNum];
            EditorUtility.SetDirty(containeSO);

            bool isCancelled;

            string[] guids = AssetDatabase.FindAssets($"{childName}_Row_ t:{typeof(TMainSO).Name}",chiledAssetPath);

            List<(TMainSO,string)> mainSoList = new(guids.Length);

            foreach (var guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);

                string fileName = System.IO.Path.GetFileNameWithoutExtension(assetPath);
                if (!fileName.Contains($"{childName}_Row_")) continue;

                TMainSO asset = AssetDatabase.LoadAssetAtPath<TMainSO>(assetPath); ;

                if (asset != null)
                {
                    mainSoList.Add((asset,assetPath));
                }
            }

            try
            {
                TMainSO targetChild;
                
                for (int k = 0; k < endNum; k++)
                {
                    float progress = (float)k / endNum;
                    string infoMsg = $"SO 굽는 중... [{k} / {endNum}]";
                    string[] splitedTsv = tsv_rows[k+startNum].Split('\t');

                    isCancelled = EditorUtility.DisplayCancelableProgressBar(
                        "Data Baker",
                        infoMsg,
                        progress
                        );

                    if (k < mainSoList.Count)
                    {
                        targetChild = mainSoList[k].Item1;
                    }
                    else
                    {
                        targetChild = ScriptableObject.CreateInstance<TMainSO>();
                        targetChild.name = $"{childName}_Row_{k}";

                        AssetDatabase.CreateAsset(targetChild, $"{path}/{childName}/{targetChild.name}.asset");
                    }
                    
                    SODataInjector<TMainSO>.InjectData(targetChild, splitedTsv);

                    EditorUtility.SetDirty(targetChild);

                    containeSO.scriptableObjects[k] = targetChild;
                }

                _isEditing = false;
                AssetDatabase.StopAssetEditing();
                if (endNum < mainSoList.Count)
                {
                    for (int i = endNum; i < mainSoList.Count; i++)
                    {
                        AssetDatabase.DeleteAsset(mainSoList[i].Item2);
                        Debug.Log($"삭제됌 / {mainSoList[i].Item2}");
                    }
                }

                EditorUtility.DisplayDialog("완료", "SO를 성공적으로 구웠습니다.", "확인");
            }
            catch (System.NullReferenceException nullEx)
            {
                Debug.LogException(nullEx);
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
            }
            finally
            {
                EditorUtility.ClearProgressBar();

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                mainSoList = null;
                if(_isEditing)
                    AssetDatabase.StopAssetEditing();
            }
        }
    }
}
