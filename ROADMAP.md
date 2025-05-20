# TestNavigatorPlus Roadmap

> ### From Outstanding to Iconic

---

## 🚀 PHASE 0: MVP (v0.1 – v0.5)
**Goal: Solid foundations for navigation & insights**

- [x] Toolbar + Command registration
- [x] Navigation for: Tests, Bugs (TODO/FIXME), Errors
- [x] Bookmark detection
- [x] Source-to-Test file switching
- [x] VSCT toolbar setup & keybindings
- [ ] Tool window scaffolding (initial status dashboard)
- [ ] Auto-refresh logic (after build or file change)
- [ ] Minimal telemetry: errors + usage (opt-in)
- [ ] Context Menu: Run Test (Until Failure) in Code Editor, in addition to: Run Test, Debug Test
- [ ] Sorting Tests: Sort Tests in the Same Order as in the Test Class 

---

## 🔍 PHASE 1: Developer Workflow Enhancer (v1.0 – v1.2)
**Goal: Make test filtering, execution, and visibility top-tier**

- [ ] Persistent test run results
- [ ] LINQ-based test filtering (category, trait, file, etc.)
- [ ] Toggleable filters (failed, skipped, passed)
- [ ] "Run Visible Tests" feature
- [ ] Test Timeline View (sortable list w/ durations)
- [ ] Status bar insights: test stats
- [ ] "Go to Corresponding Test/Source" via shortcut

### Nice-to-Haves:
- [ ] Regex test finder
- [ ] Collapse/expand test groups in tool window
- [ ] Pinned tests feature
- [ ] Manual group creation / tagging

---

## 🧹 PHASE 1.5: Quality & Cleanup Tools
**Goal: Improve code hygiene and surface bad practices early**

### Test Code Analyzer
- [ ] Detects missing asserts or empty test methods
- [ ] Flags duplicated logic or copy-paste between tests
- [ ] Warnings for catch-all try/catch blocks
- [ ] Alerts for untagged/uncategorized tests
- [ ] Lightbulb-style "Fix Suggestions"
- [ ] Missing test methods AND missing test classes

### Test File Cleanup Utility
- [ ] Removes excessive blank lines / normalizes line spacing
- [ ] Normalizes line endings
- [ ] Cleans up web-pasted code (double spacing etc.)
- [ ] Context menu: "Clean Test Formatting"
- [ ] Optional: Auto-format on save

---

## 🧠 PHASE 2: Intelligence & Coverage (v2.0 – v2.5)
**Goal: Add diagnostics, AI-powered summaries, and test quality layers**

- [ ] Code coverage integration (.coverage parsing + highlighting)
- [ ] Test Coverage Analyzer results in tool window
- [ ] Visualizer: tested/untested public methods
- [ ] Flaky test detection (multiple reruns)
- [ ] Failure summarization via GPT (opt-in)
- [ ] "Why did this fail?" trace AI
- [ ] Smart rerun suggestions
- [ ] GC/memory insights per test (optional)

---

## 🔬 PHASE 2.5: Debugger Visualizer Lite
**Goal: Enhance test-debug experience with rich live data**

- [ ] Inline Variable Visualizer
- [ ] Highlight changed values during debug
- [ ] Preview previous values (simple history)
- [ ] Smart object expansion
- [ ] Conditional branch hits/shown inline ("Hit"/"Skipped")
- [ ] Color-coded highlights: exceptions/nulls/warnings

### Nice-to-Haves:
- [ ] Expression tracking across frames
- [ ] Minimal inline graph view (collections, trees)
- [ ] Breakpoint Analyzer (predict if hit/missed)

---

## 🔄 PHASE 3: Integration & Extensibility (v3.0 – v3.5)
**Goal: Play well with external tools, services, and team workflows**

- [ ] Plugin system for analyzers: Stryker, BenchmarkDotNet, etc.
- [ ] GitHub PR test summary (markdown or .trx visual)
- [ ] VS Code test JSON compatibility
- [ ] Azure DevOps result integration
- [ ] Export results to Markdown, HTML, or PDF
- [ ] Internal API for external test providers

---

## ✨ PHASE 3.5+: Legendary Stretch Features
**Goal: Make the tool unforgettable**

- [ ] Context-aware quick actions (e.g., failed test → last screenshot)
- [ ] Find tests that touch a specific method
- [ ] Test repetition insights: detect slowdowns, leaks
- [ ] Virtual test suites: user-created dynamic groups
- [ ] Test Data Sniffing: refactor magic strings/test cases
- [ ] Idle-time suggestions: rerun failures when idle
- [ ] Visual diff of input/output for data-driven tests

---

## 💖 PHASE 4: Soul & Delight
**Goal: Make it iconic, memorable, and joyful to use**

### Sensory Feedback
- [ ] Subtle sound cues on pass/fail (optional)
- [ ] Audio/vibration on long test run complete

### Smart Delight
- [ ] "Zen Mode" during test runs (calming animation or glow)
- [ ] ASCII animations (e.g., rocket launches for 100% pass)
- [ ] Debugger Cat mascot with commentary

### AI Partnering
- [ ] GPT prompt: "Suggest assertions for this test"
- [ ] GPT prompt: "Suggest edge cases for this method"
- [ ] GPT prompt: "Why is this test flaky?"

### Developer Love
- [ ] Gamified feedback: test streaks, coverage boosts
- [ ] Shareable badges: test refactor wins, test suite mastery
- [ ] Retro mode: toggle 90s debug visuals for fun

