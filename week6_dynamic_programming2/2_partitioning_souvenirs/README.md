# Partitioning Souvenirs

> You and two of your friends have just returned back home after visiting various
> countries. Now you would  like to evenly split all the souvenirs that all three of you bought.

This problem is also called the [3-Partition Problem](https://en.wikipedia.org/wiki/3-partition_problem).

See also:

- [Partition of a set into K subsets with equal sum](https://www.geeksforgeeks.org/partition-set-k-subsets-equal-sum/)

## Problem description

Given _n_ gold bars, find the maximum weight of gold that fits into a bag of capacity _W_.

##### Input Format

The first line contains an integer _n_. The second line contains integers _v(1), v(2), ..., v(n)_ 
separated by spaces.

##### Constraints
 
- 1 ≤ n ≤ 20
- 1 ≤ v(i) ≤ 30 for all _i_

##### Output Format

Output `1`, if it possible to partition v(1), v(2), ..., v(n) into three subsets with equal sums,
and `0` otherwise.

### Sample

##### Sample 1

Input:

```text
4
3 3 3 3
```

Output:

```text
0
```

While the sum of inputs is `12` and thus divisible by 3, there is
no way to create three groups of value `12 / 3 = 3`.

##### Sample 2

Input:

```text
1
40
```

Output:

```text
0
```

At least three items are required for this problem, and `40` is also
not divisible by `3` (without remainder).

##### Sample 3

Input:

```text
11
17 59 34 57 17 23 67 1 18 2 59
```

Output:

```text
1
```
Here, `34 + 67 + 17` = `23 + 59 + 1 + 17 + 18` = `59 + 2 + 57`. This is how it plays out:


| Sum     |   |   |    |    |    |    |    |    |    |    |    |
|---------|---|---|----|----|----|----|----|----|----|----|----|
| **354** | 1 | 2 | 17 | 17 | 18 | 23 | 34 | 57 | 59 | 59 | 67 |
| **118** |   |   | 17 |    |    |    | 34 |    |    |    | 67 |
| **118** | 1 |   |    | 17 | 18 | 23 |    |    | 59 |    |    |
| **118** |   | 2 |    |    |    |    |    | 57 |    | 59 |    |

To begin with, `354` is divisible by 3, and three groups of value `354 / 3 = 118`
can be formed.

##### Sample 3

Input:

```text
13
1 2 3 4 5 5 7 7 8 10 12 19 25
```

Output:

```text
1
```

Here, `1 + 3 + 7 + 25` = `2 + 4 + 5 + 7 + 8 + 10` = `5 + 12 + 19`. As a table:

| Sum     |   |   |   |   |   |   |   |   |   |    |    |    |    |
|---------|---|---|---|---|---|---|---|---|---|----|----|----|----|
| **108** | 1 | 2 | 3 | 4 | 5 | 5 | 7 | 7 | 8 | 10 | 12 | 19 | 25 |
| **36**  | 1 |   | 3 |   |   |   | 7 |   |   |    |    |    | 25 |
| **36**  |   | 2 |   | 4 | 5 |   |   | 7 | 8 | 10 |    |    |    |
| **36**  |   |   |   |   |   | 5 |   |   |   |    | 12 | 19 |    |

To begin with, `108` is divisible by 3, and three groups of value `108 / 3 = 36`
can be formed.