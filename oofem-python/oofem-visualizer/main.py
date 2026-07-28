import sys
from pathlib import Path


# Add the neighboring "oofem" folder to the Python search path
oofem_directory = (
    Path(__file__).resolve().parent.parent / "oofem"
)

sys.path.append(str(oofem_directory))


from structure import Structure
from visualizer import Visualizer


# ------------------------------------------------------------
# Create the structure
# ------------------------------------------------------------

structure = Structure()


# ------------------------------------------------------------
# Create nodes
# ------------------------------------------------------------

node1 = structure.add_node(
    0.0,
    0.0,
    0.0,
)

node2 = structure.add_node(
    4.0,
    0.0,
    0.0,
)

node3 = structure.add_node(
    0.0,
    3.0,
    0.0,
)


# ------------------------------------------------------------
# Apply constraints
# ------------------------------------------------------------

# Node 1 is fully fixed
node1.constraint.fixed = [
    True,
    True,
    True,
]

# Node 2 may move only in the x-direction
node2.constraint.fixed = [
    False,
    True,
    True,
]

# Node 3 may move in the x-y plane
node3.constraint.fixed = [
    False,
    False,
    True,
]


# ------------------------------------------------------------
# Apply force
# ------------------------------------------------------------

node3.force.components[0] = 1200.0


# ------------------------------------------------------------
# Create elements
# ------------------------------------------------------------

youngs_modulus = 10e9
cross_section_area = 1e-4

structure.add_element(
    youngs_modulus,
    cross_section_area,
    0,
    1,
)

structure.add_element(
    youngs_modulus,
    cross_section_area,
    0,
    2,
)

structure.add_element(
    youngs_modulus,
    cross_section_area,
    2,
    1,
)


# ------------------------------------------------------------
# Print model information
# ------------------------------------------------------------

structure.print()


# ------------------------------------------------------------
# Create and configure the visualizer
# ------------------------------------------------------------

visualizer = Visualizer(structure)

visualizer.element_radius = 0.025
visualizer.node_radius = 0.07

visualizer.constraint_scale = 0.18


visualizer.force_radius = 0.018

visualizer.undeformed_radius = 0.018
visualizer.deformed_radius = 0.03


# ------------------------------------------------------------
# Show model, supports, and applied load
# ------------------------------------------------------------

visualizer.draw_model()


# ------------------------------------------------------------
# Solve the structure
# ------------------------------------------------------------

structure.solve()

visualizer.displacement_scale = 10.0
visualizer.draw_deformed_structure()


# ------------------------------------------------------------
# Show element normal forces
# ------------------------------------------------------------

visualizer.force_scale = 0.0006
visualizer.draw_element_forces()


# ------------------------------------------------------------
# Print analysis results
# ------------------------------------------------------------

structure.print_results()