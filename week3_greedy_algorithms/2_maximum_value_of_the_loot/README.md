# Fractional Knapsack: Maximum Value of the Loot

> A thief finds much more loot than his bag can fit. Help him to find the most valuable combination
> of items assuming that any fraction of a loot item can be put into his bag.

Note that this is a variant of a **Fractional Knapsack** problem where an item
can either be taken fully, partially, or not at all.
In such cases, greedy algorithms work perfectly fine.

In algorithm / week 6.1, however, a variant of a **Discrete Knapsack** (without repetitions)
is implemented, where an item is either taken or not at all (think: bars of gold).
For Discrete Knapsack problems, greedy algorithms are _not safe_ and (generally) produce invalid results.
