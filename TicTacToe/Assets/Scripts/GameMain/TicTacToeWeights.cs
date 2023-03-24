public partial class TicTacToe
{
    private void CalculateConnectedCount()
    {
        foreach (Grid grid in _grids)
        {
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
                    for (int i = 0; i < boardSize; i++)
                    {
                        if (!(_grids[i, i].GridData.GridType == GridType.Empty ||
                              _grids[i, i].GridData.PawnType == grid.GridData.PawnType))
                        {
                            connectedCount--;
                            break;
                        }
                    }
                }

                // 逆对角线是否连通？
                if (x + y == boardSize - 1)
                {
                    for (int j = 0; j < boardSize; j++)
                    {
                        if (!(_grids[j, boardSize - j - 1].GridData.GridType == GridType.Empty ||
                              _grids[j, boardSize - j - 1].GridData.PawnType == grid.GridData.PawnType))
                        {
                            connectedCount--;
                            break;
                        }
                    }
                }

                // 列是否连通？
                for (int k = 0; k < boardSize; k++)
                {
                    if (!(_grids[x, k].GridData.GridType == GridType.Empty ||
                          _grids[x, k].GridData.PawnType == grid.GridData.PawnType))
                    {
                        connectedCount--;
                        break;
                    }
                }

                // 行是否连通？
                for (int l = 0; l < boardSize; l++)
                {
                    if (!(_grids[l, y].GridData.GridType == GridType.Empty ||
                          _grids[l, y].GridData.PawnType == grid.GridData.PawnType))
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
        foreach (Grid grid in _grids)
        {
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
                    for (int i = 0; i < boardSize; i++)
                    {
                        if (_grids[i, i].GridData.PawnType == PawnType.ComputePawn)
                        {
                            maxSameTypeCount++;
                            if (maxSameTypeCountInPipeline < maxSameTypeCount)
                            {
                                maxSameTypeCountInPipeline = maxSameTypeCount;
                            }
                        }

                        if (_grids[i, i].GridData.PawnType == PawnType.PlayerPawn)
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
                    for (int j = 0; j < boardSize; j++)
                    {
                        if (_grids[j, boardSize - j - 1].GridData.PawnType == PawnType.ComputePawn)
                        {
                            maxSameTypeCount++;
                            if (maxSameTypeCountInPipeline < maxSameTypeCount)
                            {
                                maxSameTypeCountInPipeline = maxSameTypeCount;
                            }
                        }

                        if (_grids[j, boardSize - j - 1].GridData.PawnType == PawnType.PlayerPawn)
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
                for (int k = 0; k < boardSize; k++)
                {
                    if (_grids[x, k].GridData.PawnType == PawnType.ComputePawn)
                    {
                        maxSameTypeCount++;
                        if (maxSameTypeCountInPipeline < maxSameTypeCount)
                        {
                            maxSameTypeCountInPipeline = maxSameTypeCount;
                        }
                    }

                    if (_grids[x, k].GridData.PawnType == PawnType.PlayerPawn)
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
                for (int l = 0; l < boardSize; l++)
                {
                    if (_grids[l, y].GridData.PawnType == PawnType.ComputePawn)
                    {
                        maxSameTypeCount++;
                        if (maxSameTypeCountInPipeline < maxSameTypeCount)
                        {
                            maxSameTypeCountInPipeline = maxSameTypeCount;
                        }
                    }

                    if (_grids[l, y].GridData.PawnType == PawnType.PlayerPawn)
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

        foreach (var grid in _grids)
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
        int maxVal = int.MinValue;
        Grid maxWeightGrid = null;

        for (int i = 0; i < _grids.GetLength(0); i++)
        {
            for (int j = 0; j < _grids.GetLength(1); j++)
            {
                if (_grids[i, j].GridData.Weight > maxVal)
                {
                    maxVal = _grids[i, j].GridData.Weight;
                    maxWeightGrid = _grids[i, j];
                }
            }
        }

        return maxWeightGrid;
    }
}