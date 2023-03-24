using System.Linq;

public partial class TicTacToe
{
    private void CalculateConnectedCount()
    {
        for (int i = 0; i < _gridList.Count; i++)
        {
            var grid = _gridList[i];
            int connectedCount = 0;

            if (grid.GridData.GridType == GridType.Empty)
            {
                int x = grid.GridData.Coordinate.x;
                int y = grid.GridData.Coordinate.y;

                connectedCount = 2;

                if (x == y || x + y == boardSize - 1)
                {
                    connectedCount = 3; // 对角线 or 逆对角线 最大连通数：3
                }
                else if (x == y && x + y == boardSize - 1)
                {
                    connectedCount = 4; // 中心最大连通数：4
                }

                // 对角线是否连通？
                if (x == y)
                {
                    for (int l = 0; l < boardSize; l++)
                    {
                        if (!(_gridList[l * (boardSize + 1)].GridData.GridType == GridType.Empty ||
                              _gridList[l * (boardSize + 1)].GridData.PawnType == grid.GridData.PawnType))
                        {
                            connectedCount--;
                            break;
                        }
                    }
                }

                // 逆对角线是否连通？
                if (x + y == boardSize - 1)
                {
                    for (int n = 0; n < boardSize; n++)
                    {
                        if (!(_gridList[(n + 1) * (boardSize - 1)].GridData.GridType == GridType.Empty ||
                              _gridList[(n + 1) * (boardSize - 1)].GridData.PawnType == grid.GridData.PawnType))
                        {
                            connectedCount--;
                            break;
                        }
                    }
                }

                // 列是否连通？
                for (int j = 0; j < boardSize; j++)
                {
                    if (!(_gridList[boardSize * j + x].GridData.GridType == GridType.Empty ||
                          _gridList[boardSize * j + x].GridData.PawnType == grid.GridData.PawnType))
                    {
                        connectedCount--;
                        break;
                    }
                }

                // 行是否连通？
                for (int k = 0; k < boardSize; k++)
                {
                    if (!(_gridList[boardSize * y + k].GridData.GridType == GridType.Empty ||
                          _gridList[boardSize * y + k].GridData.PawnType == grid.GridData.PawnType))
                    {
                        connectedCount--;
                        break;
                    }
                }
            }

            grid.GridData.ConnectedCount = connectedCount;
        }
    }

