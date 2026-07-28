import numpy as np
import pyvista as pv


class Visualizer:

    def __init__(self, structure):

        self.structure = structure

        # Model visualization
        self.element_radius = 0.02
        self.node_radius = 0.04

        self.constraint_scale = 0.15
        self.force_scale = 0.25
        self.force_radius = 0.015

        # Result visualization
        self.undeformed_radius = 0.01
        self.deformed_radius = 0.025
        self.displacement_scale = 1.0

    def draw_elements(self, plotter):

        for element in self.structure.elements:

            point1 = element.node1.position
            point2 = element.node2.position

            line = pv.Line(point1, point2)

            plotter.add_mesh(
                line.tube(radius=self.element_radius),
                color="gray",
            )

    def draw_nodes(self, plotter):

        for node in self.structure.nodes:

            sphere = pv.Sphere(
                radius=self.node_radius,
                center=node.position,
            )

            plotter.add_mesh(
                sphere,
                color="black",
            )

    def draw_constraints(self, plotter):

        coordinate_directions = [
            np.array([1.0, 0.0, 0.0]),
            np.array([0.0, 1.0, 0.0]),
            np.array([0.0, 0.0, 1.0]),
        ]

        for node in self.structure.nodes:

            for i in range(3):

                if node.constraint.fixed[i]:

                    direction = coordinate_directions[i]

                    cone_center = (
                        node.position
                        - 0.5
                        * self.constraint_scale
                        * direction
                    )

                    cone = pv.Cone(
                        center=cone_center,
                        direction=direction,
                        height=self.constraint_scale,
                        radius=0.5 * self.constraint_scale,
                    )

                    plotter.add_mesh(
                        cone,
                        color="blue",
                    )

    def draw_forces(self, plotter):

        for node in self.structure.nodes:

            force = node.force.components
            force_magnitude = np.linalg.norm(force)

            if force_magnitude > 0.0:

                direction = force / force_magnitude

                arrow_length = (
                    self.force_scale
                    * force_magnitude
                )

                arrow_start = (
                    node.position
                    - arrow_length * direction
                )

                arrow = pv.Arrow(
                    start=arrow_start,
                    direction=direction,
                    scale=arrow_length,
                    shaft_radius=self.force_radius,
                    tip_radius=2.0 * self.force_radius,
                )

                plotter.add_mesh(
                    arrow,
                    color="red",
                )

    def draw_model(self):

        plotter = pv.Plotter()

        self.draw_elements(plotter)
        self.draw_nodes(plotter)
        self.draw_constraints(plotter)
        self.draw_forces(plotter)

        plotter.add_axes()
        plotter.enable_anti_aliasing()
        plotter.view_xy()

        plotter.show()

    def _draw_undeformed_structure(self, plotter):

        for element in self.structure.elements:

            point1 = element.node1.position
            point2 = element.node2.position

            line = pv.Line(
                point1,
                point2,
            )

            tube = line.tube(
                radius=self.undeformed_radius
            )

            plotter.add_mesh(
                tube,
                color="gray",
                opacity=0.45,
            )

    def _draw_deformed_structure(self, plotter):

        for element in self.structure.elements:

            deformed_point1 = (
                element.node1.position
                + self.displacement_scale
                * element.node1.displacement
            )

            deformed_point2 = (
                element.node2.position
                + self.displacement_scale
                * element.node2.displacement
            )

            line = pv.Line(
                deformed_point1,
                deformed_point2,
            )

            tube = line.tube(
                radius=self.deformed_radius
            )

            plotter.add_mesh(
                tube,
                color="blue",
            )

    def draw_deformed_structure(self):

        plotter = pv.Plotter()

        self._draw_undeformed_structure(plotter)
        self._draw_deformed_structure(plotter)

        self.draw_nodes(plotter)
        self.draw_constraints(plotter)
        self.draw_forces(plotter)

        plotter.add_axes()
        plotter.enable_anti_aliasing()
        plotter.view_xy()

        plotter.show()

    def _draw_element_forces(self, plotter):

        normal_forces = np.array(
            [
                element.normal_force()
                for element in self.structure.elements
            ],
            dtype=float,
        )

        if len(normal_forces) == 0:
            return None

        maximum_force = np.max(
            np.abs(normal_forces)
        )

        if maximum_force < 1e-12:
            maximum_force = 1.0

        color_limits = [
            -maximum_force,
            maximum_force,
        ]

        normal_force_mapper = None

        for element in self.structure.elements:

            deformed_point1 = (
                element.node1.position
                + self.displacement_scale
                * element.node1.displacement
            )

            deformed_point2 = (
                element.node2.position
                + self.displacement_scale
                * element.node2.displacement
            )

            line = pv.Line(
                deformed_point1,
                deformed_point2,
            )

            normal_force = float(
                element.normal_force()
            )

            line.point_data["Normal force"] = np.array(
                [
                    normal_force,
                    normal_force,
                ],
                dtype=float,
            )

            tube = line.tube(
                radius=self.deformed_radius
            )

            actor = plotter.add_mesh(
                tube,
                scalars="Normal force",
                cmap="bwr",
                clim=color_limits,
                show_scalar_bar=False,
            )

            if normal_force_mapper is None:
                normal_force_mapper = actor.mapper

        return normal_force_mapper

    def draw_element_forces(self):

        plotter = pv.Plotter()

        self._draw_undeformed_structure(plotter)

        normal_force_mapper = (
            self._draw_element_forces(plotter)
        )

        self.draw_nodes(plotter)
        self.draw_constraints(plotter)
        self.draw_forces(plotter)

        if normal_force_mapper is not None:

            plotter.add_scalar_bar(
                title="Normal force [N]",
                mapper=normal_force_mapper,
                n_labels=5,
                fmt="%.2e",
                vertical=False,
            )

        plotter.add_axes()
        plotter.enable_anti_aliasing()
        plotter.view_xy()

        plotter.show()