# Cornucopia.Net

[![GitHub top language](https://img.shields.io/github/languages/top/SWeini/Cornucopia.Net)](https://docs.microsoft.com/dotnet/csharp/)
[![Nuget Cornucopia.DataStructures](https://img.shields.io/nuget/v/Cornucopia.DataStructures?label=Cornucopia.DataStructures&logo=nuget)](https://www.nuget.org/packages/Cornucopia.DataStructures/)
[![GitHub last commit](https://img.shields.io/github/last-commit/SWeini/Cornucopia.Net?logo=GitHub)](https://github.com/SWeini/Cornucopia.Net)
[![GitHub pull requests](https://img.shields.io/github/issues-pr/SWeini/Cornucopia.Net?logo=GitHub)](https://github.com/SWeini/Cornucopia.Net/pulls)
[![GitHub issues](https://img.shields.io/github/issues/SWeini/Cornucopia.Net?logo=GitHub)](https://github.com/SWeini/Cornucopia.Net/issues)
[![GitHub Workflow Status](https://img.shields.io/github/workflow/status/SWeini/Cornucopia.Net/dotnet%20package/master?logo=GitHub)](https://github.com/SWeini/Cornucopia.Net/actions/workflows/build.yaml?query=branch%3Amaster)
[![GitHub](https://img.shields.io/github/license/SWeini/Cornucopia.Net)](https://github.com/SWeini/Cornucopia.Net/blob/master/LICENSE)

---

The **horn of plenty** for .NET developers.

- Pure .NET (no unmanaged code)
- No dependencies (sometimes reinventing the wheel)
- Low-level functionality (instead of OO overhead)
- Focus on performance (instead of safety/conventions/usability)
- Use modern technology (instead of compatibility)

## Data Structures

- For special (performance) needs
- Not implementing `System.Collections.Generic.IEnumerable<T>`
- Uses value types where appropriate

<details><summary>Design philosophy</summary>

The `System.Collections` namespace provides a great set of collections for day-to-day use. If you're looking beyond that you probably have performance issues and have already ditched `System.Linq`. Suddenly, implementing `System.Collections.Generic.IEnumerable<T>` becomes less important. Also consider that `IEnumerable<T>` is a great abstraction, but very difficult to implement efficiently in tree-like data structures.

If there's no need to implement an interface, it's also easier to diverge from common standards. For some collection types, `null` denotes an empty collection. Some collection types are value types, just providing a convenient API layer instead of adding another level of indirection.

These decisions focus on performance, but do not prevent creating a wrapper around the low-level data structures, providing behavior known from the `System.Collections` namespace.

</details>

### Random Access List
Persistent data structure based on skew binary numbers.
- **O(1)** insert/delete on one end (like `System.Collections.Immutable.ImmutableStack<T>`)
- **O(log(N))** get/set by index (like `System.Collections.Immutable.ImmutableList<T>`)

### Finger Tree
Persistent data structure based on a 2-3-tree, combined with a zipper.
- **O(1) amortized** insert/delete on both ends
- **O(log(N))** insert/delete/get/set by index (like `System.Collections.Immutable.ImmutableList<T>`)
- **O(log(N))** concatenate (like `System.Collections.Immutable.ImmutableList<T>`)
- **O(log(N))** split (in comparison to **O(N*log(N))** for `System.Collections.Immutable.ImmutableList<T>`)

### Space-Optimal Dynamic Array
Ephemeral data structure for dynamic arrays
- **O(1) amortized** insert/delete on one end (like `System.Collections.Generic.List<T>`)
- data not copied when resizing (unlike `System.Collections.Generic.List<T>`)
- **O(1)** get/set by index
- **O(sqrt(N))** wasted space (in comparison to **O(n)** for `System.Collections.Generic.List<T>`)

### Persistent Binary Tree
Persistent data structure for binary trees, used internally

### Persistent Singly Linked List
Persistent data structure for linked lists, used internally, similar to `System.Collections.Immutable.ImmutableStack<T>`

## Future Plans

Currently I'm thinking about these additions (in no particular order), many of which I've already implemented previously:
- Sorting algorithms (stable / adaptive)
- Disjoint Set (also known as Union-Find)
- (Persistent) Balanced Binary Trees (AVL / RedBlack / Splay)
- Persistent Queue
- Persistent Deque
- Heaps (Binary / Binomial / Fibonacci / Weak / MinMax)
- (Persistent / Concurrent) Hash Array Mapped Trie
- RRB Vector
- Graphs (Undirected / Directed) with most basic algorithms (e.g. spanning tree, strongly connected components, shortest path)
- Prefix Trie
- Suffix Tree

## About the author

Hi there, my name is Simon Weinberger and I live in Germany. I enjoy developing software and do that professionally for my daytime job. My special interests are data structures, math, JIT code generation, parser generators, and everything related to .NET internals.

Cornucopia.Net is a hobby project of mine. Please reach out to me if you have questions or feedback.