---

## 👥 FUTURE: Team & Collaboration (Nice-to-Have)
**Goal: Help teams align on test activity**

- [ ] View tests recently run by teammates (repo-based)
- [ ] Sync pinned/failing test sets across devs
- [ ] Shared test group links / "Test playlists"
- [ ] Live view: what others are running

---

## 📅 Release Milestones
*Coming soon*

---

# Pain Points Addressed & Feature Mapping

## Common Developer Pain Points

| Pain Point                          | Addressed In Phase(s) | Feature(s)                                  |
|--------------------------------------|-----------------------|---------------------------------------------|
| Slow test discovery                  | 0, 1                  | Fast, incremental parsing, auto-refresh     |
| Poor feedback loop                   | 1, 2                  | Live test feedback, persistent results      |
| Difficult navigation                 | 0, 1                  | Source-to-test switching, deep linking      |
| Test maintenance                     | 1.5, 2                | Analyzer, refactoring tools                 |
| Debugging/diagnostics                | 2.5                   | Debugger visualizer, one-click debug        |
| Test data management                 | 1.5, 3+               | Data helpers, test data sniffing            |
| CI/CD discrepancies                  | 3                     | CI/CD test result sync, Azure DevOps        |
| Coverage visibility                  | 2                     | Inline coverage, coverage analyzer          |
| Test code quality                    | 1.5                   | Linting, fix suggestions                    |
| Workflow flexibility                 | 1, 3                  | Custom shortcuts, plugin system             |

---

## Example User Stories by Feature

Here are user stories for each major feature, using the recommended template:  
**As a [type of user], I want to [do something] so that [benefit]**.

### 1. **Fast, Incremental Test Discovery**
- **Feature**: Use file watchers and incremental parsing to instantly detect new/changed tests.
- **Benefit**: Reduces lag in large solutions, keeps test list always up-to-date.

### 2. **Live Test Feedback**
- **Feature**: Auto-run impacted tests on file save (like NCrunch or VS Live Unit Testing).
- **Benefit**: Immediate feedback loop, faster bug detection.

### 3. **Enhanced Navigation**
- **Feature**: "Find all tests for this method/class" and "Find all code covered by this test".
- **Feature**: Deep linking between test and code, even for parameterized/data-driven tests.
- **Benefit**: Saves time, improves code/test traceability.

### 4. **Test Refactoring Tools**
- **Feature**: Safe rename/move refactoring for tests, with auto-update of references.
- **Feature**: Detect and flag orphaned or duplicate tests.
- **Benefit**: Easier test maintenance, reduces technical debt.

### 5. **Integrated Debugging**
- **Feature**: One-click "Debug failed test" with context (variables, stack trace).
- **Benefit**: Faster diagnosis of test failures.

### 6. **Test Data Helpers**
- **Feature**: Snippets or templates for common test data/mocking patterns.
- **Feature**: Integration with popular mocking libraries (Moq, NSubstitute).
- **Benefit**: Reduces boilerplate, encourages good practices.

### 7. **CI/CD Test Result Sync**
- **Feature**: Compare local and CI test results, highlight discrepancies.
- **Benefit**: Reduces "it works on my machine" issues.

### 8. **Granular Coverage Visualization**
- **Feature**: Inline code coverage display in editor (e.g., colored gutter markers).
- **Benefit**: Makes coverage actionable and visible during coding.

### 9. **Test Code Quality Analysis**
- **Feature**: Linting and suggestions for test code (naming, structure, best practices).
- **Benefit**: Encourages maintainable, high-quality tests.

### User-Customizable Shortcuts and Views
- *As a developer, I want to customize shortcuts and the layout of my test panel so that my workflow matches my personal preferences.*

---

## Feedback Loops & Quality Commitment

- **Feedback Loops:** After each phase, we’ll gather user feedback via GitHub issues, surveys, and telemetry to inform the next phase.
- **Accessibility & Performance:** These are core priorities at every phase, ensuring everyone can use TestNavigatorPlus efficiently.
- **Delight Features:** We’ll introduce small, delightful touches early and often, such as Zen Mode or subtle sound cues, to make testing enjoyable.

---

## Summary Table: Features and Addressed Pain Points

| Feature Area                 | Suggested Addition                                  | Pain Point Addressed                |
|------------------------------|----------------------------------------------------|-------------------------------------|
| Test Discovery               | Fast, incremental parsing, auto-refresh            | Slow/laggy test discovery           |
| Feedback Loop                | Live test feedback on save, persistent results     | Poor feedback loop                  |
| Navigation                   | Deep linking, source-to-test switching             | Difficult navigation                |
| Refactoring                  | Safe rename/move, orphan detection                 | Test maintenance                    |
| Debugging                    | One-click debug failed test, visualizer            | Debugging/diagnostics               |
| Test Data                    | Mocking/data helpers, snippets                     | Test data management                |
| CI/CD Integration            | Local/CI test result comparison                    | CI/CD discrepancies                 |
| Coverage                     | Inline coverage visualization, analyzer            | Coverage visibility                 |
| Code Quality                 | Test linting/suggestions, analyzer                 | Test code quality                   |
| Customization                | Shortcuts, plugin system, panel layout options     | Workflow flexibility                |

---

## Built with care, for devs who test with precision.

### *TestNavigatorPlus: From Outstanding to Iconic.*

---

**Ready to collaborate or suggest features? Open an issue or join the discussion!**
