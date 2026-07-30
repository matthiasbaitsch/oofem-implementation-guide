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

`oofem-csharp/` holds the instructor's reference C# implementation: a single
`oofem` project (the FE program) and a single `oofem-visualizer` project (the
`Visualizer` class, references `oofem`). Each `Program.cs` is one
progressively-growing script — earlier guide stages are shown by including
only a line range of the same final file, not by keeping separate per-stage
projects/snapshots around. Uses Math.NET Numerics for `Vector`/`Matrix`.

Chapters pull code straight from these files via the `include-code-files`
Quarto filter:

```{.csharp include="oofem-csharp/oofem/Program.cs" start-line=1 end-line=17}
```

Note: `oofem-visualizer/Program.cs` calls `BoDrawApp.Show()` more than once
(after each drawing stage: system, deformation, element forces) — confirmed
safe to call repeatedly, so each guide section's line range ends in its own
`app.Show()` and is independently meaningful.

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
- **"How does X work?" is a question, not a change request.** When the
  user asks how to do something (e.g. "how can I define a function in its
  own file"), answer in words/code snippet — do not go edit their files
  unless they separately ask for the change to be made.
- **Memory/feedback about how to work in this repo goes here in
  CLAUDE.md** (it's checked into the repo), not in the global
  `~/.claude/.../memory/` store.
