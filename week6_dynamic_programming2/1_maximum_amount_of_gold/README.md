# Discrete Knapsack: Maximum Amount of Gold

> You are given a set of bars of gold and your goal is to take as much gold as possible into
> your bag. There is just one copy of each bar and for each bar you can either take it or not
> (hence you cannot take a fraction of a bar).

This type of problem is also known as the 0-1 Knapsack Problem (or 0/1, in some sources).

Note that in contrast to algorithm / week 3.2 (Maximum Value of the Loot),
this is a **Discrete Knapsack** problem where greedy algorithms produce _unsafe_
solutions.

## Problem description

Given _n_ gold bars, find the maximum weight of gold that fits into a bag of capacity _W_.

##### Input Format

The first line of the input contains the capacity _W_ of a knapsack and the number _n_ of bars
of gold. The next line contains _n_ integers _w(0), w(1), ..., w(n−1)_ defining the weights of the bars of gold.

**Note:** Yes, that's correct: For the sake of the problem,
          different bars of gold can have different weights. 🤷

##### Constraints
 
- 1 ≤ W ≤ 10^4
- 1 ≤ n ≤ 300
- 0 ≤ w(0), ..., w(n−1) ≤ 10^5

##### Output Format

Output the maximum weight of gold that fits into a knapsack of capacity _W_.

### Sample

##### Sample 1

Input:

```text
10 3
1 4 8
```

Output:

```text
9
```

Here, the sum of the weights of the first and the last bar is equal to 9.

##### Sample 2

Input:

```text
10 5
3 5 3 3 5
```

Output:

```text
10
```