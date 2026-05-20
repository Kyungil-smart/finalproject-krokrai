/*
작성자 : krokrai
작성일 : 26-05-20
수정일 : 26-05-20

역할 : Debug.Log 대체 및 시각적으로 class 명시
방식 : String Builder과 Attribute를 활용해 호출한 class 및 class의 Method 명시
 */

using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

public static class Log
{
    private static readonly StringBuilder _stringBuilder = new StringBuilder(512);

    /// <summary>
    /// [Class : Method] message 형태로 출력
    /// </summary>
    /// <param name="message"></param>
    /// <param name="sourceFilePath"></param>
    /// <param name="memberName"></param>
    public static void Message(string message,
        [CallerFilePath] string sourceFilePath = "",
        [CallerMemberName] string memberName = "")
    {
        BaseSet(Path.GetFileNameWithoutExtension(sourceFilePath),memberName)
            .Append(message);

        Debug.Log(_stringBuilder.ToString());
    }

    /// <summary>
    /// 색 적용 범위 [Class : Method] / 색 미적용 범위 : message
    /// </summary>
    /// <param name="message"></param>
    /// <param name="color"></param>
    /// <param name="sourceFilePath"></param>
    /// <param name="memberName"></param>
    public static void MessageColor(string message, Color color,
        [CallerFilePath] string sourceFilePath = "",
        [CallerMemberName] string memberName = "")
    {
        _stringBuilder.Clear();

        string hexColor = ColorUtility.ToHtmlStringRGBA(color);

        _stringBuilder.Append("<color=#").Append(hexColor).Append(">")
                              .Append("[")
                              .Append(Path.GetFileNameWithoutExtension(sourceFilePath))
                              .Append(".")
                              .Append(memberName)
                              .Append("] ")
                              .Append("</color>")
                              .Append(message);

        Debug.Log(_stringBuilder.ToString());
    }

    /// <summary>
    /// 단순 int 출력 용
    /// </summary>
    /// <param name="message"></param>
    /// <param name="sourceFilePath"></param>
    /// <param name="memberName"></param>
    public static void Message(int message,
        [CallerFilePath] string sourceFilePath = "",
        [CallerMemberName] string memberName = "")
    {
        BaseSet(Path.GetFileNameWithoutExtension(sourceFilePath), memberName)
            .Append(message);

        Debug.Log(_stringBuilder.ToString());
    }

    /// <summary>
    /// 단순 float 출력 용
    /// </summary>
    /// <param name="message"></param>
    /// <param name="sourceFilePath"></param>
    /// <param name="memberName"></param>
    public static void Message(float message,
        [CallerFilePath] string sourceFilePath = "",
        [CallerMemberName] string memberName = "")
    {
        BaseSet(Path.GetFileNameWithoutExtension(sourceFilePath), memberName)
            .Append(message);

        Debug.Log(_stringBuilder.ToString());
    }

    /// <summary>
    /// 단순 bool 출력 용
    /// </summary>
    /// <param name="message"></param>
    /// <param name="sourceFilePath"></param>
    /// <param name="memberName"></param>
    public static void Message(bool message,
        [CallerFilePath] string sourceFilePath = "",
        [CallerMemberName] string memberName = "")
    {
        BaseSet(Path.GetFileNameWithoutExtension(sourceFilePath), memberName)
            .Append(message);

        Debug.Log(_stringBuilder.ToString());
    }

    /// <summary>
    /// 단순 vector2 출력 용
    /// </summary>
    /// <param name="message"></param>
    /// <param name="sourceFilePath"></param>
    /// <param name="memberName"></param>
    public static void Message(Vector2 message,
        [CallerFilePath] string sourceFilePath = "",
        [CallerMemberName] string memberName = "")
    {
        BaseSet(Path.GetFileNameWithoutExtension(sourceFilePath), memberName)
            .AppendVector2(message);

        Debug.Log(_stringBuilder.ToString());
    }
    
    /// <summary>
    /// 단순 vector3 출력 용
    /// </summary>
    /// <param name="message"></param>
    /// <param name="sourceFilePath"></param>
    /// <param name="memberName"></param>
    public static void Message(Vector3 message,
        [CallerFilePath] string sourceFilePath = "",
        [CallerMemberName] string memberName = "")
    {
        BaseSet(Path.GetFileNameWithoutExtension(sourceFilePath), memberName)
            .AppendVector3(message);

        Debug.Log(_stringBuilder.ToString());
    }

    private static StringBuilder BaseSet(string scriptName, string fName)
    {
        _stringBuilder.Clear();

        _stringBuilder.Append("[")
                              .Append(scriptName)
                              .Append(".")
                              .Append(fName)
                              .Append("] ");
        return _stringBuilder;
    }
}
