
"Al Galope" A Knight tour finder in Unity.
-------------------------------------------

Unity project in C# to generate and visualize knight tours. It can generate knight tours of any size through brute force or using the Wansdorf’s rule.

A **knight's tour** is a sequence of moves of a knight on a chessboard such that the knight visits every square exactly once. 
If the knight ends on a square that is one knight's move from the beginning square 
(so that it could tour the board again immediately, following the same path), the tour is closed (or re-entrant); otherwise, it is open. [1]

Below is a closed knight tour on a 8x8 chessboard.

<img src="https://github.com/ainac99/AlGalope/assets/48329852/64933f58-2ef3-4c71-bdaf-4078ab53f596" width="600" height="600">


To find a complete knight tour (one that visits all squares in the board) one can use the Warnsdorf's rule, that is:

- start from any square
- for each move you will have up to 8 possible squares to choose: 
  - out of these squares, remove the ones already visited.
  - of the remaining ones, check, for each one, how many possible moves you will have in the following move. Choose the square with the __least__ amount of future moves.

The Wandorf’s rule guarantees the finding of a single knight tour but generally it does not lead to a closed tour.

Originally knight tours were defined for 8x8 chessboards, however, knight tours can be found for any m x n (m <= n) unless any of these conditions are met:
1. m = 1 or 2
2. m = 3 and n = 3, 5, or 6
3. m = 4 and n = 4.

The following is an open knight tour on a 20x20 board, generated using the Wansdorf's rule.

![An open knight tour on a 20x20 chess board](https://github.com/ainac99/AlGalope/assets/48329852/c39ad6b3-9ea9-404a-884b-4e21e42c4c2a)

**References**

[1] Knight's Tour. Wikipedia. Available at: https://en.wikipedia.org/wiki/Knight%27s_tour
