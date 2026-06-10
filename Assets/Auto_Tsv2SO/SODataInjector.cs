using System;
using System.Reflection;
using UnityEngine;

public static class SODataInjector<T> where T : ScriptableObject
{
    public static void InjectData(T targetObject, string[] rawTsvRowData)
    {
        if (targetObject == null || rawTsvRowData == null) return;
        Type type = targetObject.GetType();

        FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

        foreach (FieldInfo fieldInfo in fieldInfos)
        {
            SOInjectAttribute injectattriubte = fieldInfo.GetCustomAttribute<SOInjectAttribute>();

            if (injectattriubte == null) continue;
            int targetIndex = injectattriubte.Index;

            if (targetIndex < 0 || targetIndex >= rawTsvRowData.Length)
            {
                Debug.Log($"[SODataInjector] 주입 실패 : {fieldInfo.Name}가 유효하지 않은 값을 갖고 있습니다. 값 : [{targetIndex}]");
                continue;
            }

            string rawValue = rawTsvRowData[targetIndex];

            if (rawValue == "")
            {
                rawValue = "0";
            }

            try
            {
                object convertedValue;
                Type fieldType = fieldInfo.FieldType;
                if (fieldType.IsEnum)
                {
                    if (!Enum.TryParse(fieldType, rawValue, out convertedValue))
                        Debug.Log($"[SODataInjector] {rawValue} Enum을 변환 할 수 없습니다.");
                }
                else
                {
                    convertedValue = Convert.ChangeType(rawValue, fieldType);
                }
                fieldInfo.SetValue(targetObject, convertedValue);
            }
            catch (NullReferenceException e)
            {
                Debug.LogException(e);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}