    private void CalculateMaxSameTypeCount()
    {
        for (int i = 0; i < _gridList.Count; i++)
        {
            var grid = _gridList[i];

            int maxSameTypeCountInPipeline = 0;
            int maxDiffTypeCountInPipeline = 0;

            int maxSameTypeCount = 0;
            int maxDiffTypeCount = 0;

            if (grid.GridData.GridType == GridType.Empty)
            {
                int x = grid.GridData.Coordinate.x;
                int y = grid.GridData.Coordinate.y;

                // 对角线
                if (x == y)
                {
                    for (int j = 0; j < boardSize; j++)
                    {
                        if (_gridList[j * (boardSize + 1)].GridData.PawnType == PawnType.ComputePawn)
                        {
                            maxSameTypeCount++;
                            if (maxSameTypeCountInPipeline < maxSameTypeCount)
                            {
                                maxSameTypeCountInPipeline = maxSameTypeCount;
                            }
                        }

                        if (_gridList[j * (boardSize + 1)].GridData.PawnType == PawnType.PlayerPawn)
                        {
                            maxDiffTypeCount++;
                            if (maxDiffTypeCountInPipeline < maxDiffTypeCount)
                            {
                                maxDiffTypeCountInPipeline = maxDiffTypeCount;
                            }
                        }
                    }
                }

                maxSameTypeCount = 0;
                maxDiffTypeCount = 0;

                // 逆对角线
                if (x + y == boardSize - 1)
                {
                    for (int k = 0; k < boardSize; k++)
                    {
                        if (_gridList[(k + 1) * (boardSize - 1)].GridData.PawnType == PawnType.ComputePawn)
                        {
                            maxSameTypeCount++;
                            if (maxSameTypeCountInPipeline < maxSameTypeCount)
                            {
                                maxSameTypeCountInPipeline = maxSameTypeCount;
                            }
                        }

                        if (_gridList[(k + 1) * (boardSize - 1)].GridData.PawnType == PawnType.PlayerPawn)
                        {
                            maxDiffTypeCount++;
                            if (maxDiffTypeCountInPipeline < maxDiffTypeCount)
                            {
                                maxDiffTypeCountInPipeline = maxDiffTypeCount;
                            }
                        }
                    }
                }

                maxSameTypeCount = 0;
                maxDiffTypeCount = 0;

                // 列
                for (int l = 0; l < boardSize; l++)
                {
                    if (_gridList[boardSize * l + x].GridData.PawnType == PawnType.ComputePawn)
                    {
                        maxSameTypeCount++;
                        if (maxSameTypeCountInPipeline < maxSameTypeCount)
                        {
                            maxSameTypeCountInPipeline = maxSameTypeCount;
                        }
                    }

                    if (_gridList[boardSize * l + x].GridData.PawnType == PawnType.PlayerPawn)
                    {
                        maxDiffTypeCount++;
                        if (maxDiffTypeCountInPipeline < maxDiffTypeCount)
                        {
                            maxDiffTypeCountInPipeline = maxDiffTypeCount;
                        }
                    }
                }

                maxSameTypeCount = 0;
                maxDiffTypeCount = 0;

                // 行
                for (int m = 0; m < boardSize; m++)
                {
                    if (_gridList[boardSize * y + m].GridData.PawnType == PawnType.ComputePawn)
                    {
                        maxSameTypeCount++;
                        if (maxSameTypeCountInPipeline < maxSameTypeCount)
                        {
                            maxSameTypeCountInPipeline = maxSameTypeCount;
                        }
                    }

                    if (_gridList[boardSize * y + m].GridData.PawnType == PawnType.PlayerPawn)
                    {
                        maxDiffTypeCount++;
                        if (maxDiffTypeCountInPipeline < maxDiffTypeCount)
                        {
                            maxDiffTypeCountInPipeline = maxDiffTypeCount;
                        }
                    }
                }
            }

            grid.GridData.MaxSameTypeCountInPipeline = maxSameTypeCountInPipeline;
            grid.GridData.MaxDiffTypeCountInPipeline = maxDiffTypeCountInPipeline;
        }
    }

    private void CalculateWeights()
    {
        CalculateConnectedCount();
        CalculateMaxSameTypeCount();

        foreach (var grid in _gridList)
        {
            int maxSameTypeFactory = boardSize - 1;
            int maxDiffTypeFactory = boardSize;

            // 优先求胜
            if (grid.GridData.MaxSameTypeCountInPipeline == boardSize - 1)
            {
                maxSameTypeFactory = boardSize * boardSize;
            }
            // 再求不败
            else if (grid.GridData.MaxDiffTypeCountInPipeline == boardSize - 1)
            {
                maxDiffTypeFactory = boardSize * boardSize;
            }

            grid.GridData.Weight =
                grid.GridData.ConnectedCount +
                grid.GridData.MaxSameTypeCountInPipeline * maxSameTypeFactory +
                grid.GridData.MaxDiffTypeCountInPipeline * maxDiffTypeFactory;

            grid.debugTextConnectedCount.text =
                $"{grid.GridData.ConnectedCount}/{grid.GridData.MaxSameTypeCountInPipeline}/{grid.GridData.MaxDiffTypeCountInPipeline}->{grid.GridData.Weight}";
        }
    }

    private Grid GetHighestWeightsGrid()
    {
        return _gridList.OrderByDescending(x => x.GridData.Weight).FirstOrDefault();
    }
}