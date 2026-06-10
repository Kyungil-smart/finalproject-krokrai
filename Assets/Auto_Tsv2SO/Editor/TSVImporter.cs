/*
 작성자 : krokrai
 작성일 : 26-05-21
 수정일 : 26-05-21

 역할 : tsv file -> TextAsset 변환, 유니티는 tsv를 인식 못하기 때문에 추가 작성이 필요했음
 방식 : Editor 전용 namespace 사용 및 Editor에 인식 시키기 위해 Editor 파일 내부에 선언 되어 있으며, ScriptedImporter를 통해 file 인식 과정 중에 가로채어 사용자 정의로 인식 시킴(현재는 TextAsset)
 */
using System.IO;
using UnityEditor.AssetImporters;
using UnityEngine;

[ScriptedImporter(1,"tsv")]
public class TSVImporter : ScriptedImporter
{
    public override void OnImportAsset(AssetImportContext ctx)
    {

        string fileContent = File.ReadAllText(ctx.assetPath);

        TextAsset textAsset = new TextAsset(fileContent);

        ctx.AddObjectToAsset("main", textAsset);
        ctx.SetMainObject(textAsset);
    }
}
