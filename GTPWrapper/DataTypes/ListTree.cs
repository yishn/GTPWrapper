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
        /// Returns the element at the given global index, traveling down a subtree if necessary.
        /// </summary>
        /// <param name="index">The global index of the element.</param>
        public T ElementAt(int index) {
            if (Elements.Count > index) return this.Elements[index];
            return this.SubTrees[0].ElementAt(index - Elements.Count);
        }
    }
}