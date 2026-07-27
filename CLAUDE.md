# CLAUDE.md

## What this is

A Quarto book: "Programming the Finite Element Method — Implementation
Guideline". It teaches students to build a small object-oriented FEM program
for 2D/3D truss analysis (nodes, elements, forces, constraints — assemble
stiffness matrix, solve, get displacements/normal forces), in either C# or
Python. The audience is students; the author (and primary user of Claude
here) is the instructor, Matthias Baitsch.

Chapters (see `_quarto.yml`):

- `index.qmd` — front matter
- `introduction.qmd` — approaches to OO-FEM, note on LLM use by students
- `analysis.qmd` — object-oriented analysis (classes, responsibilities)
- `design-and-implementation.qmd` — step-by-step design/implementation guide
- `references.qmd` — bibliography (`references.bib`)

## Reference solution code

`oofem-csharp/` holds the instructor's reference C# implementation, split by
guide stage (`oofem-01` = preprocessing, `oofem-02` = + processing, a
`oofem-visualizer-01` for the `Visualizer` class). Uses Math.NET Numerics for
`Vector`/`Matrix`.

Chapters pull code straight from these files via the `include-code-files`
Quarto filter:

```{.csharp include="oofem-csharp/oofem-02/Program.cs" start-line=1 end-line=17}
```

**When editing these `.cs` files, check the qmd files for `start-line`/
`end-line` references and update them if line numbers shift.** Prefer
appending code rather than inserting in the middle of a referenced range.

There is no Python reference implementation yet — the Python side of the
guide is largely `TODO`. Don't create `oofem-python/` or fill in Python
solutions unless explicitly asked; this is being built incrementally.

## Working on this repo

- This is prose + didactic reference code, not an application — there's no
  test suite. "Correctness" means: the guide reads well, the included code
  compiles/runs and its printed output matches what's shown in the chapter.
- Keep explanations simple and match the existing tone: engineering-style
  OOP (plain classes, public fields, capitalized names), simplicity over
  generality or efficiency, deliberately avoiding premature abstraction
  (see `introduction.qmd`, `design-and-implementation.qmd`).
- C# and Python content is presented side by side via
  `::: {.panel-tabset}` / `## Implementation in C#` / `## Implementation in
  Python` blocks. Keep both tabs present even if the Python one is `TODO`.
- Render/preview with `quarto render` / `quarto preview` from the repo root.
  Output goes to `__output/` (git-ignored territory, don't hand-edit it).
- **Do not write the students' assignment code for them.** The guide
  explicitly tells students not to use LLMs to implement their project
  (`introduction.qmd`) — full solutions belong only in `oofem-csharp/` (and
  a future `oofem-python/`) as instructor reference material, not as
  something generated on request for a student working through the guide.
