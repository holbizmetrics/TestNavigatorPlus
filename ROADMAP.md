TestNavigatorPlus - Roadmap

> From Outstanding to Iconic
---

PHASE 0: MVP (v0.1 – v0.5)

Goal: Solid foundations for navigation & insights

[x] Toolbar + Command registration

[x] Navigation for: Tests, Bugs (TODO/FIXME), Errors

[x] Bookmark detection

[x] Source-to-Test file switching

[x] VSCT toolbar setup & keybindings

[ ] Tool window scaffolding (initial status dashboard)

[ ] Auto-refresh logic (after build or file change)

[ ] Minimal telemetry: errors + usage (opt-in)



---

PHASE 1: Developer Workflow Enhancer (v1.0 – v1.2)

Goal: Make test filtering, execution, and visibility top-tier

[ ] Persistent test run results

[ ] LINQ-based test filtering (category, trait, file, etc.)

[ ] Toggleable filters (failed, skipped, passed)

[ ] "Run Visible Tests" feature

[ ] Test Timeline View (sortable list w/ durations)

[ ] Status bar insights: test stats

[ ] "Go to Corresponding Test/Source" via shortcut


Nice-to-Haves:

[ ] Regex test finder

[ ] Collapse/expand test groups in tool window

[ ] Pinned tests feature

[ ] Manual group creation / tagging



---

PHASE 1.5: Quality & Cleanup Tools

Goal: Improve code hygiene and surface bad practices early

Test Code Analyzer

[ ] Detects missing asserts or empty test methods

[ ] Flags duplicated logic or copy-paste between tests

[ ] Warnings for catch-all try/catch blocks

[ ] Alerts for untagged/uncategorized tests

[ ] Lightbulb-style "Fix Suggestions"


Test File Cleanup Utility

[ ] Removes excessive blank lines / normalizes line spacing

[ ] Normalizes line endings

[ ] Cleans up web-pasted code (double spacing etc.)

[ ] Context menu: "Clean Test Formatting"

[ ] Optional: Auto-format on save



---

PHASE 2: Intelligence & Coverage (v2.0 – v2.5)

Goal: Add diagnostics, AI-powered summaries, and test quality layers

[ ] Code coverage integration (.coverage parsing + highlighting)

[ ] Test Coverage Analyzer results in tool window

[ ] Visualizer: tested/untested public methods

[ ] Flaky test detection (multiple reruns)

[ ] Failure summarization via GPT (opt-in)

[ ] "Why did this fail?" trace AI

[ ] Smart rerun suggestions

[ ] GC/memory insights per test (optional)



---

PHASE 2.5: Debugger Visualizer Lite

Goal: Enhance test-debug experience with rich live data

[ ] Inline Variable Visualizer

[ ] Highlight changed values during debug

[ ] Preview previous values (simple history)

[ ] Smart object expansion

[ ] Conditional branch hits/shown inline ("Hit"/"Skipped")

[ ] Color-coded highlights: exceptions/nulls/warnings


Nice-to-Haves:

[ ] Expression tracking across frames

[ ] Minimal inline graph view (collections, trees)

[ ] Breakpoint Analyzer (predict if hit/missed)



---

PHASE 3: Integration & Extensibility (v3.0 – v3.5)

Goal: Play well with external tools, services, and team workflows

[ ] Plugin system for analyzers: Stryker, BenchmarkDotNet, etc.

[ ] GitHub PR test summary (markdown or .trx visual)

[ ] VS Code test JSON compatibility

[ ] Azure DevOps result integration

[ ] Export results to Markdown, HTML, or PDF

[ ] Internal API for external test providers



---

PHASE 3.5+: Legendary Stretch Features

Goal: Make the tool unforgettable

[ ] Context-aware quick actions (e.g., failed test → last screenshot)

[ ] Find tests that touch a specific method

[ ] Test repetition insights: detect slowdowns, leaks

[ ] Virtual test suites: user-created dynamic groups

[ ] Test Data Sniffing: refactor magic strings/test cases

[ ] Idle-time suggestions: rerun failures when idle

[ ] Visual diff of input/output for data-driven tests

---

PHASE 4: Soul & Delight

Goal: Make it iconic, memorable, and joyful to use

Sensory Feedback

[ ] Subtle sound cues on pass/fail (optional)

[ ] Audio/vibration on long test run complete


Smart Delight

[ ] "Zen Mode" during test runs (calming animation or glow)

[ ] ASCII animations (e.g., rocket launches for 100% pass)

[ ] Debugger Cat mascot with commentary


AI Partnering

[ ] GPT prompt: "Suggest assertions for this test"

[ ] GPT prompt: "Suggest edge cases for this method"

[ ] GPT prompt: "Why is this test flaky?"


Developer Love

[ ] Gamified feedback: test streaks, coverage boosts

[ ] Shareable badges: test refactor wins, test suite mastery

[ ] Retro mode: toggle 90s debug visuals for fun

---

FUTURE: Team & Collaboration (Nice-to-Have)

Goal: Help teams align on test activity

[ ] View tests recently run by teammates (repo-based)

[ ] Sync pinned/failing test sets across devs

[ ] Shared test group links / "Test playlists"

[ ] Live view: what others are running

---

Release Milestones

---

Built with care, for devs who test with precision.
Test Navigator Plus: From Outstanding to Iconic.