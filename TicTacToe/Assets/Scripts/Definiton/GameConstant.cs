using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConstant
{
    public static readonly Color PlayerThemeColor = new Color(0.78f, 1, 0.42f, 1);
    public static readonly Color ComputeThemeColor = new Color(1, 0.23f, 0.23f, 1);
    public static readonly Color GridColor = new Color(0.2f, 0, 0.5f, 1);
    public static readonly Color GridTiesColor = new Color(0.93f, 0.73f, 0, 1);
    public static readonly Color GridHighlightColor = new Color(0.4f, 0.8f, 0.495f, 1);

    public const float GridInitShowInterval = 0.03f;
    public const float GridInitShowDuration = 0.1f;
    public const float GridCompletedHighlightDuration = 0.2f;

    public const float RestartButtonShowDelay = 5f;

    public const float GridCellSize3X3 = 280f;
    public const float GridCellSize4X4 = 210f;

    public const float GridCellSpacing3X3 = 36f;
    public const float GridCellSpacing4X4 = 27f;
}
