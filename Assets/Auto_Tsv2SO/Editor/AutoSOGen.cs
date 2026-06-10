using Codice.CM.SEIDInfo;
using SOBaker;
using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class AutoSOGen : EditorWindow
{
    [SerializeField] private VisualTreeAsset _vsTreeAsset = default;

    private TextAsset _tsv;
    EditorGUILayout.ScrollViewScope _usingScroll;
    GUIStyle _style;

    private TextField _pathTextField;
    private Button _pathSelectButton;

    private int _tsvStartNum;
    private int _tsvEndNum;

    private Vector2 _scrollview;

    [MenuItem("Window/TSV2SO")]
    public static void ShowWindow()
    {
        GetWindow<AutoSOGen>("TSV2SO");
    }

    private void CreateGUI()
    {
        if (_vsTreeAsset != null)
        {
            VisualElement root = _vsTreeAsset.Instantiate();
            rootVisualElement.Add(root);
        }

        ObjectField tsv = rootVisualElement.Q<ObjectField>("tsv");
        ObjectField so = rootVisualElement.Q<ObjectField>("TargetSO");
        Button bakeButton = rootVisualElement.Q<Button>("bake_Button");

        TextField startNum = rootVisualElement.Q<TextField>("startNum");
        TextField endNum = rootVisualElement.Q<TextField>("endNum");

        _pathTextField = rootVisualElement.Q<TextField>("Path-Text-Field");
        _pathSelectButton = rootVisualElement.Q<Button>("Path-Select_Button");

        if (tsv != null )
        {
            tsv.objectType = typeof(TextAsset);
        }

        if (bakeButton != null)
        {
            bakeButton.clicked += () =>
            {
                if (string.IsNullOrEmpty(_pathTextField.value))
                {
                    EditorUtility.DisplayDialog("오류", "경로가 비어 있을 수 없습니다. 경로를 설정해주세요.","확인");
                    return;
                }
                if (tsv != null && tsv.value != null)
                {
                    if (!int.TryParse(startNum.value, out _tsvStartNum))
                    {
                        Debug.Log($"[DataBakeSOFactory]  잘 못된 형변환 {startNum.value}");
                    }
                    int.TryParse(endNum.value, out _tsvEndNum);
                    ExecuteBake((TextAsset)tsv.value, (MonoScript)so.value);
                }
                else
                {
                    EditorUtility.DisplayDialog("오류", "TSV 파일이 등록 되지 않았습니다.","확인");
                }
            };
        }

        if (_pathSelectButton != null)
        {
            _pathSelectButton.clicked += OnPathSelectButtonCliked;
        }

    }

    private void OnPathSelectButtonCliked()
    {
        string defaultPath = Application.dataPath;
        string selectedSystemPath = EditorUtility.OpenFolderPanel("SO 생성 폴더 지정", defaultPath, "");

        if (string.IsNullOrEmpty(selectedSystemPath)) return;

        if (selectedSystemPath.StartsWith(Application.dataPath))
        {
            string relativePath = "Assets"+selectedSystemPath.Substring(Application.dataPath.Length);

            _pathTextField.value = relativePath;
        }
        else
        {
            EditorUtility.DisplayDialog("경로 오류", "반드시 현재 프로젝트의 Assets 폴더 내부 또는 하위 폴더를 선택해야 에셋 베이킹이 가능합니다.", "확인");
        }
    }

    private void OnDisable()
    {
        if(_pathSelectButton != null)
        {
            _pathSelectButton.clicked -= OnPathSelectButtonCliked;
        }
    }

    private void ExecuteBake(TextAsset tsv, MonoScript so)
    {
        Type t = so.GetClass();

        MethodInfo methodinfo = typeof(DataBakeSOFactory).GetMethod(
            nameof(DataBakeSOFactory.SyncAndBakeData),
            BindingFlags.Public | BindingFlags.Static
            );

        if (methodinfo == null)
        {
            EditorUtility.DisplayDialog("오류!", "현재 DataBakeSoFactory.cs가 감지 되지 않았습니다.", "확인");
            return;
        }

        if (t != null && typeof(ScriptableObject).IsAssignableFrom(t))
        {
            if (_tsvEndNum == 0 || _tsvEndNum == default)
            {
                _tsvEndNum = int.MaxValue;
            }
            MethodInfo genericMethod = methodinfo.MakeGenericMethod(t);

            genericMethod.Invoke(null, new object[] { tsv, _pathTextField.value, _tsvStartNum, _tsvEndNum });
        }
        else
        {
            EditorUtility.DisplayDialog("오류!", "입력된 SO Script가 ScriptableOject 형식이 아닙니다.", "확인");
        }
    }
}
