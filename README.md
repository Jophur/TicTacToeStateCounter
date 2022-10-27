# TicTacToeStateCounter
## Background
My son asked me last night: 
  "How many different Tic Tac Toe boards are there? What if you remove all the reflections and
  rotations that are basically the same?

  "For example if you take the mirror image of a board, it's basically the same board even though
  it's technically different"

## Existing Research
[This article on StackExchange](https://math.stackexchange.com/a/613505) did a great job of
analyzing the problem. I used his assumptions except for #2 where he says: `We're ignoring symmetry`

## Symmetry
We found 7 transformations that generate a functionally equivalent state.
  For example, in terms of decision making, these two tic tac toe boards are the same

|   |   |   |
| - | - | - |
| x | o | x |
| . | . | . |
| x | . | o |


|   |   |   |
| - | - | - |
| x | . | o |
| o | . | . |
| x | . | x |
 
The transformations are:
1.	Rotate once (my code rotates counter-clockwise)
2.	Rotate twice
3.	Rotate three times
4.	Flip on vertical axis (MirrorImage in my code)
5.	Flip each of the 3 rotations on the vertical axis

## Algorithm
Start with a new game and traverse the decision tree
1. Start with a new game, add it to a queue
2. While there are nodes in the queue
    a. Remove a node from the queue
    b. If the node is not in the list of visited nodes,
        i. Add it to the list
        ii. Add all of its symmetrical transforms to the list
        iii. If the node is not an end state,
            1. Add each possible next move to the queue

## Musings
I originally wrote a recursive algorithm that took a boolean parameter that specified whether or
not to ingore symmetry.

When the flag was false, I did not add the transforms (step 2.b.ii.) and instead would traverse
the tree from every single node.

This took a long time and used a lot of memory. So I decided to rewrite my code using iteration.

I also took a good look at my algorithm and the decision tree and disovered that for any node *N*,
any transform operation *T*(*N*), and the set of *N*'s children *C*(*N*):
The set of transforms of *N*'s children, *T*(*C*(*N*)), is the same as the set of children of *N*'s
transform, *C*(*T*(*N*)). Or: *T*(*C*(*N*)) = *C*(*T*(*N*))

What this means is that *I don't have to visit each node*. Using a 2x2 example:

| *N*  |   |   |  |   |   |   |   |
| - | - | - | - | - | - | - | - |
| x | o | *C*(*N*) => | x | o | and | x | o |
| . | . |   | x | . |   | . | x |

| *T*(*N*)  |   |   |  |   |   |   |   |
| - | - | - | - | - | - | - | - |
| o | . | *C*(*T*(*N*)) => | o | x | and | o | x |
| x | . |   | x | x |   | x | . |

Here you can see that the possible moves from *T*(*N*)) are transforms of the possible moves of *N*.
So as long as I ensure the transforms of every visited node are in the list of possible states,
I do not have to traverse the tree from the transforms, because all states will ultimately end up
in the list.

## Results
Here's the results, using the terminology and table layout the original poster used.

| Ply | Unique(1) | NoSymmetries(1) | Unique(2) | NoSymmetries(2) | Unique(3) | NoSymmetries(3) | Unique(4) | NoSymmetries(4) |
| - | - | - | - | - | - | - | - |  |
| 0 | 1 | 1 | 1 | 1 | 1 | 1 | 1 | 1 |
| 1 | 1 | 1 | 4 | 1 | 9 | 3 | 16 | 3 |
| 2 | 2 | 2 | 12 | 2 | 72 | 12 | 240 | 33 |
| 3 |  |  | 12 | 2 | 252 | 38 | 1680 | 219 |
| 4 |  |  | 0 | 0 | 756 | 108 | 10920 | 1413 |
| 5 |  |  | 29 | 6 | 1260 | 174 | 43680 | 5514 |
| 6 |  |  |  |  | 1520 | 204 | 160160 | 20122 |
| 7 |  |  |  |  | 1140 | 153 | 400400 | 50215 |
| 8 |  |  |  |  | 390 | 57 | 895950 | 112379 |
| 9 |  |  |  |  | 78 | 15 | 1433520 | 179510 |
| 10 |  |  |  |  | 5478 | 765 | 1962576 | 245690 |
| 11 |  |  |  |  |  |  | 1962576 | 245690 |
| 12 |  |  |  |  |  |  | 1543080 | 193318 |
| 13 |  |  |  |  |  |  | 881760 | 110452 |
| 14 |  |  |  |  |  |  | 333792 | 41870 |
| 15 |  |  |  |  |  |  | 83440 | 10489 |
| 16 |  |  |  |  |  |  | 8220 | 1059 |
| Total: |  |  |  |  |  |  | 9722011 | 1217977 |




