/*
 작성자 : krokrai
 작성일 : 26-05-21
 작성일 : 26-05-21

 역할 : 행마다 생성된 SO들을 통합해서 데이터를 주입 하는 역할
 방식 : SO 형태로 받아와서 ISetSOData interface로 한 행씩 자른 문자열들을 SO 한 개씩 넘겨줌
 */
using UnityEngine;

[CreateAssetMenu(fileName = "DataBaker", menuName = "Scriptable Objects/DataBaker")]
public class DataBaker : ScriptableObject
{
    [SerializeField] private TextAsset _tsvFile;
    [SerializeField] private int _startCol;
    [SerializeField] private ScriptableObject[] _sOs;

    [ContextMenu("tsv Bake")]
    private void InjectData()
    {
        int startCol = _startCol - 1;
        string[] rowData = _tsvFile.text.Split(new[] { "\r\n", "\r", "\n"}, System.StringSplitOptions.RemoveEmptyEntries);
        
        if (_sOs.Length < rowData.Length - startCol)
        {
            Log.Message($"SO가 부족합니다. SO를 추가해주세요. 현재 SO 수 : {_sOs.Length} / 입력된 줄 수 : {rowData.Length - startCol}");
            return;
        }

        string[] data;
        for (int i = 0; i < rowData.Length - startCol; i++)
        {
            if (!(_sOs[i] is ISetSOData))
            {
                Log.Message("ISetSOData를 상속 받지 않은 SO가 존재합니다.");
                return;
            }

            data = rowData[i + startCol].Split('\t');
            data[data.Length - 1] = data[data.Length - 1].Trim();

            (_sOs[i] as ISetSOData).SetData(data);
        }
    }
}
