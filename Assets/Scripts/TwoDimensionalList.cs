using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    [System.Serializable]
    public class TwoDimensionalList<T>
    {
        private static readonly Vector2Int DEFAULT_SIZE = new Vector2Int (5, 5);
        [SerializeField] protected List<T> list;
        public Vector2Int size;
        public List<T> List => this.list;
        public T GetColumn (Vector2Int vector) => GetColumn (vector.x, vector.y);
        public T GetColumn (int x, int y)
        {
            if (CheckSize (x, y) == false)
            {
                Debug.LogError ("インデックス指定エラー");
                return default (T);
            }
            if (this.list == null)
            {
                Debug.LogError ("リスト初期化エラー");
                Debug.LogError (list.Count);
                return default (T);
            }
            return this.list[GetIndex (x, y)];
        }

        public void SetColumn (Vector2Int vector, T value) => SetColumn (vector.x, vector.y, value);
        public void SetColumn (int x, int y, T value)
        {
            if (CheckSize (x, y) == false)
            {
                Debug.LogError ("インデックス指定エラー");
                return;
            }
            this.list[GetIndex (x, y)] = value;
        }

        public Vector2Int GetPosition (T target)
        {
            var index = this.list.IndexOf (target);
            if (index < 0)
            {
                Debug.LogError ("要素が見つかりません");
                return Vector2Int.zero;
            }
            return Index2Position (index);
        }

        private Vector2Int Index2Position (int index) => new Vector2Int (index % size.x, index / size.x);

        protected bool CheckSize (int x, int y)
        {
            if (x < 0 || x > this.size.x - 1) return false;
            if (y < 0 || y > this.size.y - 1) return false;
            return true;
        }

        private int GetIndex (Vector2Int pos) => GetIndex (pos.x, pos.y);
        private int GetIndex (int x, int y) => size.x * y + x;

        public T this [int x, int y]
        {
            get => GetColumn (x, y);
            set => SetColumn (x, y, value);
        }

        public T this [Vector2Int pos]
        {
            get => this [pos.x, pos.y];
            set => this [pos.x, pos.y] = value;
        }

        public TwoDimensionalList () : this (DEFAULT_SIZE) { }
        public TwoDimensionalList (Vector2Int vector) : this (vector.x, vector.y) { }

        public TwoDimensionalList (int x, int y)
        {
            if (x < 0 || y < 0)
            {
                Debug.LogError ("インデックス指定エラー");
                x = y = 0;
            }
            this.size = new Vector2Int (x, y);
            Initialize ();
        }

        public virtual void Initialize ()
        {
            var capacity = this.size.x * this.size.y;
            this.list = new List<T> (capacity);
            for (int i = 0; i < capacity; ++i) this.list.Add (default (T));
        }

        public void Exchange (Vector2Int pos1, Vector2Int pos2) => ExchangeFromIndex (GetIndex (pos1), GetIndex (pos2));

        private void ExchangeFromIndex (int index1, int index2)
        {
            var temp = this.list[index1];
            this.list[index1] = this.list[index2];
            this.list[index2] = temp;
        }

        public TwoDimensionalList<T> Clone ()
        {
            var clone = (TwoDimensionalList<T>) MemberwiseClone ();
            clone.list = new List<T> (this.list);
            return clone;
        }

        public void ChangeSize (Vector2Int size) => ChangeSize (size.x, size.y);
        public void ChangeSize (int sizeX, int sizeY)
        {
            var capacity = sizeX * sizeY;
            var newList = new List<T> (capacity);
            for (int i = 0; i < capacity; ++i) newList.Add (default (T));

            for (int x = 0; x < Mathf.Min (this.size.x, sizeX); ++x)
            {
                for (int y = 0; y < Mathf.Min (this.size.y, sizeY); ++y)
                {
                    var newListIndex = y * sizeX + x;
                    var oldListIndex = y * this.size.x + x;
                    newList[newListIndex] = this.list[oldListIndex];
                }
            }

            this.list = newList;
            this.size = new Vector2Int (sizeX, sizeY);
        }
    }
}