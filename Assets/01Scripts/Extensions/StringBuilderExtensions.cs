/*
작성자 : krokrai
작성일 : 26-05-20
수정일 : 26-05-20

역할 : String Builder에서 미지원 하는 Vector3와 Vector2 지원용
방식 : String Builder에 기본으로 지원하는 float을 이용해 vector3와 vector2를 출력
 */
using System.Text;
using UnityEngine;

public static class StringBuilderExtensions
{
    /// <summary>
    /// 미지원하는 Vector2를 float으로 변환 후 적용
    /// </summary>
    /// <param name="sb"></param>
    /// <param name="vec"></param>
    /// <returns></returns>
    public static StringBuilder AppendVector2(this StringBuilder sb,Vector2 vec)
    {
        sb.Append("(")
            .Append(vec.x)
            .Append(", ")
            .Append(vec.y)
            .Append(")");

        return sb;
    }

    /// <summary>
    /// 미지원하는 Vector3를 float으로 변환 후 적용
    /// </summary>
    /// <param name="sb"></param>
    /// <param name="vec"></param>
    /// <returns></returns>
    public static StringBuilder AppendVector3(this StringBuilder sb,Vector3 vec)
    {
        sb.Append("(")
            .Append(vec.x)
            .Append(", ")
            .Append(vec.y)
            .Append(", ")
            .Append(vec.z)
            .Append(")");

        return sb;
    }
}
