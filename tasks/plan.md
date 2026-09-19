# AlgoMotion enrichment continuation

Source: Claude session 1778c0b3-d447-4747-8be2-3168ed216c63.
User approved all nine features, easiest first, with autonomous continuation.
No commits or pushes.

Architecture: retain standalone Blazor WASM, pure recorded simulations and
client-side rendering. Reuse current players for sorting; new graph/DP recordings
use a reusable indexed playback component. All new learning pages support VI/EN.
Bound inputs and recorded histories to keep browser work predictable.

1. Finish custom input: shared validated parser; duplicate values render safely.
2. Add deterministic executable regression tests for all sorting/tree simulators,
   playback, parser and later graph/DP. GitHub Actions runs checks and publish.
3. Complexity lab: measured comparison/write counters for reproducible inputs,
   reference growth curves, numeric table and honest metric limitations.
4. Comparison: same input, two algorithms, synchronized step controls, counters.
5. Quiz: localized question bank, one answer per question, explanations, score, retry.
6. Graph: BFS, DFS, Dijkstra and Prim; weighted undirected input, recorded node/edge
   states, distance/visited table; disconnected and invalid input handled.
7. DP: Fibonacci, 0/1 knapsack and LCS; recorded tables and recurrence explanation.
8. Verify build, regression suite and browser flows; document usage and limitations.

Verification checkpoints follow tests, learning pages, and new algorithm families.
Avoid adding chart/runtime dependencies. Tests compile production pure source into
a console runner (nonzero exit on failed assertions), avoiding a WASM test host.
