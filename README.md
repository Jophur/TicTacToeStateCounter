# TicTacToeStateCounter
My son asked me last night: How many different Tic Tac Toe boards are there?
What if you remove all the reflections and rotations that are basically the same?

For example if you take the mirror image of a board, it's basically the same board
even though it's technically different

We found 7 transformations that are functionally equivalent.
	For example: `xox...x.o` is functionally equivalent to: `x.oo..x.x` in terms of decision making
 
The transformations are:
1.	Rotate (counter clockwise) once
2.	Rotate twice
3.	Rotate three times
4.	Flip on vertical axis (MirrorImage in my code)
5.	Flip each of the 3 rotations on the vertical axis

So how I do this is I start with a new game (root), and visit all possible moves (nodes)
	When I find a node that I haven't seen before, I add it to a list of visited nodes
	Then I add all the transformations to the list of visited nodes
	When I find an end-state, either because someone won or because there are no more moves,
	  I add it to the result collection
	  If it is an end-state because someone won, I stop visiting child nodes
	 

Output
 - Unique states: 5478
 - Unique states, ignoring symmetry: 765
 - Unique end states: 958
 - Unique end states, ignoring symmetry: 138
