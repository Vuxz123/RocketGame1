using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace com.ethnicthv
{
    public class MapBehaviour : MonoBehaviour
    {
        public GameObject collectablePrefab;
        public Material material;
        public GamePlay gamePlay;

        private const int MapSizeX = UsefulConstant.MapSizeX;
        private readonly int _mapSize = UsefulConstant.MapSize;
        private Cell[] _map;

        private int _startPoint;

        private readonly int _maxCollectable = UsefulConstant.MaxCollectable - 1;
        private List<(int, int)> _collectablePosition;

        private int _playerPosition;
        
        public List<int> CollectableColors => _collectablePosition.ConvertAll(x => x.Item2);

        void Awake()
        {
            _collectablePosition = new List<(int, int)>();
            Cell[] map = new Cell[_mapSize];
            for (int i = 0; i < _mapSize; i++)
            {
                map[i] = new Cell(true, true, true, true);
            }

            GenerateMap(map);
        }

        private void Start()
        {
            InitCellObject();
            _playerPosition = 0;
        }

        private void InitCellObject()
        {
            var cellCount = _map.Length;
            for (var index = 0; index < cellCount; index++)
            {
                var cell = _map[index];

                var (x, y) = GetPosition(index);

                if (!cell.Render) continue;

                var go = new GameObject($"Cell_{index}")
                {
                    transform =
                    {
                        position = new Vector3(x, 0, -y)
                    }
                };

                var (vertices, triangles, uvs) = Cell.CalculateMesh(cell.Up, cell.Down, cell.Left, cell.Right);

                var mesh = new Mesh
                {
                    vertices = Array.ConvertAll(
                        vertices, vertex => new Vector3(vertex.x, vertex.y, vertex.z)
                    ),
                    uv = Array.ConvertAll(uvs, uv => new Vector2(uv.x, uv.y)),
                    triangles = triangles
                };
                mesh.RecalculateNormals();

                var meshFilter = go.AddComponent<MeshFilter>();
                meshFilter.mesh = mesh;

                var meshRenderer = go.AddComponent<MeshRenderer>();
                meshRenderer.material = material;
            }
        }

        private void GenerateMap(Cell[] map)
        {
            var stack = new Stack<int>();

            var random = new Random();

            _startPoint = random.Next(0, _mapSize);

            var currentCell = _startPoint;

            //Create a list of visited cells
            var visitedCells = new List<int> {
                //Add the current cell to the visited cells
                currentCell };

            //While there are still cells to visit
            while (visitedCells.Count < _mapSize)
            {
                //Get the unvisited neighbors of the current cell
                var unvisitedNeighbors = GetUnvisitedNeighbors(currentCell, visitedCells);
                //If there are unvisited neighbors
                if (unvisitedNeighbors.Count > 0)
                {
                    //Choose a random unvisited neighbor
                    var randomNeighbor = unvisitedNeighbors[random.Next(0, unvisitedNeighbors.Count)];
                    //Push the current cell to the stack
                    stack.Push(currentCell);
                    //Remove the wall between the current cell and the neighbor
                    RemoveWall(currentCell, randomNeighbor, map);
                    //Set the neighbor as the current cell
                    currentCell = randomNeighbor;
                    //Add the current cell to the visited cells
                    visitedCells.Add(currentCell);
                }
                //If there are no unvisited neighbors
                else if (stack.Count > 0)
                {
                    //Pop the stack to get the last cell
                    currentCell = stack.Pop();
                }
            }

            _map = map;
        }

        private static void RemoveWall(int currentCell, int randomNeighbor, Cell[] map)
        {
            var current = map[currentCell];
            var neighbor = map[randomNeighbor];

            //Get the x and y coordinates of the current cell
            var (x, y) = GetPosition(currentCell);

            //Get the x and y coordinates of the neighbor
            var (nX, nY) = GetPosition(randomNeighbor);

            //If the neighbor is above the current cell
            if (nY < y)
            {
                current.Up = false;
                neighbor.Down = false;
            }
            else if (nY > y)
            {
                current.Down = false;
                neighbor.Up = false;
            }
            else if (nX < x)
            {
                current.Left = false;
                neighbor.Right = false;
            }
            else if (nX > x)
            {
                current.Right = false;
                neighbor.Left = false;
            }
        }

        private static List<int> GetUnvisitedNeighbors(int currentCell, List<int> visitedCells)
        {
            var neighbors = new List<int>();

            var mapSizeY = UsefulConstant.MapSizeY;

            var (x, y) = GetPosition(currentCell);

            //Check if the cell above is unvisited
            var nX = x;
            var nY = y - 1;
            var nIndex = nY * MapSizeX + nX;
            if (y > 0 && !visitedCells.Contains(nIndex))
            {
                neighbors.Add(nIndex);
            }

            //Check if the cell below is unvisited
            nX = x;
            nY = y + 1;
            nIndex = nY * MapSizeX + nX;
            if (y < mapSizeY - 1 && !visitedCells.Contains(nIndex))
            {
                neighbors.Add(nIndex);
            }

            //Check if the cell to the left is unvisited
            nX = x - 1;
            nY = y;
            nIndex = nY * MapSizeX + nX;
            if (x > 0 && !visitedCells.Contains(nIndex))
            {
                neighbors.Add(nIndex);
            }

            //Check if the cell to the right is unvisited
            nX = x + 1;
            nY = y;
            nIndex = nY * MapSizeX + nX;
            if (x < MapSizeX - 1 && !visitedCells.Contains(nIndex))
            {
                neighbors.Add(nIndex);
            }

            return neighbors;
        }

        private static (int, int) GetPosition(int index)
        {
            var x = index % MapSizeX;
            var y = index / MapSizeX;
            return (x, y);
        }

        /// <summary>
        /// Function to check movable of player
        /// </summary>
        /// <param name="original"> Player current position </param>
        /// <param name="direction"> Direction in form of Rotation Degree (ex: 90, 180, 360) </param>
        /// <returns> If Player can move on that Direction </returns>
        public bool CheckPlayerMovable(Vector3 original, int direction)
        {
            var x = Mathf.RoundToInt(original.x);
            var y = Mathf.RoundToInt(-original.z);

            var index = y * MapSizeX + x;

            var cell = _map[index];

            var d = (direction % 360) / 90;

            return d switch
            {
                0 => !cell.Up,
                1 => !cell.Right,
                2 => !cell.Down,
                3 => !cell.Left,
                -1 => !cell.Left,
                -2 => !cell.Down,
                -3 => !cell.Right,
                _ => false
            };
        }

        /// <summary>
        /// Use To Update Player Position.
        /// If Player is on Collectable, Send Update to GamePlayManager
        /// </summary>
        /// <param name="position"> Position of the player </param>
        public void UpdatePlayerPosition(Vector3 position)
        {
            var x = Mathf.RoundToInt(position.x);
            var y = Mathf.RoundToInt(-position.z);

            _playerPosition = y * MapSizeX + x;

            foreach (var (pos, color) in _collectablePosition)
            {
                if (pos != _playerPosition) continue;
                _collectablePosition.Remove((pos, color));
                DestroyCollectable(pos);
                Debug.Log($"Player Collect Color: {color}");
                gamePlay.UpdatePlayerOnCollect(color);
                return;
            }
        }

        /// <summary>
        /// Add Collectable to Map
        /// </summary>
        /// <param name="x"> Position X </param>
        /// <param name="y"> Position Y </param>
        /// <param name="color"> Color id of the object </param>
        public void AddCollectable(int x, int y, int color)
        {
            if (_collectablePosition.Count == _maxCollectable) return;
            var index = y * MapSizeX + x;
            // Prevent duplicate collectable
            if (_collectablePosition.Contains((index, color))) return;
            CreateCollectable(x, y, color, index);
        }

        private readonly Dictionary<int, GameObject> _collectableObjects = new();
        private static readonly int Color1 = Shader.PropertyToID("_color");

        private void CreateCollectable(int x, int y, int color, int index)
        {
            var instantiate = Instantiate(collectablePrefab);
            instantiate.transform.position = new Vector3(x, 0, -y);
            instantiate.GetComponent<MeshRenderer>().material.SetColor(Color1, gamePlay.GetColor(color));
            _collectableObjects.Add(y * MapSizeX + x, instantiate);
            _collectablePosition.Add((index, color));
        }
        
        private void DestroyCollectable(int index)
        {
            if (!_collectableObjects.ContainsKey(index)) return;
            Destroy(_collectableObjects[index]);
            _collectableObjects.Remove(index);
            _collectablePosition.RemoveAll(x => x.Item1 == index);
        }
    }
}