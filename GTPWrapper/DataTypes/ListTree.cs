using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTPWrapper.DataTypes {
    public class ListTree<T> {
        /// <summary>
        /// Gets or sets the list of elements of this tree.
        /// </summary>
        public List<T> Elements { get; set; }
        /// <summary>
        /// Gets or sets the list of subtrees of this tree.
        /// </summary>
        public List<ListTree<T>> SubTrees { get; set; }

        /// <summary>
        /// Initializes a new instance of the ListTreeNode class with an empty list.
        /// </summary>
        public ListTree() : this(new List<T>()) { }
        /// <summary>
        /// Initializes a new instance of the ListTreeNode class.
        /// </summary>
        public ListTree(List<T> list) {
            this.Elements = list;
            this.SubTrees = new List<ListTree<T>>();
        }

        /// <summary>
        /// Returns a linear list of the elements in the tree as IEnumerable.
        /// </summary>
        public IEnumerable<T> AsEnumerable() {
            foreach (T element in this.Elements) yield return element;
            if (this.SubTrees.Count == 0) yield break;
            foreach (T element in this.SubTrees[0].AsEnumerable()) yield return element;
        }
    }
}