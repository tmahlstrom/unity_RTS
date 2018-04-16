
This set of scripts controls the state of the Units.

Each Unit has a UnitBaseState variable, from which the different states (MoveState, AttackState, etc.) inherit.

When a Unit changes state, one [State]:UnitBaseState is switched out for another, undergoing the corresponding exit and initiate routines in the process.

This design architecture makes it easy to make the user input passed to the unit sensitive to the state it is in. For the simplest example, right clicks are ignored when they are passed to units in the DeadState. 


