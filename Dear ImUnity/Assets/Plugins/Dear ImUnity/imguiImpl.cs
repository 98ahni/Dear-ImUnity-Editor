using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Dear
{
    #region Enums
    public enum ImGuiWindowFlags
    {
        None = 0,
        NoTitleBar = 1 << 0,   // Disable title-bar
        NoResize = 1 << 1,   // Disable user resizing with the lower-right grip
        NoMove = 1 << 2,   // Disable user moving the window
        NoScrollbar = 1 << 3,   // Disable scrollbars (window can still scroll with mouse or programmatically)
        NoScrollWithMouse = 1 << 4,   // Disable user vertically scrolling with mouse wheel. On child window, mouse wheel will be forwarded to the parent unless NoScrollbar is also set.
        NoCollapse = 1 << 5,   // Disable user collapsing window by double-clicking on it. Also referred to as Window Menu Button (e.g. within a docking node).
        AlwaysAutoResize = 1 << 6,   // Resize every window to its content every frame
        NoBackground = 1 << 7,   // Disable drawing background color (WindowBg, etc.) and outside border. Similar as using SetNextWindowBgAlpha(0.0f).
        NoSavedSettings = 1 << 8,   // Never load/save settings in .ini file
        NoMouseInputs = 1 << 9,   // Disable catching mouse, hovering test with pass through.
        MenuBar = 1 << 10,  // Has a menu-bar
        HorizontalScrollbar = 1 << 11,  // Allow horizontal scrollbar to appear (off by default). You may use SetNextWindowContentSize(ImVec2(width,0.0f)); prior to calling Begin() to specify width. Read code in imgui.demo in the "Horizontal Scrolling" section.
        NoFocusOnAppearing = 1 << 12,  // Disable taking focus when transitioning from hidden to visible state
        NoBringToFrontOnFocus = 1 << 13,  // Disable bringing window to front when taking focus (e.g. clicking on it or programmatically giving it focus)
        AlwaysVerticalScrollbar = 1 << 14,  // Always show vertical scrollbar (even if ContentSize.y < Size.y)
        AlwaysHorizontalScrollbar = 1 << 15,  // Always show horizontal scrollbar (even if ContentSize.x < Size.x)
        AlwaysUseWindowPadding = 1 << 16,  // Ensure child windows without border uses style.WindowPadding (ignored by default for non-bordered child windows, because more convenient)
        NoNavInputs = 1 << 18,  // No gamepad/keyboard navigation within the window
        NoNavFocus = 1 << 19,  // No focusing toward this window with gamepad/keyboard navigation (e.g. skipped by CTRL+TAB)
        UnsavedDocument = 1 << 20,  // Display a dot next to the title. When used in a tab/docking context, tab is selected when clicking the X + closure is not assumed (will wait for user to stop submitting the tab). Otherwise closure is assumed when pressing the X, so if you keep submitting the tab may reappear at end of tab bar.
        NoNav = ImGuiWindowFlags.NoNavInputs | ImGuiWindowFlags.NoNavFocus,
        NoDecoration = ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoCollapse,
        NoInputs = ImGuiWindowFlags.NoMouseInputs | ImGuiWindowFlags.NoNavInputs | ImGuiWindowFlags.NoNavFocus,

        // [Internal]
        NavFlattened = 1 << 23,  // [BETA] On child window: allow gamepad/keyboard navigation to cross over parent border to this child or between sibling child windows.
        ChildWindow = 1 << 24,  // Don't use! For internal use by BeginChild()
        Tooltip = 1 << 25,  // Don't use! For internal use by BeginTooltip()
        Popup = 1 << 26,  // Don't use! For internal use by BeginPopup()
        Modal = 1 << 27,  // Don't use! For internal use by BeginPopupModal()
        ChildMenu = 1 << 28,  // Don't use! For internal use by BeginMenu()
    }

    public enum ImGuiInputTextFlags
    {
        None = 0,
        CharsDecimal = 1 << 0,   // Allow 0123456789.+-*/
        CharsHexadecimal = 1 << 1,   // Allow 0123456789ABCDEFabcdef
        CharsUppercase = 1 << 2,   // Turn a..z into A..Z
        CharsNoBlank = 1 << 3,   // Filter out spaces, tabs
        AutoSelectAll = 1 << 4,   // Select entire text when first taking mouse focus
        EnterReturnsTrue = 1 << 5,   // Return 'true' when Enter is pressed (as opposed to every time the value was modified). Consider looking at the IsItemDeactivatedAfterEdit() function.
        CallbackCompletion = 1 << 6,   // Callback on pressing TAB (for completion handling)
        CallbackHistory = 1 << 7,   // Callback on pressing Up/Down arrows (for history handling)
        CallbackAlways = 1 << 8,   // Callback on each iteration. User code may query cursor position, modify text buffer.
        CallbackCharFilter = 1 << 9,   // Callback on character inputs to replace or discard them. Modify 'EventChar' to replace or discard, or return 1 in callback to discard.
        AllowTabInput = 1 << 10,  // Pressing TAB input a '\t' character into the text field
        CtrlEnterForNewLine = 1 << 11,  // In multi-line mode, unfocus with Enter, add new line with Ctrl+Enter (default is opposite: unfocus with Ctrl+Enter, add line with Enter).
        NoHorizontalScroll = 1 << 12,  // Disable following the cursor horizontally
        AlwaysOverwrite = 1 << 13,  // Overwrite mode
        ReadOnly = 1 << 14,  // Read-only mode
        Password = 1 << 15,  // Password mode, display all characters as '*'
        NoUndoRedo = 1 << 16,  // Disable undo/redo. Note that input text owns the text data while active, if you want to provide your own undo/redo stack you need e.g. to call ClearActiveID().
        CharsScientific = 1 << 17,  // Allow 0123456789.+-*/eE (Scientific notation input)
        CallbackResize = 1 << 18,  // Callback on buffer capacity changes request (beyond 'buf.size' parameter value), allowing the string to grow. Notify when the string wants to be resized (for string types which hold a cache of their Size). You will be provided a new BufSize in the callback and NEED to honor it. (see misc/cpp/imgui.stdlib.h for an example of using this)
        CallbackEdit = 1 << 19,  // Callback on any edit (note that InputText() already returns true on edit, the callback is useful mainly to manipulate the underlying buffer while focus is active)
        EscapeClearsAll = 1 << 20,  // Escape key clears content if not empty, and deactivate otherwise (contrast to default behavior of Escape to revert)

        // Obsolete names
        //AlwaysInsertMode  = ImGuiInputTextFlags.AlwaysOverwrite   // [renamed in 1.82] name was not matching behavior
    }

    public enum ImGuiTreeNodeFlags
    {
        None = 0,
        Selected = 1 << 0,   // Draw as selected
        Framed = 1 << 1,   // Draw frame with background (e.g. for CollapsingHeader)
        AllowOverlap = 1 << 2,   // Hit testing to allow subsequent widgets to overlap this one
        NoTreePushOnOpen = 1 << 3,   // Don't do a TreePush() when open (e.g. for CollapsingHeader) 
        NoAutoOpenOnLog = 1 << 4,   // Don't automatically and temporarily open node when Logging is active (by default logging will automatically open tree nodes)
        DefaultOpen = 1 << 5,   // Default node to be open
        OpenOnDoubleClick = 1 << 6,   // Need double-click to open node
        OpenOnArrow = 1 << 7,   // Only open when clicking on the arrow part. If ImGuiTreeNodeFlags.OpenOnDoubleClick is also set, single-click arrow or double-click all box to open.
        Leaf = 1 << 8,   // No collapsing, no arrow (use as a convenience for leaf nodes).
        Bullet = 1 << 9,   // Display a bullet instead of arrow. IMPORTANT: node can still be marked open/close if you don't set the .Leaf flag!
        FramePadding = 1 << 10,  // Use FramePadding (even for an unframed text node) to vertically align text baseline to regular widget height. Equivalent to calling AlignTextToFramePadding().
        SpanAvailWidth = 1 << 11,  // Extend hit box to the right-most edge, even if not framed. This is not the default in order to allow adding other items on the same line. In the future we may refactor the hit system to be front-to-back, allowing natural overlaps and then this can become the default.
        SpanFullWidth = 1 << 12,  // Extend hit box to the left-most and right-most edges (bypass the indented area).
        NavLeftJumpsBackHere = 1 << 13,  // (WIP) Nav: left direction may move to this TreeNode() from any of its child (items submitted between TreeNode and TreePop)
                                         //NoScrollOnOpen     = 1 << 14,  // FIXME: TODO: Disable automatic scroll on TreePop() if node got just open and contents is not visible
        CollapsingHeader = ImGuiTreeNodeFlags.Framed | ImGuiTreeNodeFlags.NoTreePushOnOpen | ImGuiTreeNodeFlags.NoAutoOpenOnLog,

        AllowItemOverlap = ImGuiTreeNodeFlags.AllowOverlap,  // Renamed in 1.89.7
    }

    public enum ImGuiPopupFlags
    {
        None = 0,
        MouseButtonLeft = 0,        // For BeginPopupContext*(): open on Left Mouse release. Guaranteed to always be 
        MouseButtonRight = 1,        // For BeginPopupContext*(): open on Right Mouse release. Guaranteed to always be 
        MouseButtonMiddle = 2,        // For BeginPopupContext*(): open on Middle Mouse release. Guaranteed to always be 
        MouseButtonMask_ = 0x1F,
        MouseButtonDefault_ = 1,
        NoOpenOverExistingPopup = 1 << 5,   // For OpenPopup*(), BeginPopupContext*(): don't open if there's already a popup at the same level of the popup stack
        NoOpenOverItems = 1 << 6,   // For BeginPopupContextWindow(): don't return true when hovering items, only when hovering empty space
        AnyPopupId = 1 << 7,   // For IsPopupOpen(): ignore the ImGuiID parameter and test for any popup.
        AnyPopupLevel = 1 << 8,   // For IsPopupOpen(): search/test at any level of the popup stack (default test in the current level)
        AnyPopup = ImGuiPopupFlags.AnyPopupId | ImGuiPopupFlags.AnyPopupLevel,
    }

    public enum ImGuiSelectableFlags
    {
        None = 0,
        DontClosePopups = 1 << 0,   // Clicking this doesn't close parent popup window
        SpanAllColumns = 1 << 1,   // Selectable frame can span all columns (text will still fit in current column)
        AllowDoubleClick = 1 << 2,   // Generate press events on double clicks too
        Disabled = 1 << 3,   // Cannot be selected, display grayed out text
        AllowOverlap = 1 << 4,   // (WIP) Hit testing to allow subsequent widgets to overlap this one

        AllowItemOverlap = ImGuiSelectableFlags.AllowOverlap,  // Renamed in 1.89.7
    }

    public enum ImGuiComboFlags
    {
        None = 0,
        PopupAlignLeft = 1 << 0,   // Align the popup toward the left by default
        HeightSmall = 1 << 1,   // Max ~4 items visible. Tip: If you want your combo popup to be a specific size you can use SetNextWindowSizeConstraints() prior to calling BeginCombo()
        HeightRegular = 1 << 2,   // Max ~8 items visible (default)
        HeightLarge = 1 << 3,   // Max ~20 items visible
        HeightLargest = 1 << 4,   // As many fitting items as possible
        NoArrowButton = 1 << 5,   // Display on the preview box without the square arrow button
        NoPreview = 1 << 6,   // Display only a square arrow button
        HeightMask_ = ImGuiComboFlags.HeightSmall | ImGuiComboFlags.HeightRegular | ImGuiComboFlags.HeightLarge | ImGuiComboFlags.HeightLargest,
    }

    public enum ImGuiTabBarFlags
    {
        None = 0,
        Reorderable = 1 << 0,   // Allow manually dragging tabs to re-order them + New tabs are appended at the end of list
        AutoSelectNewTabs = 1 << 1,   // Automatically select new tabs when they appear
        TabListPopupButton = 1 << 2,   // Disable buttons to open the tab list popup
        NoCloseWithMiddleMouseButton = 1 << 3,   // Disable behavior of closing tabs (that are submitted with p.open !
        NoTabListScrollingButtons = 1 << 4,   // Disable scrolling buttons (apply when fitting policy is ImGuiTabBarFlags.FittingPolicyScroll)
        NoTooltip = 1 << 5,   // Disable tooltips when hovering a tab
        FittingPolicyResizeDown = 1 << 6,   // Resize tabs when they don't fit
        FittingPolicyScroll = 1 << 7,   // Add scroll buttons when tabs don't fit
        FittingPolicyMask_ = ImGuiTabBarFlags.FittingPolicyResizeDown | ImGuiTabBarFlags.FittingPolicyScroll,
        FittingPolicyDefault_ = ImGuiTabBarFlags.FittingPolicyResizeDown,
    }

    public enum ImGuiTabItemFlags
    {
        None = 0,
        UnsavedDocument = 1 << 0,   // Display a dot next to the title + tab is selected when clicking the X + closure is not assumed (will wait for user to stop submitting the tab). Otherwise closure is assumed when pressing the X, so if you keep submitting the tab may reappear at end of tab bar.
        SetSelected = 1 << 1,   // Trigger flag to programmatically make the tab selected when calling BeginTabItem()
        NoCloseWithMiddleMouseButton = 1 << 2,   // Disable behavior of closing tabs (that are submitted with p.open !
        NoPushId = 1 << 3,   // Don't call PushID(tab->ID)/PopID() on BeginTabItem()/EndTabItem()
        NoTooltip = 1 << 4,   // Disable tooltip for the given tab
        NoReorder = 1 << 5,   // Disable reordering this tab or having another tab cross over this tab
        Leading = 1 << 6,   // Enforce the tab position to the left of the tab bar (after the tab list popup button)
        Trailing = 1 << 7,   // Enforce the tab position to the right of the tab bar (before the scrolling buttons)
    }

    public enum ImGuiTableFlags
    {
        // Features
        None = 0,
        Resizable = 1 << 0,   // Enable resizing columns.
        Reorderable = 1 << 1,   // Enable reordering columns in header row (need calling TableSetupColumn() + TableHeadersRow() to display headers)
        Hideable = 1 << 2,   // Enable hiding/disabling columns in context menu.
        Sortable = 1 << 3,   // Enable sorting. Call TableGetSortSpecs() to obtain sort specs. Also see ImGuiTableFlags.SortMulti and ImGuiTableFlags.SortTristate.
        NoSavedSettings = 1 << 4,   // Disable persisting columns order, width and sort settings in the .ini file.
        ContextMenuInBody = 1 << 5,   // Right-click on columns body/contents will display table context menu. By default it is available in TableHeadersRow().
                                      // Decorations
        RowBg = 1 << 6,   // Set each RowBg color with ImGuiCol.TableRowBg or ImGuiCol.TableRowBgAlt (equivalent of calling TableSetBgColor with ImGuiTableBgFlags.RowBg0 on each row manually)
        BordersInnerH = 1 << 7,   // Draw horizontal borders between rows.
        BordersOuterH = 1 << 8,   // Draw horizontal borders at the top and bottom.
        BordersInnerV = 1 << 9,   // Draw vertical borders between columns.
        BordersOuterV = 1 << 10,  // Draw vertical borders on the left and right sides.
        BordersH = ImGuiTableFlags.BordersInnerH | ImGuiTableFlags.BordersOuterH, // Draw horizontal borders.
        BordersV = ImGuiTableFlags.BordersInnerV | ImGuiTableFlags.BordersOuterV, // Draw vertical borders.
        BordersInner = ImGuiTableFlags.BordersInnerV | ImGuiTableFlags.BordersInnerH, // Draw inner borders.
        BordersOuter = ImGuiTableFlags.BordersOuterV | ImGuiTableFlags.BordersOuterH, // Draw outer borders.
        Borders = ImGuiTableFlags.BordersInner | ImGuiTableFlags.BordersOuter,   // Draw all borders.
        NoBordersInBody = 1 << 11,  // [ALPHA] Disable vertical borders in columns Body (borders will always appear in Headers). -> May move to style
        NoBordersInBodyUntilResize = 1 << 12,  // [ALPHA] Disable vertical borders in columns Body until hovered for resize (borders will always appear in Headers). -> May move to style
                                               // Sizing Policy (read above for defaults)
        SizingFixedFit = 1 << 13,  // Columns default to .WidthFixed or .WidthAuto (if resizable or not resizable), matching contents width.
        SizingFixedSame = 2 << 13,  // Columns default to .WidthFixed or .WidthAuto (if resizable or not resizable), matching the maximum contents width of all columns. Implicitly enable ImGuiTableFlags.NoKeepColumnsVisible.
        SizingStretchProp = 3 << 13,  // Columns default to .WidthStretch with default weights proportional to each columns contents widths.
        SizingStretchSame = 4 << 13,  // Columns default to .WidthStretch with default weights all equal, unless overridden by TableSetupColumn().
                                      // Sizing Extra Options
        NoHostExtendX = 1 << 16,  // Make outer width auto-fit to columns, overriding outer.size.x value. Only available when ScrollX/ScrollY are disabled and Stretch columns are not used.
        NoHostExtendY = 1 << 17,  // Make outer height stop exactly at outer.size.y (prevent auto-extending table past the limit). Only available when ScrollX/ScrollY are disabled. Data below the limit will be clipped and not visible.
        NoKeepColumnsVisible = 1 << 18,  // Disable keeping column always minimally visible when ScrollX is off and table gets too small. Not recommended if columns are resizable.
        PreciseWidths = 1 << 19,  // Disable distributing remainder width to stretched columns (width allocation on a 100-wide table with 3 columns: Without this flag: 33,33,34. With this flag: 33,33,33). With larger number of columns, resizing will appear to be less smooth.
                                  // Clipping
        NoClip = 1 << 20,  // Disable clipping rectangle for every individual columns (reduce draw command count, items will be able to overflow into other columns). Generally incompatible with TableSetupScrollFreeze().
                           // Padding
        PadOuterX = 1 << 21,  // Default if BordersOuterV is on. Enable outermost padding. Generally desirable if you have headers.
        NoPadOuterX = 1 << 22,  // Default if BordersOuterV is off. Disable outermost padding.
        NoPadInnerX = 1 << 23,  // Disable inner padding between columns (double inner padding if BordersOuterV is on, single inner padding if BordersOuterV is off).
                                // Scrolling
        ScrollX = 1 << 24,  // Enable horizontal scrolling. Require 'outer.size' parameter of BeginTable() to specify the container size. Changes default sizing policy. Because this creates a child window, ScrollY is currently generally recommended when using ScrollX.
        ScrollY = 1 << 25,  // Enable vertical scrolling. Require 'outer.size' parameter of BeginTable() to specify the container size.
                            // Sorting
        SortMulti = 1 << 26,  // Hold shift when clicking headers to sort on multiple column. TableGetSortSpecs() may return specs where (SpecsCount > 1).
        SortTristate = 1 << 27,  // Allow no sorting, disable default sorting. TableGetSortSpecs() may return specs where (SpecsCount 

        // [Internal] Combinations and masks
        SizingMask_ = ImGuiTableFlags.SizingFixedFit | ImGuiTableFlags.SizingFixedSame | ImGuiTableFlags.SizingStretchProp | ImGuiTableFlags.SizingStretchSame,
    }

    public enum ImGuiTableColumnFlags
    {
        // Input configuration flags
        None = 0,
        Disabled = 1 << 0,   // Overriding/master disable flag: hide column, won't show in context menu (unlike calling TableSetColumnEnabled() which manipulates the user accessible state)
        DefaultHide = 1 << 1,   // Default as a hidden/disabled column.
        DefaultSort = 1 << 2,   // Default as a sorting column.
        WidthStretch = 1 << 3,   // Column will stretch. Preferable with horizontal scrolling disabled (default if table sizing policy is .SizingStretchSame or .SizingStretchProp).
        WidthFixed = 1 << 4,   // Column will not stretch. Preferable with horizontal scrolling enabled (default if table sizing policy is .SizingFixedFit and table is resizable).
        NoResize = 1 << 5,   // Disable manual resizing.
        NoReorder = 1 << 6,   // Disable manual reordering this column, this will also prevent other columns from crossing over this column.
        NoHide = 1 << 7,   // Disable ability to hide/disable this column.
        NoClip = 1 << 8,   // Disable clipping for this column (all NoClip columns will render in a same draw command).
        NoSort = 1 << 9,   // Disable ability to sort on this field (even if ImGuiTableFlags.Sortable is set on the table).
        NoSortAscending = 1 << 10,  // Disable ability to sort in the ascending direction.
        NoSortDescending = 1 << 11,  // Disable ability to sort in the descending direction.
        NoHeaderLabel = 1 << 12,  // TableHeadersRow() will not submit label for this column. Convenient for some small columns. Name will still appear in context menu.
        NoHeaderWidth = 1 << 13,  // Disable header text width contribution to automatic column width.
        PreferSortAscending = 1 << 14,  // Make the initial sort direction Ascending when first sorting on this column (default).
        PreferSortDescending = 1 << 15,  // Make the initial sort direction Descending when first sorting on this column.
        IndentEnable = 1 << 16,  // Use current Indent value when entering cell (default for column 0).
        IndentDisable = 1 << 17,  // Ignore current Indent value when entering cell (default for columns > 0). Indentation changes .within. the cell will still be honored.

        // Output status flags, read-only via TableGetColumnFlags()
        IsEnabled = 1 << 24,  // Status: is enabled 
        IsVisible = 1 << 25,  // Status: is visible 
        IsSorted = 1 << 26,  // Status: is currently part of the sort specs
        IsHovered = 1 << 27,  // Status: is hovered by mouse

        // [Internal] Combinations and masks
        WidthMask_ = ImGuiTableColumnFlags.WidthStretch | ImGuiTableColumnFlags.WidthFixed,
        IndentMask_ = ImGuiTableColumnFlags.IndentEnable | ImGuiTableColumnFlags.IndentDisable,
        StatusMask_ = ImGuiTableColumnFlags.IsEnabled | ImGuiTableColumnFlags.IsVisible | ImGuiTableColumnFlags.IsSorted | ImGuiTableColumnFlags.IsHovered,
        NoDirectResize_ = 1 << 30,  // [Internal] Disable user resizing this column directly (it may however we resized indirectly from its left edge)
    }

    public enum ImGuiTableRowFlags
    {
        None = 0,
        Headers = 1 << 0,   // Identify header row (set default background color + width of its contents accounted differently for auto column width)
    }

    public enum ImGuiTableBgTarget
    {
        None = 0,
        RowBg0 = 1,        // Set row background color 0 (generally used for background, automatically set when ImGuiTableFlags.RowBg is used)
        RowBg1 = 2,        // Set row background color 1 (generally used for selection marking)
        CellBg = 3,        // Set cell background color (top-most color)
    }

    public enum ImGuiFocusedFlags
    {
        None = 0,
        ChildWindows = 1 << 0,   // Return true if any children of the window is focused
        RootWindow = 1 << 1,   // Test from root window (top most parent of the current hierarchy)
        AnyWindow = 1 << 2,   // Return true if any window is focused. Important: If you are trying to tell how to dispatch your low-level inputs, do NOT use this. Use 'io.WantCaptureMouse' instead! Please read the FAQ!
        NoPopupHierarchy = 1 << 3,   // Do not consider popup hierarchy (do not treat popup emitter as parent of popup) (when used with .ChildWindows or .RootWindow)
                                     //DockHierarchy               = 1 << 4,   // Consider docking hierarchy (treat dockspace host as parent of docked window) (when used with .ChildWindows or .RootWindow)
        RootAndChildWindows = ImGuiFocusedFlags.RootWindow | ImGuiFocusedFlags.ChildWindows,
    }

    public enum ImGuiHoveredFlags
    {
        None = 0,        // Return true if directly over the item/window, not obstructed by another window, not obstructed by an active popup or modal blocking inputs under them.
        ChildWindows = 1 << 0,   // IsWindowHovered() only: Return true if any children of the window is hovered
        RootWindow = 1 << 1,   // IsWindowHovered() only: Test from root window (top most parent of the current hierarchy)
        AnyWindow = 1 << 2,   // IsWindowHovered() only: Return true if any window is hovered
        NoPopupHierarchy = 1 << 3,   // IsWindowHovered() only: Do not consider popup hierarchy (do not treat popup emitter as parent of popup) (when used with .ChildWindows or .RootWindow)
                                     //DockHierarchy               = 1 << 4,   // IsWindowHovered() only: Consider docking hierarchy (treat dockspace host as parent of docked window) (when used with .ChildWindows or .RootWindow)
        AllowWhenBlockedByPopup = 1 << 5,   // Return true even if a popup window is normally blocking access to this item/window
                                            //AllowWhenBlockedByModal     = 1 << 6,   // Return true even if a modal popup window is normally blocking access to this item/window. FIXME-TODO: Unavailable yet.
        AllowWhenBlockedByActiveItem = 1 << 7,   // Return true even if an active item is blocking access to this item/window. Useful for Drag and Drop patterns.
        AllowWhenOverlappedByItem = 1 << 8,   // IsItemHovered() only: Return true even if the item uses AllowOverlap mode and is overlapped by another hoverable item.
        AllowWhenOverlappedByWindow = 1 << 9,   // IsItemHovered() only: Return true even if the position is obstructed or overlapped by another window.
        AllowWhenDisabled = 1 << 10,  // IsItemHovered() only: Return true even if the item is disabled
        NoNavOverride = 1 << 11,  // IsItemHovered() only: Disable using gamepad/keyboard navigation state when active, always query mouse
        AllowWhenOverlapped = ImGuiHoveredFlags.AllowWhenOverlappedByItem | ImGuiHoveredFlags.AllowWhenOverlappedByWindow,
        RectOnly = ImGuiHoveredFlags.AllowWhenBlockedByPopup | ImGuiHoveredFlags.AllowWhenBlockedByActiveItem | ImGuiHoveredFlags.AllowWhenOverlapped,
        RootAndChildWindows = ImGuiHoveredFlags.RootWindow | ImGuiHoveredFlags.ChildWindows,

        // Tooltips mode
        // - typically used in IsItemHovered() + SetTooltip() sequence.
        // - this is a shortcut to pull flags from 'style.HoverFlagsForTooltipMouse' or 'style.HoverFlagsForTooltipNav' where you can reconfigure desired behavior.
        //   e.g. 'TooltipHoveredFlagsForMouse' defaults to 'Stationary | DelayShort'.
        // - for frequently actioned or hovered items providing a tooltip, you want may to use ForTooltip (stationary + delay) so the tooltip doesn't show too often.
        // - for items which main purpose is to be hovered, or items with low affordance, or in less consistent apps, prefer no delay or shorter delay.
        ForTooltip = 1 << 12,  // Shortcut for standard flags when using IsItemHovered() + SetTooltip() sequence.

        // (Advanced) Mouse Hovering delays.
        // - generally you can use ForTooltip to use application-standardized flags.
        // - use those if you need specific overrides.
        Stationary = 1 << 13,  // Require mouse to be stationary for style.HoverStationaryDelay (~0.15 sec) .at least one time.. After this, can move on same item/window. Using the stationary test tends to reduces the need for a long delay.
        DelayNone = 1 << 14,  // IsItemHovered() only: Return true immediately (default). As this is the default you generally ignore this.
        DelayShort = 1 << 15,  // IsItemHovered() only: Return true after style.HoverDelayShort elapsed (~0.15 sec) (shared between items) + requires mouse to be stationary for style.HoverStationaryDelay (once per item).
        DelayNormal = 1 << 16,  // IsItemHovered() only: Return true after style.HoverDelayNormal elapsed (~0.40 sec) (shared between items) + requires mouse to be stationary for style.HoverStationaryDelay (once per item).
        NoSharedDelay = 1 << 17,  // IsItemHovered() only: Disable shared delay system where moving from one item to the next keeps the previous timer for a short time (standard for tooltips with long delays)
    }

    public enum ImGuiDragDropFlags
    {
        None = 0,
        // BeginDragDropSource() flags
        SourceNoPreviewTooltip = 1 << 0,   // Disable preview tooltip. By default, a successful call to BeginDragDropSource opens a tooltip so you can display a preview or description of the source contents. This flag disables this behavior.
        SourceNoDisableHover = 1 << 1,   // By default, when dragging we clear data so that IsItemHovered() will return false, to avoid subsequent user code submitting tooltips. This flag disables this behavior so you can still call IsItemHovered() on the source item.
        SourceNoHoldToOpenOthers = 1 << 2,   // Disable the behavior that allows to open tree nodes and collapsing header by holding over them while dragging a source item.
        SourceAllowNullID = 1 << 3,   // Allow items such as Text(), Image() that have no unique identifier to be used as drag source, by manufacturing a temporary identifier based on their window-relative position. This is extremely unusual within the dear imgui ecosystem and so we made it explicit.
        SourceExtern = 1 << 4,   // External source (from outside of dear imgui), won't attempt to read current item/window info. Will always return true. Only one Extern source can be active simultaneously.
        SourceAutoExpirePayload = 1 << 5,   // Automatically expire the payload if the source cease to be submitted (otherwise payloads are persisting while being dragged)
                                            // AcceptDragDropPayload() flags
        AcceptBeforeDelivery = 1 << 10,  // AcceptDragDropPayload() will returns true even before the mouse button is released. You can then call IsDelivery() to test if the payload needs to be delivered.
        AcceptNoDrawDefaultRect = 1 << 11,  // Do not draw the default highlight rectangle when hovering over target.
        AcceptNoPreviewTooltip = 1 << 12,  // Request hiding the BeginDragDropSource tooltip from the BeginDragDropTarget site.
        AcceptPeekOnly = ImGuiDragDropFlags.AcceptBeforeDelivery | ImGuiDragDropFlags.AcceptNoDrawDefaultRect, // For peeking ahead and inspecting the payload before delivery.
    }

    public enum ImGuiDataType
    {
        S8,       // signed char / char (with sensible compilers)
        U8,       // unsigned char
        S16,      // short
        U16,      // unsigned short
        S32,      // int
        U32,      // unsigned int
        S64,      // long long / __int64
        U64,      // unsigned long long / unsigned __int64
        Float,    // float
        Double,   // double
        COUNT
    }

    public enum ImGuiDir
    {
        None = -1,
        Left = 0,
        Right = 1,
        Up = 2,
        Down = 3,
        COUNT
    }

    public enum ImGuiSortDirection
    {
        None = 0,
        Ascending = 1,    // Ascending 
        Descending = 2     // Descending 
    }

    public enum ImGuiKey : int
    {
        // Keyboard
        _None = 0,
        _Tab = 512,             // 
        _LeftArrow,
        _RightArrow,
        _UpArrow,
        _DownArrow,
        _PageUp,
        _PageDown,
        _Home,
        _End,
        _Insert,
        _Delete,
        _Backspace,
        _Space,
        _Enter,
        _Escape,
        _LeftCtrl, _LeftShift, _LeftAlt, _LeftSuper,
        _RightCtrl, _RightShift, _RightAlt, _RightSuper,
        _Menu,
        _0, _1, _2, _3, _4, _5, _6, _7, _8, _9,
        _A, _B, _C, _D, _E, _F, _G, _H, _I, _J,
        _K, _L, _M, _N, _O, _P, _Q, _R, _S, _T,
        _U, _V, _W, _X, _Y, _Z,
        _F1, _F2, _F3, _F4, _F5, _F6,
        _F7, _F8, _F9, _F10, _F11, _F12,
        _Apostrophe,        // '
        _Comma,             // ,
        _Minus,             // -
        _Period,            // .
        _Slash,             // /
        _Semicolon,         // ;
        _Equal,             // =
        _LeftBracket,       // [
        _Backslash,         // \ (this text inhibit multiline comment caused by backslash)
        _RightBracket,      // ]
        _GraveAccent,       // `
        _CapsLock,
        _ScrollLock,
        _NumLock,
        _PrintScreen,
        _Pause,
        _Keypad0, _Keypad1, _Keypad2, _Keypad3, _Keypad4,
        _Keypad5, _Keypad6, _Keypad7, _Keypad8, _Keypad9,
        _KeypadDecimal,
        _KeypadDivide,
        _KeypadMultiply,
        _KeypadSubtract,
        _KeypadAdd,
        _KeypadEnter,
        _KeypadEqual,

        // Gamepad (some of those are analog values, 0.0f to 1.0f)                          // NAVIGATION ACTION
        // (download controller mapping PNG/PSD at http://dearimgui.com/controls_sheets)
        _GamepadStart,          // Menu (Xbox)      + (Switch)   Start/Options (PS)
        _GamepadBack,           // View (Xbox)      - (Switch)   Share (PS)
        _GamepadFaceLeft,       // X (Xbox)         Y (Switch)   Square (PS)        // Tap: Toggle Menu. Hold: Windowing mode (Focus/Move/Resize windows)
        _GamepadFaceRight,      // B (Xbox)         A (Switch)   Circle (PS)        // Cancel / Close / Exit
        _GamepadFaceUp,         // Y (Xbox)         X (Switch)   Triangle (PS)      // Text Input / On-screen Keyboard
        _GamepadFaceDown,       // A (Xbox)         B (Switch)   Cross (PS)         // Activate / Open / Toggle / Tweak
        _GamepadDpadLeft,       // D-pad Left                                       // Move / Tweak / Resize Window (in Windowing mode)
        _GamepadDpadRight,      // D-pad Right                                      // Move / Tweak / Resize Window (in Windowing mode)
        _GamepadDpadUp,         // D-pad Up                                         // Move / Tweak / Resize Window (in Windowing mode)
        _GamepadDpadDown,       // D-pad Down                                       // Move / Tweak / Resize Window (in Windowing mode)
        _GamepadL1,             // L Bumper (Xbox)  L (Switch)   L1 (PS)            // Tweak Slower / Focus Previous (in Windowing mode)
        _GamepadR1,             // R Bumper (Xbox)  R (Switch)   R1 (PS)            // Tweak Faster / Focus Next (in Windowing mode)
        _GamepadL2,             // L Trig. (Xbox)   ZL (Switch)  L2 (PS) [Analog]
        _GamepadR2,             // R Trig. (Xbox)   ZR (Switch)  R2 (PS) [Analog]
        _GamepadL3,             // L Stick (Xbox)   L3 (Switch)  L3 (PS)
        _GamepadR3,             // R Stick (Xbox)   R3 (Switch)  R3 (PS)
        _GamepadLStickLeft,     // [Analog]                                         // Move Window (in Windowing mode)
        _GamepadLStickRight,    // [Analog]                                         // Move Window (in Windowing mode)
        _GamepadLStickUp,       // [Analog]                                         // Move Window (in Windowing mode)
        _GamepadLStickDown,     // [Analog]                                         // Move Window (in Windowing mode)
        _GamepadRStickLeft,     // [Analog]
        _GamepadRStickRight,    // [Analog]
        _GamepadRStickUp,       // [Analog]
        _GamepadRStickDown,     // [Analog]

        // Aliases: Mouse Buttons (auto-submitted from AddMouseButtonEvent() calls)
        // - This is mirroring the data also written to io.MouseDown[], io.MouseWheel, in a format allowing them to be accessed via standard key API.
        _MouseLeft, _MouseRight, _MouseMiddle, _MouseX1, _MouseX2, _MouseWheelX, _MouseWheelY,

        // [Internal] Reserved for mod storage
        _ReservedForModCtrl, _ReservedForModShift, _ReservedForModAlt, _ReservedForModSuper,
        _COUNT,

        // Keyboard Modifiers (explicitly submitted by backend via AddKeyEvent() calls)
        // - This is mirroring the data also written to io.KeyCtrl, io.KeyShift, io.KeyAlt, io.KeySuper, in a format allowing
        //   them to be accessed via standard key API, allowing calls such as IsKeyPressed(), IsKeyReleased(), querying duration etc.
        // - Code polling every key (e.g. an interface to detect a key press for input mapping) might want to ignore those
        //   and prefer using the real keys (e.g. _LeftCtrl, _RightCtrl instead of ImGuiMod_Ctrl).
        // - In theory the value of keyboard modifiers should be roughly equivalent to a logical or of the equivalent left/right keys.
        //   In practice: it's complicated; mods are often provided from different sources. Keyboard layout, IME, sticky keys and
        //   backends tend to interfere and break that equivalence. The safer decision is to relay that ambiguity down to the end-user...
        ImGuiMod_None = 0,
        ImGuiMod_Ctrl = 1 << 12, // Ctrl
        ImGuiMod_Shift = 1 << 13, // Shift
        ImGuiMod_Alt = 1 << 14, // Option/Menu
        ImGuiMod_Super = 1 << 15, // Cmd/Super/Windows
        ImGuiMod_Shortcut = 1 << 11, // Alias for Ctrl (non-macOS) .or. Super (macOS).
        ImGuiMod_Mask_ = 0xF800,  // 5-bits

        // [Internal] Prior to 1.87 we required user to fill io.KeysDown[512] using their own native index + the io.KeyMap[] array.
        // We are ditching this method but keeping a legacy path for user code doing e.g. IsKeyPressed(MY_NATIVE_KEY_CODE)
        // If you need to iterate all keys (for e.g. an input mapper) you may use _NamedKey_BEGIN.._NamedKey_END.
        _NamedKey_BEGIN = 512,
        _NamedKey_END = ImGuiKey._COUNT,
        _NamedKey_COUNT = ImGuiKey._NamedKey_END - ImGuiKey._NamedKey_BEGIN,
        _KeysData_SIZE = ImGuiKey._COUNT,           // Size of KeysData[]: hold legacy 0..512 keycodes + named keys
        _KeysData_OFFSET = 0,                        // Accesses to io.KeysData[] must use (key - ImGuiKey.KeysData.OFFSET) index.

        _ModCtrl = ImGuiMod_Ctrl, ImGuiKey_ModShift,
        _KeyPadEnter = ImGuiKey._KeypadEnter,    // Renamed in 1.87
    }

    public enum ImGuiNavInput
    {
        _Activate, _Cancel, _Input, _Menu, _DpadLeft, _DpadRight, _DpadUp, _DpadDown,
        _LStickLeft, _LStickRight, _LStickUp, _LStickDown, _FocusPrev, _FocusNext, _TweakSlow, _TweakFast,
        _COUNT,
    }

    public enum ImGuiConfigFlags
    {
        None = 0,
        NavEnableKeyboard = 1 << 0,   // Master keyboard navigation enable flag. Enable full Tabbing + directional arrows + space/enter to activate.
        NavEnableGamepad = 1 << 1,   // Master gamepad navigation enable flag. Backend also needs to set ImGuiBackendFlags.HasGamepad.
        NavEnableSetMousePos = 1 << 2,   // Instruct navigation to move the mouse cursor. May be useful on TV/console systems where moving a virtual mouse is awkward. Will update io.MousePos and set io.WantSetMousePos
        NavNoCaptureKeyboard = 1 << 3,   // Instruct navigation to not set the io.WantCaptureKeyboard flag when io.NavActive is set.
        NoMouse = 1 << 4,   // Instruct imgui to clear mouse position/buttons in NewFrame(). This allows ignoring the mouse information set by the backend.
        NoMouseCursorChange = 1 << 5,   // Instruct backend to not alter mouse cursor shape and visibility. Use if the backend cursor changes are interfering with yours and you don't want to use SetMouseCursor() to change mouse cursor. You may want to honor requests from imgui by reading GetMouseCursor() yourself instead.

        // User storage (to allow your backend/engine to communicate to code that may be shared between multiple projects. Those flags are NOT used by core Dear ImGui)
        IsSRGB = 1 << 20,  // Application is SRGB-aware.
        IsTouchScreen = 1 << 21,  // Application is using a touch screen instead of a mouse.
    }

    public enum ImGuiBackendFlags
    {
        None = 0,
        HasGamepad = 1 << 0,   // Backend Platform supports gamepad and currently has one connected.
        HasMouseCursors = 1 << 1,   // Backend Platform supports honoring GetMouseCursor() value to change the OS cursor shape.
        HasSetMousePos = 1 << 2,   // Backend Platform supports io.WantSetMousePos requests to reposition the OS mouse position (only used if ImGuiConfigFlags.NavEnableSetMousePos is set).
        RendererHasVtxOffset = 1 << 3,   // Backend Renderer supports ImDrawCmd::VtxOffset. This enables output of large meshes (64K+ vertices) while still using 16-bit indices.
    }

    public enum ImGuiCol
    {
        Text,
        TextDisabled,
        WindowBg,              // Background of normal windows
        ChildBg,               // Background of child windows
        PopupBg,               // Background of popups, menus, tooltips windows
        Border,
        BorderShadow,
        FrameBg,               // Background of checkbox, radio button, plot, slider, text input
        FrameBgHovered,
        FrameBgActive,
        TitleBg,
        TitleBgActive,
        TitleBgCollapsed,
        MenuBarBg,
        ScrollbarBg,
        ScrollbarGrab,
        ScrollbarGrabHovered,
        ScrollbarGrabActive,
        CheckMark,
        SliderGrab,
        SliderGrabActive,
        Button,
        ButtonHovered,
        ButtonActive,
        Header,                // Header* colors are used for CollapsingHeader, TreeNode, Selectable, MenuItem
        HeaderHovered,
        HeaderActive,
        Separator,
        SeparatorHovered,
        SeparatorActive,
        ResizeGrip,            // Resize grip in lower-right and lower-left corners of windows.
        ResizeGripHovered,
        ResizeGripActive,
        Tab,                   // TabItem in a TabBar
        TabHovered,
        TabActive,
        TabUnfocused,
        TabUnfocusedActive,
        PlotLines,
        PlotLinesHovered,
        PlotHistogram,
        PlotHistogramHovered,
        TableHeaderBg,         // Table header background
        TableBorderStrong,     // Table outer and header borders (prefer using Alpha=1.0 here)
        TableBorderLight,      // Table inner borders (prefer using Alpha=1.0 here)
        TableRowBg,            // Table row background (even rows)
        TableRowBgAlt,         // Table row background (odd rows)
        TextSelectedBg,
        DragDropTarget,        // Rectangle highlighting a drop target
        NavHighlight,          // Gamepad/keyboard: current highlighted item
        NavWindowingHighlight, // Highlight window when using CTRL+TAB
        NavWindowingDimBg,     // Darken/colorize entire screen behind the CTRL+TAB window list, when active
        ModalWindowDimBg,      // Darken/colorize entire screen behind a modal window, when one is active
        COUNT
    }

    public enum ImGuiStyleVar
    {
        // Enum name --------------------- // Member in ImGuiStyle structure (see ImGuiStyle for descriptions)
        Alpha,               // float     Alpha
        DisabledAlpha,       // float     DisabledAlpha
        WindowPadding,       // ImVec2    WindowPadding
        WindowRounding,      // float     WindowRounding
        WindowBorderSize,    // float     WindowBorderSize
        WindowMinSize,       // ImVec2    WindowMinSize
        WindowTitleAlign,    // ImVec2    WindowTitleAlign
        ChildRounding,       // float     ChildRounding
        ChildBorderSize,     // float     ChildBorderSize
        PopupRounding,       // float     PopupRounding
        PopupBorderSize,     // float     PopupBorderSize
        FramePadding,        // ImVec2    FramePadding
        FrameRounding,       // float     FrameRounding
        FrameBorderSize,     // float     FrameBorderSize
        ItemSpacing,         // ImVec2    ItemSpacing
        ItemInnerSpacing,    // ImVec2    ItemInnerSpacing
        IndentSpacing,       // float     IndentSpacing
        CellPadding,         // ImVec2    CellPadding
        ScrollbarSize,       // float     ScrollbarSize
        ScrollbarRounding,   // float     ScrollbarRounding
        GrabMinSize,         // float     GrabMinSize
        GrabRounding,        // float     GrabRounding
        TabRounding,         // float     TabRounding
        ButtonTextAlign,     // ImVec2    ButtonTextAlign
        SelectableTextAlign, // ImVec2    SelectableTextAlign
        SeparatorTextBorderSize,// float  SeparatorTextBorderSize
        SeparatorTextAlign,  // ImVec2    SeparatorTextAlign
        SeparatorTextPadding,// ImVec2    SeparatorTextPadding
        COUNT
    }

    public enum ImGuiButtonFlags
    {
        None = 0,
        MouseButtonLeft = 1 << 0,   // React on left mouse button (default)
        MouseButtonRight = 1 << 1,   // React on right mouse button
        MouseButtonMiddle = 1 << 2,   // React on center mouse button

        // [Internal]
        MouseButtonMask_ = ImGuiButtonFlags.MouseButtonLeft | ImGuiButtonFlags.MouseButtonRight | ImGuiButtonFlags.MouseButtonMiddle,
        MouseButtonDefault_ = ImGuiButtonFlags.MouseButtonLeft,
    }

    public enum ImGuiColorEditFlags
    {
        None = 0,
        NoAlpha = 1 << 1,   //              // ColorEdit, ColorPicker, ColorButton: ignore Alpha component (will only read 3 components from the input pointer).
        NoPicker = 1 << 2,   //              // ColorEdit: disable picker when clicking on color square.
        NoOptions = 1 << 3,   //              // ColorEdit: disable toggling options menu when right-clicking on inputs/small preview.
        NoSmallPreview = 1 << 4,   //              // ColorEdit, ColorPicker: disable color square preview next to the inputs. (e.g. to show only the inputs)
        NoInputs = 1 << 5,   //              // ColorEdit, ColorPicker: disable inputs sliders/text widgets (e.g. to show only the small preview color square).
        NoTooltip = 1 << 6,   //              // ColorEdit, ColorPicker, ColorButton: disable tooltip when hovering the preview.
        NoLabel = 1 << 7,   //              // ColorEdit, ColorPicker: disable display of inline text label (the label is still forwarded to the tooltip and picker).
        NoSidePreview = 1 << 8,   //              // ColorPicker: disable bigger color preview on right side of the picker, use small color square preview instead.
        NoDragDrop = 1 << 9,   //              // ColorEdit: disable drag and drop target. ColorButton: disable drag and drop source.
        NoBorder = 1 << 10,  //              // ColorButton: disable border (which is enforced by default)

        // User Options (right-click on widget to change some of them).
        AlphaBar = 1 << 16,  //              // ColorEdit, ColorPicker: show vertical alpha bar/gradient in picker.
        AlphaPreview = 1 << 17,  //              // ColorEdit, ColorPicker, ColorButton: display preview as a transparent color over a checkerboard, instead of opaque.
        AlphaPreviewHalf = 1 << 18,  //              // ColorEdit, ColorPicker, ColorButton: display half opaque / half checkerboard, instead of opaque.
        HDR = 1 << 19,  //              // (WIP) ColorEdit: Currently only disable 0.0f..1.0f limits in RGBA edition (note: you probably want to use ImGuiColorEditFlags.Float flag as well).
        DisplayRGB = 1 << 20,  // [Display]    // ColorEdit: override .display. type among RGB/HSV/Hex. ColorPicker: select any combination using one or more of RGB/HSV/Hex.
        DisplayHSV = 1 << 21,  // [Display]    // "
        DisplayHex = 1 << 22,  // [Display]    // "
        Uint8 = 1 << 23,  // [DataType]   // ColorEdit, ColorPicker, ColorButton: .display. values formatted as 0..255.
        Float = 1 << 24,  // [DataType]   // ColorEdit, ColorPicker, ColorButton: .display. values formatted as 0.0f..1.0f floats instead of 0..255 integers. No round-trip of value via integers.
        PickerHueBar = 1 << 25,  // [Picker]     // ColorPicker: bar for Hue, rectangle for Sat/Value.
        PickerHueWheel = 1 << 26,  // [Picker]     // ColorPicker: wheel for Hue, triangle for Sat/Value.
        InputRGB = 1 << 27,  // [Input]      // ColorEdit, ColorPicker: input and output data in RGB format.
        InputHSV = 1 << 28,  // [Input]      // ColorEdit, ColorPicker: input and output data in HSV format.

        // Defaults Options. You can set application defaults using SetColorEditOptions(). The intent is that you probably don't want to
        // override them in most of your calls. Let the user choose via the option menu and/or call SetColorEditOptions() once during startup.
        DefaultOptions_ = ImGuiColorEditFlags.Uint8 | ImGuiColorEditFlags.DisplayRGB | ImGuiColorEditFlags.InputRGB | ImGuiColorEditFlags.PickerHueBar,

        // [Internal] Masks
        DisplayMask_ = ImGuiColorEditFlags.DisplayRGB | ImGuiColorEditFlags.DisplayHSV | ImGuiColorEditFlags.DisplayHex,
        DataTypeMask_ = ImGuiColorEditFlags.Uint8 | ImGuiColorEditFlags.Float,
        PickerMask_ = ImGuiColorEditFlags.PickerHueWheel | ImGuiColorEditFlags.PickerHueBar,
        InputMask_ = ImGuiColorEditFlags.InputRGB | ImGuiColorEditFlags.InputHSV,

        // Obsolete names
        //RGB = ImGuiColorEditFlags.DisplayRGB, ImGuiColorEditFlags.HSV 
    }

    public enum ImGuiSliderFlags
    {
        None = 0,
        AlwaysClamp = 1 << 4,       // Clamp value to min/max bounds when input manually with CTRL+Click. By default CTRL+Click allows going out of bounds.
        Logarithmic = 1 << 5,       // Make the widget logarithmic (linear otherwise). Consider using ImGuiSliderFlags.NoRoundToFormat with this if using a format-string with small amount of digits.
        NoRoundToFormat = 1 << 6,       // Disable rounding underlying value to match precision of the display format string (e.g. %.3f values are rounded to those 3 digits)
        NoInput = 1 << 7,       // Disable CTRL+Click or Enter key allowing to input text directly into the widget
        InvalidMask_ = 0x7000000F,   // [Internal] We treat using those bits as being potentially a 'float power' argument from the previous API that has got miscast to this enum, and will trigger an assert if needed.

        // Obsolete names
        //ClampOnInput = ImGuiSliderFlags.AlwaysClamp, // [renamed in 1.79]
    }

    public enum ImGuiMouseButton
    {
        Left = 0,
        Right = 1,
        Middle = 2,
        COUNT = 5
    }

    public enum ImGuiMouseCursor
    {
        None = -1,
        Arrow = 0,
        TextInput,         // When hovering over InputText, etc.
        ResizeAll,         // (Unused by Dear ImGui functions)
        ResizeNS,          // When hovering over a horizontal border
        ResizeEW,          // When hovering over a vertical border or a column
        ResizeNESW,        // When hovering over the bottom-left corner of a window
        ResizeNWSE,        // When hovering over the bottom-right corner of a window
        Hand,              // (Unused by Dear ImGui functions. Use for e.g. hyperlinks)
        NotAllowed,        // When hovering something with disallowed interaction. Usually a crossed circle.
        COUNT
    }

    public enum ImGuiMouseSource : int
    {
        _Mouse = 0,         // Input is coming from an actual mouse.
        _TouchScreen,       // Input is coming from a touch screen (no hovering prior to initial press, less precise initial press aiming, dual-axis wheeling possible).
        _Pen,               // Input is coming from a pressure/magnetic pen (often used in conjunction with high-sampling rates).
        _COUNT
    }

    public enum ImGuiCond
    {
        None = 0,        // No condition (always set the variable), same as .Always
        Always = 1 << 0,   // No condition (always set the variable), same as .None
        Once = 1 << 1,   // Set the variable once per runtime session (only the first call will succeed)
        FirstUseEver = 1 << 2,   // Set the variable if the object/window has no persistently saved data (no entry in .ini file)
        Appearing = 1 << 3,   // Set the variable if the object/window is appearing after being hidden/inactive (or the first time)
    }

    public enum ImDrawFlags
    {
        None = 0,
        Closed = 1 << 0, // PathStroke(), AddPolyline(): specify that shape should be closed (Important: this is always 
        RoundCornersTopLeft = 1 << 4, // AddRect(), AddRectFilled(), PathRect(): enable rounding top-left corner only (when rounding > 0.0f, we default to all corners). Was 0x01.
        RoundCornersTopRight = 1 << 5, // AddRect(), AddRectFilled(), PathRect(): enable rounding top-right corner only (when rounding > 0.0f, we default to all corners). Was 0x02.
        RoundCornersBottomLeft = 1 << 6, // AddRect(), AddRectFilled(), PathRect(): enable rounding bottom-left corner only (when rounding > 0.0f, we default to all corners). Was 0x04.
        RoundCornersBottomRight = 1 << 7, // AddRect(), AddRectFilled(), PathRect(): enable rounding bottom-right corner only (when rounding > 0.0f, we default to all corners). Wax 0x08.
        RoundCornersNone = 1 << 8, // AddRect(), AddRectFilled(), PathRect(): disable rounding on all corners (when rounding > 0.0f). This is NOT zero, NOT an implicit flag!
        RoundCornersTop = ImDrawFlags.RoundCornersTopLeft | ImDrawFlags.RoundCornersTopRight,
        RoundCornersBottom = ImDrawFlags.RoundCornersBottomLeft | ImDrawFlags.RoundCornersBottomRight,
        RoundCornersLeft = ImDrawFlags.RoundCornersBottomLeft | ImDrawFlags.RoundCornersTopLeft,
        RoundCornersRight = ImDrawFlags.RoundCornersBottomRight | ImDrawFlags.RoundCornersTopRight,
        RoundCornersAll = ImDrawFlags.RoundCornersTopLeft | ImDrawFlags.RoundCornersTopRight | ImDrawFlags.RoundCornersBottomLeft | ImDrawFlags.RoundCornersBottomRight,
        RoundCornersDefault_ = ImDrawFlags.RoundCornersAll, // Default to ALL corners if none of the .RoundCornersXX flags are specified.
        RoundCornersMask_ = ImDrawFlags.RoundCornersAll | ImDrawFlags.RoundCornersNone,
    }

    public enum ImDrawListFlags
    {
        None = 0,
        AntiAliasedLines = 1 << 0,  // Enable anti-aliased lines/borders (*2 the number of triangles for 1.0f wide line or lines thin enough to be drawn using textures, otherwise *3 the number of triangles)
        AntiAliasedLinesUseTex = 1 << 1,  // Enable anti-aliased lines/borders using textures when possible. Require backend to render with bilinear filtering (NOT point/nearest filtering).
        AntiAliasedFill = 1 << 2,  // Enable anti-aliased edge around filled shapes (rounded rectangles, circles).
        AllowVtxOffset = 1 << 3,  // Can emit 'VtxOffset > 0' to allow large meshes. Set when 'ImGuiBackendFlags.RendererHasVtxOffset' is enabled.
    }

    public enum ImFontAtlasFlags
    {
        None = 0,
        NoPowerOfTwoHeight = 1 << 0,   // Don't round the height to next power of two
        NoMouseCursors = 1 << 1,   // Don't build software mouse cursors into the atlas (save a little texture memory)
        NoBakedLines = 1 << 2,   // Don't build thick line textures into the atlas (save a little texture memory, allow support for point/nearest filtering). The AntiAliasedLinesUseTex features uses them, otherwise they will be rendered using polygons (more expensive for CPU/GPU).
    }

    public enum ImGuiViewportFlags
    {
        None = 0,
        IsPlatformWindow = 1 << 0,   // Represent a Platform Window
        IsPlatformMonitor = 1 << 1,   // Represent a Platform Monitor (unused yet)
        OwnedByApp = 1 << 2,   // Platform Window: is created/managed by the application (rather than a dear imgui backend)
    }

    public enum ImDrawCornerFlags
    {
        None = ImDrawFlags.RoundCornersNone,         // Was 
        TopLeft = ImDrawFlags.RoundCornersTopLeft,      // Was 
        TopRight = ImDrawFlags.RoundCornersTopRight,     // Was 
        BotLeft = ImDrawFlags.RoundCornersBottomLeft,   // Was 
        BotRight = ImDrawFlags.RoundCornersBottomRight,  // Was 
        All = ImDrawFlags.RoundCornersAll,          // Was 
        Top = ImDrawCornerFlags.TopLeft | ImDrawCornerFlags.TopRight,
        Bot = ImDrawCornerFlags.BotLeft | ImDrawCornerFlags.BotRight,
        Left = ImDrawCornerFlags.TopLeft | ImDrawCornerFlags.BotLeft,
        Right = ImDrawCornerFlags.TopRight | ImDrawCornerFlags.BotRight,
    }

    public enum ImGuiModFlags
    {
        ImGuiModFlags_None = 0,
        ImGuiModFlags_Ctrl = ImGuiKey.ImGuiMod_Ctrl,
        ImGuiModFlags_Shift = ImGuiKey.ImGuiMod_Shift,
        ImGuiModFlags_Alt = ImGuiKey.ImGuiMod_Alt,
        ImGuiModFlags_Super = ImGuiKey.ImGuiMod_Super
    }

    #endregion // Enums

    #region Functions
    public static partial class ImGui
	{

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern IntPtr ImGui_GetIO300();

	/// <summary><code>IMGUI_API ImGuiIO&      GetIO();                                    </code>
		/// access the IO structure (mouse/keyboard/gamepad inputs, time, various configuration options/flags) </summary>
	public static ImGuiIO GetIO()
	{
		return new ImGuiIO(ImGui_GetIO300());
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern IntPtr ImGui_GetStyle301();

	/// <summary><code>IMGUI_API ImGuiStyle&   GetStyle();                                 </code>
		/// access the Style structure (colors, sizes). Always use PushStyleCol(), PushStyleVar() to modify style mid-frame! </summary>
	public static ImGuiStyle GetStyle()
	{
		return new ImGuiStyle(ImGui_GetStyle301());
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_NewFrame302();

	/// <summary><code>IMGUI_API void          NewFrame();                                 </code>
		/// start a new Dear ImGui frame, you can submit any command from this point until Render()/EndFrame(). </summary>
	public static void NewFrame()
	{
		ImGui_NewFrame302();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_EndFrame303();

	/// <summary><code>IMGUI_API void          EndFrame();                                 </code>
		/// ends the Dear ImGui frame. automatically called by Render(). If you don't need to render data (skipping rendering) you may call EndFrame() without Render()... but you'll have wasted CPU already! If you don't need to render, better to not create any windows and not call NewFrame() at all! </summary>
	public static void EndFrame()
	{
		ImGui_EndFrame303();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_Render304();

	/// <summary><code>IMGUI_API void          Render();                                   </code>
		/// ends the Dear ImGui frame, finalize the draw data. You can then get call GetDrawData(). </summary>
	public static void Render()
	{
		ImGui_Render304();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern IntPtr ImGui_GetDrawData305();

	/// <summary><code>IMGUI_API ImDrawData*   GetDrawData();                              </code>
		/// valid after Render() and until the next call to NewFrame(). this is what you have to render. </summary>
	public static ImDrawData GetDrawData()
	{
		return new ImDrawData(ImGui_GetDrawData305());
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_ShowDemoWindow308([MarshalAs(UnmanagedType.I1)]out bool POpen);

	/// <summary><code>IMGUI_API void          ShowDemoWindow(bool* p_open = NULL);        </code>
		/// create Demo window. demonstrate most ImGui features. call this to learn about the library! try to make it always available in your application! </summary>
	public static void ShowDemoWindow()
	{
		ImGui_ShowDemoWindow308(out _);
	}

	/// <summary><code>IMGUI_API void          ShowDemoWindow(bool* p_open = NULL);        </code>
		/// create Demo window. demonstrate most ImGui features. call this to learn about the library! try to make it always available in your application! </summary>
	public static void ShowDemoWindow(out bool POpen)
	{
		ImGui_ShowDemoWindow308(out POpen);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_ShowMetricsWindow309([MarshalAs(UnmanagedType.I1)]out bool POpen);

	/// <summary><code>IMGUI_API void          ShowMetricsWindow(bool* p_open = NULL);     </code>
		/// create Metrics/Debugger window. display Dear ImGui internals: windows, draw commands, various internal state, etc. </summary>
	public static void ShowMetricsWindow()
	{
		ImGui_ShowMetricsWindow309(out _);
	}

	/// <summary><code>IMGUI_API void          ShowMetricsWindow(bool* p_open = NULL);     </code>
		/// create Metrics/Debugger window. display Dear ImGui internals: windows, draw commands, various internal state, etc. </summary>
	public static void ShowMetricsWindow(out bool POpen)
	{
		ImGui_ShowMetricsWindow309(out POpen);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_ShowDebugLogWindow310([MarshalAs(UnmanagedType.I1)]out bool POpen);

	/// <summary><code>IMGUI_API void          ShowDebugLogWindow(bool* p_open = NULL);    </code>
		/// create Debug Log window. display a simplified log of important dear imgui events. </summary>
	public static void ShowDebugLogWindow()
	{
		ImGui_ShowDebugLogWindow310(out _);
	}

	/// <summary><code>IMGUI_API void          ShowDebugLogWindow(bool* p_open = NULL);    </code>
		/// create Debug Log window. display a simplified log of important dear imgui events. </summary>
	public static void ShowDebugLogWindow(out bool POpen)
	{
		ImGui_ShowDebugLogWindow310(out POpen);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_ShowStackToolWindow311([MarshalAs(UnmanagedType.I1)]out bool POpen);

	/// <summary><code>IMGUI_API void          ShowStackToolWindow(bool* p_open = NULL);   </code>
		/// create Stack Tool window. hover items with mouse to query information about the source of their unique ID. </summary>
	public static void ShowStackToolWindow()
	{
		ImGui_ShowStackToolWindow311(out _);
	}

	/// <summary><code>IMGUI_API void          ShowStackToolWindow(bool* p_open = NULL);   </code>
		/// create Stack Tool window. hover items with mouse to query information about the source of their unique ID. </summary>
	public static void ShowStackToolWindow(out bool POpen)
	{
		ImGui_ShowStackToolWindow311(out POpen);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_ShowAboutWindow312([MarshalAs(UnmanagedType.I1)]out bool POpen);

	/// <summary><code>IMGUI_API void          ShowAboutWindow(bool* p_open = NULL);       </code>
		/// create About window. display Dear ImGui version, credits and build/system information. </summary>
	public static void ShowAboutWindow()
	{
		ImGui_ShowAboutWindow312(out _);
	}

	/// <summary><code>IMGUI_API void          ShowAboutWindow(bool* p_open = NULL);       </code>
		/// create About window. display Dear ImGui version, credits and build/system information. </summary>
	public static void ShowAboutWindow(out bool POpen)
	{
		ImGui_ShowAboutWindow312(out POpen);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_ShowStyleEditor313(IntPtr Ref);

	/// <summary><code>IMGUI_API void          ShowStyleEditor(ImGuiStyle* ref = NULL);    </code>
		/// add style editor block (not a window). you can pass in a reference ImGuiStyle structure to compare to, revert to and save to (else it uses the default style) </summary>
	public static void ShowStyleEditor()
	{
		ImGui_ShowStyleEditor313(IntPtr.Zero);
	}

	/// <summary><code>IMGUI_API void          ShowStyleEditor(ImGuiStyle* ref = NULL);    </code>
		/// add style editor block (not a window). you can pass in a reference ImGuiStyle structure to compare to, revert to and save to (else it uses the default style) </summary>
	public static void ShowStyleEditor(ImGuiStyle Ref)
	{
		ImGui_ShowStyleEditor313(Ref.AsPtr);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_ShowStyleSelector314([MarshalAs(UnmanagedType.LPStr)]string Label);

	/// <summary><code>IMGUI_API bool          ShowStyleSelector(const char* label);       </code>
		/// add style selector block (not a window), essentially a combo listing the default styles. </summary>
	public static bool ShowStyleSelector(string Label)
	{
		return ImGui_ShowStyleSelector314(Label);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_ShowFontSelector315([MarshalAs(UnmanagedType.LPStr)]string Label);

	/// <summary><code>IMGUI_API void          ShowFontSelector(const char* label);        </code>
		/// add font selector block (not a window), essentially a combo listing the loaded fonts. </summary>
	public static void ShowFontSelector(string Label)
	{
		ImGui_ShowFontSelector315(Label);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_ShowUserGuide316();

	/// <summary><code>IMGUI_API void          ShowUserGuide();                            </code>
		/// add basic help/info block (not a window): how to manipulate ImGui as an end-user (mouse/keyboard controls). </summary>
	public static void ShowUserGuide()
	{
		ImGui_ShowUserGuide316();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.LPStr)]
	private static extern string ImGui_GetVersion317();

	/// <summary><code>IMGUI_API const char*   GetVersion();                               </code>
		/// get the compiled version string e.g. "1.80 WIP" (essentially the value for IMGUI_VERSION from the compiled version of imgui.cpp) </summary>
	public static string GetVersion()
	{
		return ImGui_GetVersion317();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_StyleColorsDark320(IntPtr Dst);

	/// <summary><code>IMGUI_API void          StyleColorsDark(ImGuiStyle* dst = NULL);    </code>
		/// new, recommended style (default) </summary>
	public static void StyleColorsDark()
	{
		ImGui_StyleColorsDark320(IntPtr.Zero);
	}

	/// <summary><code>IMGUI_API void          StyleColorsDark(ImGuiStyle* dst = NULL);    </code>
		/// new, recommended style (default) </summary>
	public static void StyleColorsDark(ImGuiStyle Dst)
	{
		ImGui_StyleColorsDark320(Dst.AsPtr);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_StyleColorsLight321(IntPtr Dst);

	/// <summary><code>IMGUI_API void          StyleColorsLight(ImGuiStyle* dst = NULL);   </code>
		/// best used with borders and a custom, thicker font </summary>
	public static void StyleColorsLight()
	{
		ImGui_StyleColorsLight321(IntPtr.Zero);
	}

	/// <summary><code>IMGUI_API void          StyleColorsLight(ImGuiStyle* dst = NULL);   </code>
		/// best used with borders and a custom, thicker font </summary>
	public static void StyleColorsLight(ImGuiStyle Dst)
	{
		ImGui_StyleColorsLight321(Dst.AsPtr);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_StyleColorsClassic322(IntPtr Dst);

	/// <summary><code>IMGUI_API void          StyleColorsClassic(ImGuiStyle* dst = NULL); </code>
		/// classic imgui style </summary>
	public static void StyleColorsClassic()
	{
		ImGui_StyleColorsClassic322(IntPtr.Zero);
	}

	/// <summary><code>IMGUI_API void          StyleColorsClassic(ImGuiStyle* dst = NULL); </code>
		/// classic imgui style </summary>
	public static void StyleColorsClassic(ImGuiStyle Dst)
	{
		ImGui_StyleColorsClassic322(Dst.AsPtr);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_Begin336([MarshalAs(UnmanagedType.LPStr)]string Name, [MarshalAs(UnmanagedType.I1)]out bool POpen, ImGuiWindowFlags Flags);

	/// <summary><code>IMGUI_API bool          Begin(const char* name, bool* p_open = NULL, ImGuiWindowFlags flags = 0);</code>
		///    IMGUI_API bool          Begin(const char* name, bool* p_open = NULL, ImGuiWindowFlags flags = 0); </summary>
	public static bool Begin(string Name)
	{
		return ImGui_Begin336(Name, out _, (ImGuiWindowFlags)0);
	}

	/// <summary><code>IMGUI_API bool          Begin(const char* name, bool* p_open = NULL, ImGuiWindowFlags flags = 0);</code>
		///    IMGUI_API bool          Begin(const char* name, bool* p_open = NULL, ImGuiWindowFlags flags = 0); </summary>
	public static bool Begin(string Name, out bool POpen)
	{
		return ImGui_Begin336(Name, out POpen, (ImGuiWindowFlags)0);
	}

	/// <summary><code>IMGUI_API bool          Begin(const char* name, bool* p_open = NULL, ImGuiWindowFlags flags = 0);</code>
		///    IMGUI_API bool          Begin(const char* name, bool* p_open = NULL, ImGuiWindowFlags flags = 0); </summary>
	public static bool Begin(string Name, out bool POpen, ImGuiWindowFlags Flags)
	{
		return ImGui_Begin336(Name, out POpen, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_End337();

	/// <summary><code>IMGUI_API void          End();</code>
		///    IMGUI_API void          End(); </summary>
	public static void End()
	{
		ImGui_End337();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_BeginChild347([MarshalAs(UnmanagedType.LPStr)]string StrId, out  Vector2 Size, [MarshalAs(UnmanagedType.I1)]bool Border, ImGuiWindowFlags Flags);

	/// <summary><code>IMGUI_API bool          BeginChild(const char* str_id, const ImVec2& size = ImVec2(0, 0), bool border = false, ImGuiWindowFlags flags = 0);</code>
		///    IMGUI_API bool          BeginChild(const char* str_id, const ImVec2& size = ImVec2(0, 0), bool border = false, ImGuiWindowFlags flags = 0); </summary>
	public static bool BeginChild(string StrId)
	{
		 Vector2 param1 = new  Vector2 (0,  0);
		return ImGui_BeginChild347(StrId, out param1, false, (ImGuiWindowFlags)0);
	}

	/// <summary><code>IMGUI_API bool          BeginChild(const char* str_id, const ImVec2& size = ImVec2(0, 0), bool border = false, ImGuiWindowFlags flags = 0);</code>
		///    IMGUI_API bool          BeginChild(const char* str_id, const ImVec2& size = ImVec2(0, 0), bool border = false, ImGuiWindowFlags flags = 0); </summary>
	public static bool BeginChild(string StrId, out  Vector2 Size)
	{
		return ImGui_BeginChild347(StrId, out Size, false, (ImGuiWindowFlags)0);
	}

	/// <summary><code>IMGUI_API bool          BeginChild(const char* str_id, const ImVec2& size = ImVec2(0, 0), bool border = false, ImGuiWindowFlags flags = 0);</code>
		///    IMGUI_API bool          BeginChild(const char* str_id, const ImVec2& size = ImVec2(0, 0), bool border = false, ImGuiWindowFlags flags = 0); </summary>
	public static bool BeginChild(string StrId, out  Vector2 Size, bool Border)
	{
		return ImGui_BeginChild347(StrId, out Size, Border, (ImGuiWindowFlags)0);
	}

	/// <summary><code>IMGUI_API bool          BeginChild(const char* str_id, const ImVec2& size = ImVec2(0, 0), bool border = false, ImGuiWindowFlags flags = 0);</code>
		///    IMGUI_API bool          BeginChild(const char* str_id, const ImVec2& size = ImVec2(0, 0), bool border = false, ImGuiWindowFlags flags = 0); </summary>
	public static bool BeginChild(string StrId, out  Vector2 Size, bool Border, ImGuiWindowFlags Flags)
	{
		return ImGui_BeginChild347(StrId, out Size, Border, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_BeginChild348(ImGuiID Id, out  Vector2 Size, [MarshalAs(UnmanagedType.I1)]bool Border, ImGuiWindowFlags Flags);

	/// <summary><code>IMGUI_API bool          BeginChild(ImGuiID id, const ImVec2& size = ImVec2(0, 0), bool border = false, ImGuiWindowFlags flags = 0);</code>
		///    IMGUI_API bool          BeginChild(ImGuiID id, const ImVec2& size = ImVec2(0, 0), bool border = false, ImGuiWindowFlags flags = 0); </summary>
	public static bool BeginChild(ImGuiID Id)
	{
		 Vector2 param1 = new  Vector2 (0,  0);
		return ImGui_BeginChild348(Id, out param1, false, (ImGuiWindowFlags)0);
	}

	/// <summary><code>IMGUI_API bool          BeginChild(ImGuiID id, const ImVec2& size = ImVec2(0, 0), bool border = false, ImGuiWindowFlags flags = 0);</code>
		///    IMGUI_API bool          BeginChild(ImGuiID id, const ImVec2& size = ImVec2(0, 0), bool border = false, ImGuiWindowFlags flags = 0); </summary>
	public static bool BeginChild(ImGuiID Id, out  Vector2 Size)
	{
		return ImGui_BeginChild348(Id, out Size, false, (ImGuiWindowFlags)0);
	}

	/// <summary><code>IMGUI_API bool          BeginChild(ImGuiID id, const ImVec2& size = ImVec2(0, 0), bool border = false, ImGuiWindowFlags flags = 0);</code>
		///    IMGUI_API bool          BeginChild(ImGuiID id, const ImVec2& size = ImVec2(0, 0), bool border = false, ImGuiWindowFlags flags = 0); </summary>
	public static bool BeginChild(ImGuiID Id, out  Vector2 Size, bool Border)
	{
		return ImGui_BeginChild348(Id, out Size, Border, (ImGuiWindowFlags)0);
	}

	/// <summary><code>IMGUI_API bool          BeginChild(ImGuiID id, const ImVec2& size = ImVec2(0, 0), bool border = false, ImGuiWindowFlags flags = 0);</code>
		///    IMGUI_API bool          BeginChild(ImGuiID id, const ImVec2& size = ImVec2(0, 0), bool border = false, ImGuiWindowFlags flags = 0); </summary>
	public static bool BeginChild(ImGuiID Id, out  Vector2 Size, bool Border, ImGuiWindowFlags Flags)
	{
		return ImGui_BeginChild348(Id, out Size, Border, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_EndChild349();

	/// <summary><code>IMGUI_API void          EndChild();</code>
		///    IMGUI_API void          EndChild(); </summary>
	public static void EndChild()
	{
		ImGui_EndChild349();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_IsWindowAppearing353();

	/// <summary><code>IMGUI_API bool          IsWindowAppearing();</code>
		///    IMGUI_API bool          IsWindowAppearing(); </summary>
	public static bool IsWindowAppearing()
	{
		return ImGui_IsWindowAppearing353();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_IsWindowCollapsed354();

	/// <summary><code>IMGUI_API bool          IsWindowCollapsed();</code>
		///    IMGUI_API bool          IsWindowCollapsed(); </summary>
	public static bool IsWindowCollapsed()
	{
		return ImGui_IsWindowCollapsed354();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_IsWindowFocused355(ImGuiFocusedFlags Flags);

	/// <summary><code>IMGUI_API bool          IsWindowFocused(ImGuiFocusedFlags flags=0); </code>
		/// is current window focused? or its root/child, depending on flags. see flags for options. </summary>
	public static bool IsWindowFocused()
	{
		return ImGui_IsWindowFocused355((ImGuiFocusedFlags)0);
	}

	/// <summary><code>IMGUI_API bool          IsWindowFocused(ImGuiFocusedFlags flags=0); </code>
		/// is current window focused? or its root/child, depending on flags. see flags for options. </summary>
	public static bool IsWindowFocused(ImGuiFocusedFlags Flags)
	{
		return ImGui_IsWindowFocused355(Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_IsWindowHovered356(ImGuiHoveredFlags Flags);

	/// <summary><code>IMGUI_API bool          IsWindowHovered(ImGuiHoveredFlags flags=0); </code>
		/// is current window hovered (and typically: not blocked by a popup/modal)? see flags for options. NB: If you are trying to check whether your mouse should be dispatched to imgui or to your app, you should use the 'io.WantCaptureMouse' boolean for that! Please read the FAQ! </summary>
	public static bool IsWindowHovered()
	{
		return ImGui_IsWindowHovered356((ImGuiHoveredFlags)0);
	}

	/// <summary><code>IMGUI_API bool          IsWindowHovered(ImGuiHoveredFlags flags=0); </code>
		/// is current window hovered (and typically: not blocked by a popup/modal)? see flags for options. NB: If you are trying to check whether your mouse should be dispatched to imgui or to your app, you should use the 'io.WantCaptureMouse' boolean for that! Please read the FAQ! </summary>
	public static bool IsWindowHovered(ImGuiHoveredFlags Flags)
	{
		return ImGui_IsWindowHovered356(Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern IntPtr ImGui_GetWindowDrawList357();

	/// <summary><code>IMGUI_API ImDrawList*   GetWindowDrawList();                        </code>
		/// get draw list associated to the current window, to append your own drawing primitives </summary>
	public static ImDrawList GetWindowDrawList()
	{
		return new ImDrawList(ImGui_GetWindowDrawList357());
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern Vector2 ImGui_GetWindowPos358();

	/// <summary><code>IMGUI_API ImVec2        GetWindowPos();                             </code>
		/// get current window position in screen space (note: it is unlikely you need to use this. Consider using current layout pos instead, GetScreenCursorPos()) </summary>
	public static Vector2 GetWindowPos()
	{
		return ImGui_GetWindowPos358();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern Vector2 ImGui_GetWindowSize359();

	/// <summary><code>IMGUI_API ImVec2        GetWindowSize();                            </code>
		/// get current window size (note: it is unlikely you need to use this. Consider using GetScreenCursorPos() and e.g. GetContentRegionAvail() instead) </summary>
	public static Vector2 GetWindowSize()
	{
		return ImGui_GetWindowSize359();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGui_GetWindowWidth360();

	/// <summary><code>IMGUI_API float         GetWindowWidth();                           </code>
		/// get current window width (shortcut for GetWindowSize().x) </summary>
	public static float GetWindowWidth()
	{
		return ImGui_GetWindowWidth360();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGui_GetWindowHeight361();

	/// <summary><code>IMGUI_API float         GetWindowHeight();                          </code>
		/// get current window height (shortcut for GetWindowSize().y) </summary>
	public static float GetWindowHeight()
	{
		return ImGui_GetWindowHeight361();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_SetNextWindowPos365(out  Vector2 Pos, ImGuiCond Cond, out  Vector2 Pivot);

	/// <summary><code>IMGUI_API void          SetNextWindowPos(const ImVec2& pos, ImGuiCond cond = 0, const ImVec2& pivot = ImVec2(0, 0)); </code>
		/// set next window position. call before Begin(). use pivot=(0.5f,0.5f) to center on given point, etc. </summary>
	public static void SetNextWindowPos(out  Vector2 Pos)
	{
		 Vector2 param2 = new  Vector2 (0,  0);
		ImGui_SetNextWindowPos365(out Pos, (ImGuiCond)0, out param2);
	}

	/// <summary><code>IMGUI_API void          SetNextWindowPos(const ImVec2& pos, ImGuiCond cond = 0, const ImVec2& pivot = ImVec2(0, 0)); </code>
		/// set next window position. call before Begin(). use pivot=(0.5f,0.5f) to center on given point, etc. </summary>
	public static void SetNextWindowPos(out  Vector2 Pos, ImGuiCond Cond)
	{
		 Vector2 param2 = new  Vector2 (0,  0);
		ImGui_SetNextWindowPos365(out Pos, Cond, out param2);
	}

	/// <summary><code>IMGUI_API void          SetNextWindowPos(const ImVec2& pos, ImGuiCond cond = 0, const ImVec2& pivot = ImVec2(0, 0)); </code>
		/// set next window position. call before Begin(). use pivot=(0.5f,0.5f) to center on given point, etc. </summary>
	public static void SetNextWindowPos(out  Vector2 Pos, ImGuiCond Cond, out  Vector2 Pivot)
	{
		ImGui_SetNextWindowPos365(out Pos, Cond, out Pivot);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_SetNextWindowSize366(out  Vector2 Size, ImGuiCond Cond);

	/// <summary><code>IMGUI_API void          SetNextWindowSize(const ImVec2& size, ImGuiCond cond = 0);                  </code>
		/// set next window size. set axis to 0.0f to force an auto-fit on this axis. call before Begin() </summary>
	public static void SetNextWindowSize(out  Vector2 Size)
	{
		ImGui_SetNextWindowSize366(out Size, (ImGuiCond)0);
	}

	/// <summary><code>IMGUI_API void          SetNextWindowSize(const ImVec2& size, ImGuiCond cond = 0);                  </code>
		/// set next window size. set axis to 0.0f to force an auto-fit on this axis. call before Begin() </summary>
	public static void SetNextWindowSize(out  Vector2 Size, ImGuiCond Cond)
	{
		ImGui_SetNextWindowSize366(out Size, Cond);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_SetNextWindowSizeConstraints367(out  Vector2 SizeMin, out  Vector2 SizeMax, ImGuiSizeCallback CustomCallback, IntPtr CustomCallbackData);

	/// <summary><code>IMGUI_API void          SetNextWindowSizeConstraints(const ImVec2& size_min, const ImVec2& size_max, ImGuiSizeCallback custom_callback = NULL, void* custom_callback_data = NULL); </code>
		/// set next window size limits. use -1,-1 on either X/Y axis to preserve the current size. Sizes will be rounded down. Use callback to apply non-trivial programmatic constraints. </summary>
	public static void SetNextWindowSizeConstraints(out  Vector2 SizeMin, out  Vector2 SizeMax)
	{
		ImGui_SetNextWindowSizeConstraints367(out SizeMin, out SizeMax, default, default);
	}

	/// <summary><code>IMGUI_API void          SetNextWindowSizeConstraints(const ImVec2& size_min, const ImVec2& size_max, ImGuiSizeCallback custom_callback = NULL, void* custom_callback_data = NULL); </code>
		/// set next window size limits. use -1,-1 on either X/Y axis to preserve the current size. Sizes will be rounded down. Use callback to apply non-trivial programmatic constraints. </summary>
	public static void SetNextWindowSizeConstraints(out  Vector2 SizeMin, out  Vector2 SizeMax, ImGuiSizeCallback CustomCallback)
	{
		ImGui_SetNextWindowSizeConstraints367(out SizeMin, out SizeMax, CustomCallback, default);
	}

	/// <summary><code>IMGUI_API void          SetNextWindowSizeConstraints(const ImVec2& size_min, const ImVec2& size_max, ImGuiSizeCallback custom_callback = NULL, void* custom_callback_data = NULL); </code>
		/// set next window size limits. use -1,-1 on either X/Y axis to preserve the current size. Sizes will be rounded down. Use callback to apply non-trivial programmatic constraints. </summary>
	public static void SetNextWindowSizeConstraints(out  Vector2 SizeMin, out  Vector2 SizeMax, ImGuiSizeCallback CustomCallback, IntPtr CustomCallbackData)
	{
		ImGui_SetNextWindowSizeConstraints367(out SizeMin, out SizeMax, CustomCallback, CustomCallbackData);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_SetNextWindowContentSize368(out  Vector2 Size);

	/// <summary><code>IMGUI_API void          SetNextWindowContentSize(const ImVec2& size);                               </code>
		/// set next window content size (~ scrollable client area, which enforce the range of scrollbars). Not including window decorations (title bar, menu bar, etc.) nor WindowPadding. set an axis to 0.0f to leave it automatic. call before Begin() </summary>
	public static void SetNextWindowContentSize(out  Vector2 Size)
	{
		ImGui_SetNextWindowContentSize368(out Size);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_SetNextWindowCollapsed369([MarshalAs(UnmanagedType.I1)]bool Collapsed, ImGuiCond Cond);

	/// <summary><code>IMGUI_API void          SetNextWindowCollapsed(bool collapsed, ImGuiCond cond = 0);                 </code>
		/// set next window collapsed state. call before Begin() </summary>
	public static void SetNextWindowCollapsed(bool Collapsed)
	{
		ImGui_SetNextWindowCollapsed369(Collapsed, (ImGuiCond)0);
	}

	/// <summary><code>IMGUI_API void          SetNextWindowCollapsed(bool collapsed, ImGuiCond cond = 0);                 </code>
		/// set next window collapsed state. call before Begin() </summary>
	public static void SetNextWindowCollapsed(bool Collapsed, ImGuiCond Cond)
	{
		ImGui_SetNextWindowCollapsed369(Collapsed, Cond);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_SetNextWindowFocus370();

	/// <summary><code>IMGUI_API void          SetNextWindowFocus();                                                       </code>
		/// set next window to be focused / top-most. call before Begin() </summary>
	public static void SetNextWindowFocus()
	{
		ImGui_SetNextWindowFocus370();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_SetNextWindowScroll371(out  Vector2 Scroll);

	/// <summary><code>IMGUI_API void          SetNextWindowScroll(const ImVec2& scroll);                                  </code>
		/// set next window scrolling value (use < 0.0f to not affect a given axis). </summary>
	public static void SetNextWindowScroll(out  Vector2 Scroll)
	{
		ImGui_SetNextWindowScroll371(out Scroll);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_SetNextWindowBgAlpha372(float Alpha);

	/// <summary><code>IMGUI_API void          SetNextWindowBgAlpha(float alpha);                                          </code>
		/// set next window background color alpha. helper to easily override the Alpha component of ImGuiCol_WindowBg/ChildBg/PopupBg. you may also use ImGuiWindowFlags_NoBackground. </summary>
	public static void SetNextWindowBgAlpha(float Alpha)
	{
		ImGui_SetNextWindowBgAlpha372(Alpha);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_SetWindowPos373(out  Vector2 Pos, ImGuiCond Cond);

	/// <summary><code>IMGUI_API void          SetWindowPos(const ImVec2& pos, ImGuiCond cond = 0);                        </code>
		/// (not recommended) set current window position - call within Begin()/End(). prefer using SetNextWindowPos(), as this may incur tearing and side-effects. </summary>
	public static void SetWindowPos(out  Vector2 Pos)
	{
		ImGui_SetWindowPos373(out Pos, (ImGuiCond)0);
	}

	/// <summary><code>IMGUI_API void          SetWindowPos(const ImVec2& pos, ImGuiCond cond = 0);                        </code>
		/// (not recommended) set current window position - call within Begin()/End(). prefer using SetNextWindowPos(), as this may incur tearing and side-effects. </summary>
	public static void SetWindowPos(out  Vector2 Pos, ImGuiCond Cond)
	{
		ImGui_SetWindowPos373(out Pos, Cond);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_SetWindowSize374(out  Vector2 Size, ImGuiCond Cond);

	/// <summary><code>IMGUI_API void          SetWindowSize(const ImVec2& size, ImGuiCond cond = 0);                      </code>
		/// (not recommended) set current window size - call within Begin()/End(). set to ImVec2(0, 0) to force an auto-fit. prefer using SetNextWindowSize(), as this may incur tearing and minor side-effects. </summary>
	public static void SetWindowSize(out  Vector2 Size)
	{
		ImGui_SetWindowSize374(out Size, (ImGuiCond)0);
	}

	/// <summary><code>IMGUI_API void          SetWindowSize(const ImVec2& size, ImGuiCond cond = 0);                      </code>
		/// (not recommended) set current window size - call within Begin()/End(). set to ImVec2(0, 0) to force an auto-fit. prefer using SetNextWindowSize(), as this may incur tearing and minor side-effects. </summary>
	public static void SetWindowSize(out  Vector2 Size, ImGuiCond Cond)
	{
		ImGui_SetWindowSize374(out Size, Cond);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_SetWindowCollapsed375([MarshalAs(UnmanagedType.I1)]bool Collapsed, ImGuiCond Cond);

	/// <summary><code>IMGUI_API void          SetWindowCollapsed(bool collapsed, ImGuiCond cond = 0);                     </code>
		/// (not recommended) set current window collapsed state. prefer using SetNextWindowCollapsed(). </summary>
	public static void SetWindowCollapsed(bool Collapsed)
	{
		ImGui_SetWindowCollapsed375(Collapsed, (ImGuiCond)0);
	}

	/// <summary><code>IMGUI_API void          SetWindowCollapsed(bool collapsed, ImGuiCond cond = 0);                     </code>
		/// (not recommended) set current window collapsed state. prefer using SetNextWindowCollapsed(). </summary>
	public static void SetWindowCollapsed(bool Collapsed, ImGuiCond Cond)
	{
		ImGui_SetWindowCollapsed375(Collapsed, Cond);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_SetWindowFocus376();

	/// <summary><code>IMGUI_API void          SetWindowFocus();                                                           </code>
		/// (not recommended) set current window to be focused / top-most. prefer using SetNextWindowFocus(). </summary>
	public static void SetWindowFocus()
	{
		ImGui_SetWindowFocus376();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_SetWindowPos378([MarshalAs(UnmanagedType.LPStr)]string Name, out  Vector2 Pos, ImGuiCond Cond);

	/// <summary><code>IMGUI_API void          SetWindowPos(const char* name, const ImVec2& pos, ImGuiCond cond = 0);      </code>
		/// set named window position. </summary>
	public static void SetWindowPos(string Name, out  Vector2 Pos)
	{
		ImGui_SetWindowPos378(Name, out Pos, (ImGuiCond)0);
	}

	/// <summary><code>IMGUI_API void          SetWindowPos(const char* name, const ImVec2& pos, ImGuiCond cond = 0);      </code>
		/// set named window position. </summary>
	public static void SetWindowPos(string Name, out  Vector2 Pos, ImGuiCond Cond)
	{
		ImGui_SetWindowPos378(Name, out Pos, Cond);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_SetWindowSize379([MarshalAs(UnmanagedType.LPStr)]string Name, out  Vector2 Size, ImGuiCond Cond);

	/// <summary><code>IMGUI_API void          SetWindowSize(const char* name, const ImVec2& size, ImGuiCond cond = 0);    </code>
		/// set named window size. set axis to 0.0f to force an auto-fit on this axis. </summary>
	public static void SetWindowSize(string Name, out  Vector2 Size)
	{
		ImGui_SetWindowSize379(Name, out Size, (ImGuiCond)0);
	}

	/// <summary><code>IMGUI_API void          SetWindowSize(const char* name, const ImVec2& size, ImGuiCond cond = 0);    </code>
		/// set named window size. set axis to 0.0f to force an auto-fit on this axis. </summary>
	public static void SetWindowSize(string Name, out  Vector2 Size, ImGuiCond Cond)
	{
		ImGui_SetWindowSize379(Name, out Size, Cond);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_SetWindowCollapsed380([MarshalAs(UnmanagedType.LPStr)]string Name, [MarshalAs(UnmanagedType.I1)]bool Collapsed, ImGuiCond Cond);

	/// <summary><code>IMGUI_API void          SetWindowCollapsed(const char* name, bool collapsed, ImGuiCond cond = 0);   </code>
		/// set named window collapsed state </summary>
	public static void SetWindowCollapsed(string Name, bool Collapsed)
	{
		ImGui_SetWindowCollapsed380(Name, Collapsed, (ImGuiCond)0);
	}

	/// <summary><code>IMGUI_API void          SetWindowCollapsed(const char* name, bool collapsed, ImGuiCond cond = 0);   </code>
		/// set named window collapsed state </summary>
	public static void SetWindowCollapsed(string Name, bool Collapsed, ImGuiCond Cond)
	{
		ImGui_SetWindowCollapsed380(Name, Collapsed, Cond);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_SetWindowFocus381([MarshalAs(UnmanagedType.LPStr)]string Name);

	/// <summary><code>IMGUI_API void          SetWindowFocus(const char* name);                                           </code>
		/// set named window to be focused / top-most. use NULL to remove focus. </summary>
	public static void SetWindowFocus(string Name)
	{
		ImGui_SetWindowFocus381(Name);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern Vector2 ImGui_GetContentRegionAvail386();

	/// <summary><code>IMGUI_API ImVec2        GetContentRegionAvail();                                        </code>
		/// == GetContentRegionMax() - GetCursorPos() </summary>
	public static Vector2 GetContentRegionAvail()
	{
		return ImGui_GetContentRegionAvail386();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern Vector2 ImGui_GetContentRegionMax387();

	/// <summary><code>IMGUI_API ImVec2        GetContentRegionMax();                                          </code>
		/// current content boundaries (typically window boundaries including scrolling, or current column boundaries), in windows coordinates </summary>
	public static Vector2 GetContentRegionMax()
	{
		return ImGui_GetContentRegionMax387();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern Vector2 ImGui_GetWindowContentRegionMin388();

	/// <summary><code>IMGUI_API ImVec2        GetWindowContentRegionMin();                                    </code>
		/// content boundaries min for the full window (roughly (0,0)-Scroll), in window coordinates </summary>
	public static Vector2 GetWindowContentRegionMin()
	{
		return ImGui_GetWindowContentRegionMin388();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern Vector2 ImGui_GetWindowContentRegionMax389();

	/// <summary><code>IMGUI_API ImVec2        GetWindowContentRegionMax();                                    </code>
		/// content boundaries max for the full window (roughly (0,0)+Size-Scroll) where Size can be overridden with SetNextWindowContentSize(), in window coordinates </summary>
	public static Vector2 GetWindowContentRegionMax()
	{
		return ImGui_GetWindowContentRegionMax389();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGui_GetScrollX394();

	/// <summary><code>IMGUI_API float         GetScrollX();                                                   </code>
		/// get scrolling amount [0 .. GetScrollMaxX()] </summary>
	public static float GetScrollX()
	{
		return ImGui_GetScrollX394();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGui_GetScrollY395();

	/// <summary><code>IMGUI_API float         GetScrollY();                                                   </code>
		/// get scrolling amount [0 .. GetScrollMaxY()] </summary>
	public static float GetScrollY()
	{
		return ImGui_GetScrollY395();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_SetScrollX396(float ScrollX);

	/// <summary><code>IMGUI_API void          SetScrollX(float scroll_x);                                     </code>
		/// set scrolling amount [0 .. GetScrollMaxX()] </summary>
	public static void SetScrollX(float ScrollX)
	{
		ImGui_SetScrollX396(ScrollX);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_SetScrollY397(float ScrollY);

	/// <summary><code>IMGUI_API void          SetScrollY(float scroll_y);                                     </code>
		/// set scrolling amount [0 .. GetScrollMaxY()] </summary>
	public static void SetScrollY(float ScrollY)
	{
		ImGui_SetScrollY397(ScrollY);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGui_GetScrollMaxX398();

	/// <summary><code>IMGUI_API float         GetScrollMaxX();                                                </code>
		/// get maximum scrolling amount ~~ ContentSize.x - WindowSize.x - DecorationsSize.x </summary>
	public static float GetScrollMaxX()
	{
		return ImGui_GetScrollMaxX398();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGui_GetScrollMaxY399();

	/// <summary><code>IMGUI_API float         GetScrollMaxY();                                                </code>
		/// get maximum scrolling amount ~~ ContentSize.y - WindowSize.y - DecorationsSize.y </summary>
	public static float GetScrollMaxY()
	{
		return ImGui_GetScrollMaxY399();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_SetScrollHereX400(float CenterXRatio);

	/// <summary><code>IMGUI_API void          SetScrollHereX(float center_x_ratio = 0.5f);                    </code>
		/// adjust scrolling amount to make current cursor position visible. center_x_ratio=0.0: left, 0.5: center, 1.0: right. When using to make a "default/current item" visible, consider using SetItemDefaultFocus() instead. </summary>
	public static void SetScrollHereX()
	{
		ImGui_SetScrollHereX400((float)0.5f);
	}

	/// <summary><code>IMGUI_API void          SetScrollHereX(float center_x_ratio = 0.5f);                    </code>
		/// adjust scrolling amount to make current cursor position visible. center_x_ratio=0.0: left, 0.5: center, 1.0: right. When using to make a "default/current item" visible, consider using SetItemDefaultFocus() instead. </summary>
	public static void SetScrollHereX(float CenterXRatio)
	{
		ImGui_SetScrollHereX400(CenterXRatio);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_SetScrollHereY401(float CenterYRatio);

	/// <summary><code>IMGUI_API void          SetScrollHereY(float center_y_ratio = 0.5f);                    </code>
		/// adjust scrolling amount to make current cursor position visible. center_y_ratio=0.0: top, 0.5: center, 1.0: bottom. When using to make a "default/current item" visible, consider using SetItemDefaultFocus() instead. </summary>
	public static void SetScrollHereY()
	{
		ImGui_SetScrollHereY401((float)0.5f);
	}

	/// <summary><code>IMGUI_API void          SetScrollHereY(float center_y_ratio = 0.5f);                    </code>
		/// adjust scrolling amount to make current cursor position visible. center_y_ratio=0.0: top, 0.5: center, 1.0: bottom. When using to make a "default/current item" visible, consider using SetItemDefaultFocus() instead. </summary>
	public static void SetScrollHereY(float CenterYRatio)
	{
		ImGui_SetScrollHereY401(CenterYRatio);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_SetScrollFromPosX402(float LocalX, float CenterXRatio);

	/// <summary><code>IMGUI_API void          SetScrollFromPosX(float local_x, float center_x_ratio = 0.5f);  </code>
		/// adjust scrolling amount to make given position visible. Generally GetCursorStartPos() + offset to compute a valid position. </summary>
	public static void SetScrollFromPosX(float LocalX)
	{
		ImGui_SetScrollFromPosX402(LocalX, (float)0.5f);
	}

	/// <summary><code>IMGUI_API void          SetScrollFromPosX(float local_x, float center_x_ratio = 0.5f);  </code>
		/// adjust scrolling amount to make given position visible. Generally GetCursorStartPos() + offset to compute a valid position. </summary>
	public static void SetScrollFromPosX(float LocalX, float CenterXRatio)
	{
		ImGui_SetScrollFromPosX402(LocalX, CenterXRatio);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_SetScrollFromPosY403(float LocalY, float CenterYRatio);

	/// <summary><code>IMGUI_API void          SetScrollFromPosY(float local_y, float center_y_ratio = 0.5f);  </code>
		/// adjust scrolling amount to make given position visible. Generally GetCursorStartPos() + offset to compute a valid position. </summary>
	public static void SetScrollFromPosY(float LocalY)
	{
		ImGui_SetScrollFromPosY403(LocalY, (float)0.5f);
	}

	/// <summary><code>IMGUI_API void          SetScrollFromPosY(float local_y, float center_y_ratio = 0.5f);  </code>
		/// adjust scrolling amount to make given position visible. Generally GetCursorStartPos() + offset to compute a valid position. </summary>
	public static void SetScrollFromPosY(float LocalY, float CenterYRatio)
	{
		ImGui_SetScrollFromPosY403(LocalY, CenterYRatio);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_PushStyleColor408(ImGuiCol Idx, uint Col);

	/// <summary><code>IMGUI_API void          PushStyleColor(ImGuiCol idx, ImU32 col);                        </code>
		/// modify a style color. always use this if you modify the style after NewFrame(). </summary>
	public static void PushStyleColor(ImGuiCol Idx, uint Col)
	{
		ImGui_PushStyleColor408(Idx, Col);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_PushStyleColor409(ImGuiCol Idx, out  Vector4 Col);

	/// <summary><code>IMGUI_API void          PushStyleColor(ImGuiCol idx, const ImVec4& col);</code>
		///    IMGUI_API void          PushStyleColor(ImGuiCol idx, const ImVec4& col); </summary>
	public static void PushStyleColor(ImGuiCol Idx, out  Vector4 Col)
	{
		ImGui_PushStyleColor409(Idx, out Col);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_PopStyleColor410(int Count);

	/// <summary><code>IMGUI_API void          PopStyleColor(int count = 1);</code>
		///    IMGUI_API void          PopStyleColor(int count = 1); </summary>
	public static void PopStyleColor()
	{
		ImGui_PopStyleColor410((int)1);
	}

	/// <summary><code>IMGUI_API void          PopStyleColor(int count = 1);</code>
		///    IMGUI_API void          PopStyleColor(int count = 1); </summary>
	public static void PopStyleColor(int Count)
	{
		ImGui_PopStyleColor410(Count);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_PushStyleVar411(ImGuiStyleVar Idx, float Val);

	/// <summary><code>IMGUI_API void          PushStyleVar(ImGuiStyleVar idx, float val);                     </code>
		/// modify a style float variable. always use this if you modify the style after NewFrame(). </summary>
	public static void PushStyleVar(ImGuiStyleVar Idx, float Val)
	{
		ImGui_PushStyleVar411(Idx, Val);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_PushStyleVar412(ImGuiStyleVar Idx, out  Vector2 Val);

	/// <summary><code>IMGUI_API void          PushStyleVar(ImGuiStyleVar idx, const ImVec2& val);             </code>
		/// modify a style ImVec2 variable. always use this if you modify the style after NewFrame(). </summary>
	public static void PushStyleVar(ImGuiStyleVar Idx, out  Vector2 Val)
	{
		ImGui_PushStyleVar412(Idx, out Val);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_PopStyleVar413(int Count);

	/// <summary><code>IMGUI_API void          PopStyleVar(int count = 1);</code>
		///    IMGUI_API void          PopStyleVar(int count = 1); </summary>
	public static void PopStyleVar()
	{
		ImGui_PopStyleVar413((int)1);
	}

	/// <summary><code>IMGUI_API void          PopStyleVar(int count = 1);</code>
		///    IMGUI_API void          PopStyleVar(int count = 1); </summary>
	public static void PopStyleVar(int Count)
	{
		ImGui_PopStyleVar413(Count);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_PushTabStop414([MarshalAs(UnmanagedType.I1)]bool TabStop);

	/// <summary><code>IMGUI_API void          PushTabStop(bool tab_stop);                                     </code>
		/// == tab stop enable. Allow focusing using TAB/Shift-TAB, enabled by default but you can disable it for certain widgets </summary>
	public static void PushTabStop(bool TabStop)
	{
		ImGui_PushTabStop414(TabStop);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_PopTabStop415();

	/// <summary><code>IMGUI_API void          PopTabStop();</code>
		///    IMGUI_API void          PopTabStop(); </summary>
	public static void PopTabStop()
	{
		ImGui_PopTabStop415();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_PushButtonRepeat416([MarshalAs(UnmanagedType.I1)]bool Repeat);

	/// <summary><code>IMGUI_API void          PushButtonRepeat(bool repeat);                                  </code>
		/// in 'repeat' mode, Button*() functions return repeated true in a typematic manner (using io.KeyRepeatDelay/io.KeyRepeatRate setting). Note that you can call IsItemActive() after any Button() to tell if the button is held in the current frame. </summary>
	public static void PushButtonRepeat(bool Repeat)
	{
		ImGui_PushButtonRepeat416(Repeat);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_PopButtonRepeat417();

	/// <summary><code>IMGUI_API void          PopButtonRepeat();</code>
		///    IMGUI_API void          PopButtonRepeat(); </summary>
	public static void PopButtonRepeat()
	{
		ImGui_PopButtonRepeat417();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_PushItemWidth420(float ItemWidth);

	/// <summary><code>IMGUI_API void          PushItemWidth(float item_width);                                </code>
		/// push width of items for common large "item+label" widgets. >0.0f: width in pixels, <0.0f align xx pixels to the right of window (so -FLT_MIN always align width to the right side). </summary>
	public static void PushItemWidth(float ItemWidth)
	{
		ImGui_PushItemWidth420(ItemWidth);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_PopItemWidth421();

	/// <summary><code>IMGUI_API void          PopItemWidth();</code>
		///    IMGUI_API void          PopItemWidth(); </summary>
	public static void PopItemWidth()
	{
		ImGui_PopItemWidth421();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_SetNextItemWidth422(float ItemWidth);

	/// <summary><code>IMGUI_API void          SetNextItemWidth(float item_width);                             </code>
		/// set width of the _next_ common large "item+label" widget. >0.0f: width in pixels, <0.0f align xx pixels to the right of window (so -FLT_MIN always align width to the right side) </summary>
	public static void SetNextItemWidth(float ItemWidth)
	{
		ImGui_SetNextItemWidth422(ItemWidth);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGui_CalcItemWidth423();

	/// <summary><code>IMGUI_API float         CalcItemWidth();                                                </code>
		/// width of item given pushed settings and current cursor position. NOT necessarily the width of last item unlike most 'Item' functions. </summary>
	public static float CalcItemWidth()
	{
		return ImGui_CalcItemWidth423();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_PushTextWrapPos424(float WrapLocalPosX);

	/// <summary><code>IMGUI_API void          PushTextWrapPos(float wrap_local_pos_x = 0.0f);                 </code>
		/// push word-wrapping position for Text*() commands. < 0.0f: no wrapping; 0.0f: wrap to end of window (or column); > 0.0f: wrap at 'wrap_pos_x' position in window local space </summary>
	public static void PushTextWrapPos()
	{
		ImGui_PushTextWrapPos424((float)0.0f);
	}

	/// <summary><code>IMGUI_API void          PushTextWrapPos(float wrap_local_pos_x = 0.0f);                 </code>
		/// push word-wrapping position for Text*() commands. < 0.0f: no wrapping; 0.0f: wrap to end of window (or column); > 0.0f: wrap at 'wrap_pos_x' position in window local space </summary>
	public static void PushTextWrapPos(float WrapLocalPosX)
	{
		ImGui_PushTextWrapPos424(WrapLocalPosX);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_PopTextWrapPos425();

	/// <summary><code>IMGUI_API void          PopTextWrapPos();</code>
		///    IMGUI_API void          PopTextWrapPos(); </summary>
	public static void PopTextWrapPos()
	{
		ImGui_PopTextWrapPos425();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern uint ImGui_GetColorU32432(ImGuiCol Idx, float AlphaMul);

	/// <summary><code>IMGUI_API ImU32         GetColorU32(ImGuiCol idx, float alpha_mul = 1.0f);              </code>
		/// retrieve given style color with style alpha applied and optional extra alpha multiplier, packed as a 32-bit value suitable for ImDrawList </summary>
	public static uint GetColorU32(ImGuiCol Idx)
	{
		return ImGui_GetColorU32432(Idx, (float)1.0f);
	}

	/// <summary><code>IMGUI_API ImU32         GetColorU32(ImGuiCol idx, float alpha_mul = 1.0f);              </code>
		/// retrieve given style color with style alpha applied and optional extra alpha multiplier, packed as a 32-bit value suitable for ImDrawList </summary>
	public static uint GetColorU32(ImGuiCol Idx, float AlphaMul)
	{
		return ImGui_GetColorU32432(Idx, AlphaMul);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern uint ImGui_GetColorU32433(out  Vector4 Col);

	/// <summary><code>IMGUI_API ImU32         GetColorU32(const ImVec4& col);                                 </code>
		/// retrieve given color with style alpha applied, packed as a 32-bit value suitable for ImDrawList </summary>
	public static uint GetColorU32(out  Vector4 Col)
	{
		return ImGui_GetColorU32433(out Col);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern uint ImGui_GetColorU32434(uint Col);

	/// <summary><code>IMGUI_API ImU32         GetColorU32(ImU32 col);                                         </code>
		/// retrieve given color with style alpha applied, packed as a 32-bit value suitable for ImDrawList </summary>
	public static uint GetColorU32(uint Col)
	{
		return ImGui_GetColorU32434(Col);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern ref  Vector4 ImGui_GetStyleColorVec4435(ImGuiCol Idx);

	/// <summary><code>IMGUI_API const ImVec4& GetStyleColorVec4(ImGuiCol idx);                                </code>
		/// retrieve style color as stored in ImGuiStyle structure. use to feed back into PushStyleColor(), otherwise use GetColorU32() to get style color with style alpha baked in. </summary>
	public static ref  Vector4 GetStyleColorVec4(ImGuiCol Idx)
	{
		return ref ImGui_GetStyleColorVec4435(Idx);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_Separator444();

	/// <summary><code>IMGUI_API void          Separator();                                                    </code>
		/// separator, generally horizontal. inside a menu bar or in horizontal layout mode, this becomes a vertical separator. </summary>
	public static void Separator()
	{
		ImGui_Separator444();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_SameLine445(float OffsetFromStartX, float Spacing);

	/// <summary><code>IMGUI_API void          SameLine(float offset_from_start_x=0.0f, float spacing=-1.0f);  </code>
		/// call between widgets or groups to layout them horizontally. X position given in window coordinates. </summary>
	public static void SameLine()
	{
		ImGui_SameLine445((float)0.0f, -1.0f);
	}

	/// <summary><code>IMGUI_API void          SameLine(float offset_from_start_x=0.0f, float spacing=-1.0f);  </code>
		/// call between widgets or groups to layout them horizontally. X position given in window coordinates. </summary>
	public static void SameLine(float OffsetFromStartX)
	{
		ImGui_SameLine445(OffsetFromStartX, -1.0f);
	}

	/// <summary><code>IMGUI_API void          SameLine(float offset_from_start_x=0.0f, float spacing=-1.0f);  </code>
		/// call between widgets or groups to layout them horizontally. X position given in window coordinates. </summary>
	public static void SameLine(float OffsetFromStartX, float Spacing)
	{
		ImGui_SameLine445(OffsetFromStartX, Spacing);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_NewLine446();

	/// <summary><code>IMGUI_API void          NewLine();                                                      </code>
		/// undo a SameLine() or force a new line when in a horizontal-layout context. </summary>
	public static void NewLine()
	{
		ImGui_NewLine446();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_Spacing447();

	/// <summary><code>IMGUI_API void          Spacing();                                                      </code>
		/// add vertical spacing. </summary>
	public static void Spacing()
	{
		ImGui_Spacing447();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_Dummy448(out  Vector2 Size);

	/// <summary><code>IMGUI_API void          Dummy(const ImVec2& size);                                      </code>
		/// add a dummy item of given size. unlike InvisibleButton(), Dummy() won't take the mouse click or be navigable into. </summary>
	public static void Dummy(out  Vector2 Size)
	{
		ImGui_Dummy448(out Size);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_Indent449(float IndentW);

	/// <summary><code>IMGUI_API void          Indent(float indent_w = 0.0f);                                  </code>
		/// move content position toward the right, by indent_w, or style.IndentSpacing if indent_w <= 0 </summary>
	public static void Indent()
	{
		ImGui_Indent449((float)0.0f);
	}

	/// <summary><code>IMGUI_API void          Indent(float indent_w = 0.0f);                                  </code>
		/// move content position toward the right, by indent_w, or style.IndentSpacing if indent_w <= 0 </summary>
	public static void Indent(float IndentW)
	{
		ImGui_Indent449(IndentW);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_Unindent450(float IndentW);

	/// <summary><code>IMGUI_API void          Unindent(float indent_w = 0.0f);                                </code>
		/// move content position back to the left, by indent_w, or style.IndentSpacing if indent_w <= 0 </summary>
	public static void Unindent()
	{
		ImGui_Unindent450((float)0.0f);
	}

	/// <summary><code>IMGUI_API void          Unindent(float indent_w = 0.0f);                                </code>
		/// move content position back to the left, by indent_w, or style.IndentSpacing if indent_w <= 0 </summary>
	public static void Unindent(float IndentW)
	{
		ImGui_Unindent450(IndentW);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_BeginGroup451();

	/// <summary><code>IMGUI_API void          BeginGroup();                                                   </code>
		/// lock horizontal starting position </summary>
	public static void BeginGroup()
	{
		ImGui_BeginGroup451();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_EndGroup452();

	/// <summary><code>IMGUI_API void          EndGroup();                                                     </code>
		/// unlock horizontal starting position + capture the whole group bounding box into one "item" (so you can use IsItemHovered() or layout primitives such as SameLine() on whole group, etc.) </summary>
	public static void EndGroup()
	{
		ImGui_EndGroup452();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern Vector2 ImGui_GetCursorPos453();

	/// <summary><code>IMGUI_API ImVec2        GetCursorPos();                                                 </code>
		/// cursor position in window coordinates (relative to window position) </summary>
	public static Vector2 GetCursorPos()
	{
		return ImGui_GetCursorPos453();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGui_GetCursorPosX454();

	/// <summary><code>IMGUI_API float         GetCursorPosX();                                                </code>
		///   (some functions are using window-relative coordinates, such as: GetCursorPos, GetCursorStartPos, GetContentRegionMax, GetWindowContentRegion* etc. </summary>
	public static float GetCursorPosX()
	{
		return ImGui_GetCursorPosX454();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGui_GetCursorPosY455();

	/// <summary><code>IMGUI_API float         GetCursorPosY();                                                </code>
		///    other functions such as GetCursorScreenPos or everything in ImDrawList:: </summary>
	public static float GetCursorPosY()
	{
		return ImGui_GetCursorPosY455();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_SetCursorPos456(out  Vector2 LocalPos);

	/// <summary><code>IMGUI_API void          SetCursorPos(const ImVec2& local_pos);                          </code>
		///    are using the main, absolute coordinate system. </summary>
	public static void SetCursorPos(out  Vector2 LocalPos)
	{
		ImGui_SetCursorPos456(out LocalPos);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_SetCursorPosX457(float LocalX);

	/// <summary><code>IMGUI_API void          SetCursorPosX(float local_x);                                   </code>
		///    GetWindowPos() + GetCursorPos() == GetCursorScreenPos() etc.) </summary>
	public static void SetCursorPosX(float LocalX)
	{
		ImGui_SetCursorPosX457(LocalX);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_SetCursorPosY458(float LocalY);

	/// <summary><code>IMGUI_API void          SetCursorPosY(float local_y);                                   </code>
		/// </summary>
	public static void SetCursorPosY(float LocalY)
	{
		ImGui_SetCursorPosY458(LocalY);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern Vector2 ImGui_GetCursorStartPos459();

	/// <summary><code>IMGUI_API ImVec2        GetCursorStartPos();                                            </code>
		/// initial cursor position in window coordinates </summary>
	public static Vector2 GetCursorStartPos()
	{
		return ImGui_GetCursorStartPos459();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern Vector2 ImGui_GetCursorScreenPos460();

	/// <summary><code>IMGUI_API ImVec2        GetCursorScreenPos();                                           </code>
		/// cursor position in absolute coordinates (useful to work with ImDrawList API). generally top-left == GetMainViewport()->Pos == (0,0) in single viewport mode, and bottom-right == GetMainViewport()->Pos+Size == io.DisplaySize in single-viewport mode. </summary>
	public static Vector2 GetCursorScreenPos()
	{
		return ImGui_GetCursorScreenPos460();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_SetCursorScreenPos461(out  Vector2 Pos);

	/// <summary><code>IMGUI_API void          SetCursorScreenPos(const ImVec2& pos);                          </code>
		/// cursor position in absolute coordinates </summary>
	public static void SetCursorScreenPos(out  Vector2 Pos)
	{
		ImGui_SetCursorScreenPos461(out Pos);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_AlignTextToFramePadding462();

	/// <summary><code>IMGUI_API void          AlignTextToFramePadding();                                      </code>
		/// vertically align upcoming text baseline to FramePadding.y so that it will align properly to regularly framed items (call if you have text on a line before a framed item) </summary>
	public static void AlignTextToFramePadding()
	{
		ImGui_AlignTextToFramePadding462();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGui_GetTextLineHeight463();

	/// <summary><code>IMGUI_API float         GetTextLineHeight();                                            </code>
		/// ~ FontSize </summary>
	public static float GetTextLineHeight()
	{
		return ImGui_GetTextLineHeight463();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGui_GetTextLineHeightWithSpacing464();

	/// <summary><code>IMGUI_API float         GetTextLineHeightWithSpacing();                                 </code>
		/// ~ FontSize + style.ItemSpacing.y (distance in pixels between 2 consecutive lines of text) </summary>
	public static float GetTextLineHeightWithSpacing()
	{
		return ImGui_GetTextLineHeightWithSpacing464();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGui_GetFrameHeight465();

	/// <summary><code>IMGUI_API float         GetFrameHeight();                                               </code>
		/// ~ FontSize + style.FramePadding.y * 2 </summary>
	public static float GetFrameHeight()
	{
		return ImGui_GetFrameHeight465();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGui_GetFrameHeightWithSpacing466();

	/// <summary><code>IMGUI_API float         GetFrameHeightWithSpacing();                                    </code>
		/// ~ FontSize + style.FramePadding.y * 2 + style.ItemSpacing.y (distance in pixels between 2 consecutive lines of framed widgets) </summary>
	public static float GetFrameHeightWithSpacing()
	{
		return ImGui_GetFrameHeightWithSpacing466();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_PushID479([MarshalAs(UnmanagedType.LPStr)]string StrId);

	/// <summary><code>IMGUI_API void          PushID(const char* str_id);                                     </code>
		/// push string into the ID stack (will hash string). </summary>
	public static void PushID(string StrId)
	{
		ImGui_PushID479(StrId);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_PushID480([MarshalAs(UnmanagedType.LPStr)]string StrIdBegin, [MarshalAs(UnmanagedType.LPStr)]string StrIdEnd);

	/// <summary><code>IMGUI_API void          PushID(const char* str_id_begin, const char* str_id_end);       </code>
		/// push string into the ID stack (will hash string). </summary>
	public static void PushID(string StrIdBegin, string StrIdEnd)
	{
		ImGui_PushID480(StrIdBegin, StrIdEnd);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_PushID481( IntPtr PtrId);

	/// <summary><code>IMGUI_API void          PushID(const void* ptr_id);                                     </code>
		/// push pointer into the ID stack (will hash pointer). </summary>
	public static void PushID( IntPtr PtrId)
	{
		ImGui_PushID481(PtrId);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_PushID482(int IntId);

	/// <summary><code>IMGUI_API void          PushID(int int_id);                                             </code>
		/// push integer into the ID stack (will hash integer). </summary>
	public static void PushID(int IntId)
	{
		ImGui_PushID482(IntId);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_PopID483();

	/// <summary><code>IMGUI_API void          PopID();                                                        </code>
		/// pop from the ID stack. </summary>
	public static void PopID()
	{
		ImGui_PopID483();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern ImGuiID ImGui_GetID484([MarshalAs(UnmanagedType.LPStr)]string StrId);

	/// <summary><code>IMGUI_API ImGuiID       GetID(const char* str_id);                                      </code>
		/// calculate unique ID (hash of whole ID stack + given parameter). e.g. if you want to query into ImGuiStorage yourself </summary>
	public static ImGuiID GetID(string StrId)
	{
		return ImGui_GetID484(StrId);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern ImGuiID ImGui_GetID485([MarshalAs(UnmanagedType.LPStr)]string StrIdBegin, [MarshalAs(UnmanagedType.LPStr)]string StrIdEnd);

	/// <summary><code>IMGUI_API ImGuiID       GetID(const char* str_id_begin, const char* str_id_end);</code>
		///    IMGUI_API ImGuiID       GetID(const char* str_id_begin, const char* str_id_end); </summary>
	public static ImGuiID GetID(string StrIdBegin, string StrIdEnd)
	{
		return ImGui_GetID485(StrIdBegin, StrIdEnd);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern ImGuiID ImGui_GetID486( IntPtr PtrId);

	/// <summary><code>IMGUI_API ImGuiID       GetID(const void* ptr_id);</code>
		///    IMGUI_API ImGuiID       GetID(const void* ptr_id); </summary>
	public static ImGuiID GetID( IntPtr PtrId)
	{
		return ImGui_GetID486(PtrId);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_TextUnformatted489([MarshalAs(UnmanagedType.LPStr)]string Text, [MarshalAs(UnmanagedType.LPStr)]string TextEnd);

	/// <summary><code>IMGUI_API void          TextUnformatted(const char* text, const char* text_end = NULL); </code>
		/// raw text without formatting. Roughly equivalent to Text("%s", text) but: A) doesn't require null terminated string if 'text_end' is specified, B) it's faster, no memory copy is done, no buffer size limits, recommended for long chunks of text. </summary>
	public static void TextUnformatted(string Text)
	{
		ImGui_TextUnformatted489(Text, default);
	}

	/// <summary><code>IMGUI_API void          TextUnformatted(const char* text, const char* text_end = NULL); </code>
		/// raw text without formatting. Roughly equivalent to Text("%s", text) but: A) doesn't require null terminated string if 'text_end' is specified, B) it's faster, no memory copy is done, no buffer size limits, recommended for long chunks of text. </summary>
	public static void TextUnformatted(string Text, string TextEnd)
	{
		ImGui_TextUnformatted489(Text, TextEnd);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_SeparatorText502([MarshalAs(UnmanagedType.LPStr)]string Label);

	/// <summary><code>IMGUI_API void          SeparatorText(const char* label);                               </code>
		/// currently: formatted text with an horizontal line </summary>
	public static void SeparatorText(string Label)
	{
		ImGui_SeparatorText502(Label);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_Button507([MarshalAs(UnmanagedType.LPStr)]string Label, out  Vector2 Size);

	/// <summary><code>IMGUI_API bool          Button(const char* label, const ImVec2& size = ImVec2(0, 0));   </code>
		/// button </summary>
	public static bool Button(string Label)
	{
		 Vector2 param1 = new  Vector2 (0,  0);
		return ImGui_Button507(Label, out param1);
	}

	/// <summary><code>IMGUI_API bool          Button(const char* label, const ImVec2& size = ImVec2(0, 0));   </code>
		/// button </summary>
	public static bool Button(string Label, out  Vector2 Size)
	{
		return ImGui_Button507(Label, out Size);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_SmallButton508([MarshalAs(UnmanagedType.LPStr)]string Label);

	/// <summary><code>IMGUI_API bool          SmallButton(const char* label);                                 </code>
		/// button with FramePadding=(0,0) to easily embed within text </summary>
	public static bool SmallButton(string Label)
	{
		return ImGui_SmallButton508(Label);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_InvisibleButton509([MarshalAs(UnmanagedType.LPStr)]string StrId, out  Vector2 Size, ImGuiButtonFlags Flags);

	/// <summary><code>IMGUI_API bool          InvisibleButton(const char* str_id, const ImVec2& size, ImGuiButtonFlags flags = 0); </code>
		/// flexible button behavior without the visuals, frequently useful to build custom behaviors using the public api (along with IsItemActive, IsItemHovered, etc.) </summary>
	public static bool InvisibleButton(string StrId, out  Vector2 Size)
	{
		return ImGui_InvisibleButton509(StrId, out Size, (ImGuiButtonFlags)0);
	}

	/// <summary><code>IMGUI_API bool          InvisibleButton(const char* str_id, const ImVec2& size, ImGuiButtonFlags flags = 0); </code>
		/// flexible button behavior without the visuals, frequently useful to build custom behaviors using the public api (along with IsItemActive, IsItemHovered, etc.) </summary>
	public static bool InvisibleButton(string StrId, out  Vector2 Size, ImGuiButtonFlags Flags)
	{
		return ImGui_InvisibleButton509(StrId, out Size, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_ArrowButton510([MarshalAs(UnmanagedType.LPStr)]string StrId, ImGuiDir Dir);

	/// <summary><code>IMGUI_API bool          ArrowButton(const char* str_id, ImGuiDir dir);                  </code>
		/// square button with an arrow shape </summary>
	public static bool ArrowButton(string StrId, ImGuiDir Dir)
	{
		return ImGui_ArrowButton510(StrId, Dir);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_Checkbox511([MarshalAs(UnmanagedType.LPStr)]string Label, [MarshalAs(UnmanagedType.I1)]out bool V);

	/// <summary><code>IMGUI_API bool          Checkbox(const char* label, bool* v);</code>
		///    IMGUI_API bool          Checkbox(const char* label, bool* v); </summary>
	public static bool Checkbox(string Label, out bool V)
	{
		return ImGui_Checkbox511(Label, out V);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_CheckboxFlags512([MarshalAs(UnmanagedType.LPStr)]string Label, out int Flags, int FlagsValue);

	/// <summary><code>IMGUI_API bool          CheckboxFlags(const char* label, int* flags, int flags_value);</code>
		///    IMGUI_API bool          CheckboxFlags(const char* label, int* flags, int flags_value); </summary>
	public static bool CheckboxFlags(string Label, out int Flags, int FlagsValue)
	{
		return ImGui_CheckboxFlags512(Label, out Flags, FlagsValue);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_CheckboxFlags513([MarshalAs(UnmanagedType.LPStr)]string Label, out uint Flags, uint FlagsValue);

	/// <summary><code>IMGUI_API bool          CheckboxFlags(const char* label, unsigned int* flags, unsigned int flags_value);</code>
		///    IMGUI_API bool          CheckboxFlags(const char* label, unsigned int* flags, unsigned int flags_value); </summary>
	public static bool CheckboxFlags(string Label, out uint Flags, uint FlagsValue)
	{
		return ImGui_CheckboxFlags513(Label, out Flags, FlagsValue);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_RadioButton514([MarshalAs(UnmanagedType.LPStr)]string Label, [MarshalAs(UnmanagedType.I1)]bool Active);

	/// <summary><code>IMGUI_API bool          RadioButton(const char* label, bool active);                    </code>
		/// use with e.g. if (RadioButton("one", my_value==1)) { my_value = 1; } </summary>
	public static bool RadioButton(string Label, bool Active)
	{
		return ImGui_RadioButton514(Label, Active);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_RadioButton515([MarshalAs(UnmanagedType.LPStr)]string Label, out int V, int VButton);

	/// <summary><code>IMGUI_API bool          RadioButton(const char* label, int* v, int v_button);           </code>
		/// shortcut to handle the above pattern when value is an integer </summary>
	public static bool RadioButton(string Label, out int V, int VButton)
	{
		return ImGui_RadioButton515(Label, out V, VButton);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_ProgressBar516(float Fraction, out  Vector2 SizeArg, [MarshalAs(UnmanagedType.LPStr)]string Overlay);

	/// <summary><code>IMGUI_API void          ProgressBar(float fraction, const ImVec2& size_arg = ImVec2(-FLT_MIN, 0), const char* overlay = NULL);</code>
		///    IMGUI_API void          ProgressBar(float fraction, const ImVec2& size_arg = ImVec2(-FLT_MIN, 0), const char* overlay = NULL); </summary>
	public static void ProgressBar(float Fraction)
	{
		 Vector2 param1 = new  Vector2 (-float.MinValue,  0);
		ImGui_ProgressBar516(Fraction, out param1, default);
	}

	/// <summary><code>IMGUI_API void          ProgressBar(float fraction, const ImVec2& size_arg = ImVec2(-FLT_MIN, 0), const char* overlay = NULL);</code>
		///    IMGUI_API void          ProgressBar(float fraction, const ImVec2& size_arg = ImVec2(-FLT_MIN, 0), const char* overlay = NULL); </summary>
	public static void ProgressBar(float Fraction, out  Vector2 SizeArg)
	{
		ImGui_ProgressBar516(Fraction, out SizeArg, default);
	}

	/// <summary><code>IMGUI_API void          ProgressBar(float fraction, const ImVec2& size_arg = ImVec2(-FLT_MIN, 0), const char* overlay = NULL);</code>
		///    IMGUI_API void          ProgressBar(float fraction, const ImVec2& size_arg = ImVec2(-FLT_MIN, 0), const char* overlay = NULL); </summary>
	public static void ProgressBar(float Fraction, out  Vector2 SizeArg, string Overlay)
	{
		ImGui_ProgressBar516(Fraction, out SizeArg, Overlay);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_Bullet517();

	/// <summary><code>IMGUI_API void          Bullet();                                                       </code>
		/// draw a small circle + keep the cursor on the same line. advance cursor x position by GetTreeNodeToLabelSpacing(), same distance that TreeNode() uses </summary>
	public static void Bullet()
	{
		ImGui_Bullet517();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_Image521(ImTextureID UserTextureId, out  Vector2 Size, out  Vector2 Uv0, out  Vector2 Uv1, out  Vector4 TintCol, out  Vector4 BorderCol);

	/// <summary><code>IMGUI_API void          Image(ImTextureID user_texture_id, const ImVec2& size, const ImVec2& uv0 = ImVec2(0, 0), const ImVec2& uv1 = ImVec2(1, 1), const ImVec4& tint_col = ImVec4(1, 1, 1, 1), const ImVec4& border_col = ImVec4(0, 0, 0, 0));</code>
		///    IMGUI_API void          Image(ImTextureID user_texture_id, const ImVec2& size, const ImVec2& uv0 = ImVec2(0, 0), const ImVec2& uv1 = ImVec2(1, 1), const ImVec4& tint_col = ImVec4(1, 1, 1, 1), const ImVec4& border_col = ImVec4(0, 0, 0, 0)); </summary>
	public static void Image(ImTextureID UserTextureId, out  Vector2 Size)
	{
		 Vector2 param2 = new  Vector2 (0,  0);
		 Vector2 param3 = new  Vector2 (1,  1);
		 Vector4 param4 = new  Vector4 (1,  1,  1,  1);
		 Vector4 param5 = new  Vector4 (0,  0,  0,  0);
		ImGui_Image521(UserTextureId, out Size, out param2, out param3, out param4, out param5);
	}

	/// <summary><code>IMGUI_API void          Image(ImTextureID user_texture_id, const ImVec2& size, const ImVec2& uv0 = ImVec2(0, 0), const ImVec2& uv1 = ImVec2(1, 1), const ImVec4& tint_col = ImVec4(1, 1, 1, 1), const ImVec4& border_col = ImVec4(0, 0, 0, 0));</code>
		///    IMGUI_API void          Image(ImTextureID user_texture_id, const ImVec2& size, const ImVec2& uv0 = ImVec2(0, 0), const ImVec2& uv1 = ImVec2(1, 1), const ImVec4& tint_col = ImVec4(1, 1, 1, 1), const ImVec4& border_col = ImVec4(0, 0, 0, 0)); </summary>
	public static void Image(ImTextureID UserTextureId, out  Vector2 Size, out  Vector2 Uv0)
	{
		 Vector2 param3 = new  Vector2 (1,  1);
		 Vector4 param4 = new  Vector4 (1,  1,  1,  1);
		 Vector4 param5 = new  Vector4 (0,  0,  0,  0);
		ImGui_Image521(UserTextureId, out Size, out Uv0, out param3, out param4, out param5);
	}

	/// <summary><code>IMGUI_API void          Image(ImTextureID user_texture_id, const ImVec2& size, const ImVec2& uv0 = ImVec2(0, 0), const ImVec2& uv1 = ImVec2(1, 1), const ImVec4& tint_col = ImVec4(1, 1, 1, 1), const ImVec4& border_col = ImVec4(0, 0, 0, 0));</code>
		///    IMGUI_API void          Image(ImTextureID user_texture_id, const ImVec2& size, const ImVec2& uv0 = ImVec2(0, 0), const ImVec2& uv1 = ImVec2(1, 1), const ImVec4& tint_col = ImVec4(1, 1, 1, 1), const ImVec4& border_col = ImVec4(0, 0, 0, 0)); </summary>
	public static void Image(ImTextureID UserTextureId, out  Vector2 Size, out  Vector2 Uv0, out  Vector2 Uv1)
	{
		 Vector4 param4 = new  Vector4 (1,  1,  1,  1);
		 Vector4 param5 = new  Vector4 (0,  0,  0,  0);
		ImGui_Image521(UserTextureId, out Size, out Uv0, out Uv1, out param4, out param5);
	}

	/// <summary><code>IMGUI_API void          Image(ImTextureID user_texture_id, const ImVec2& size, const ImVec2& uv0 = ImVec2(0, 0), const ImVec2& uv1 = ImVec2(1, 1), const ImVec4& tint_col = ImVec4(1, 1, 1, 1), const ImVec4& border_col = ImVec4(0, 0, 0, 0));</code>
		///    IMGUI_API void          Image(ImTextureID user_texture_id, const ImVec2& size, const ImVec2& uv0 = ImVec2(0, 0), const ImVec2& uv1 = ImVec2(1, 1), const ImVec4& tint_col = ImVec4(1, 1, 1, 1), const ImVec4& border_col = ImVec4(0, 0, 0, 0)); </summary>
	public static void Image(ImTextureID UserTextureId, out  Vector2 Size, out  Vector2 Uv0, out  Vector2 Uv1, out  Vector4 TintCol)
	{
		 Vector4 param5 = new  Vector4 (0,  0,  0,  0);
		ImGui_Image521(UserTextureId, out Size, out Uv0, out Uv1, out TintCol, out param5);
	}

	/// <summary><code>IMGUI_API void          Image(ImTextureID user_texture_id, const ImVec2& size, const ImVec2& uv0 = ImVec2(0, 0), const ImVec2& uv1 = ImVec2(1, 1), const ImVec4& tint_col = ImVec4(1, 1, 1, 1), const ImVec4& border_col = ImVec4(0, 0, 0, 0));</code>
		///    IMGUI_API void          Image(ImTextureID user_texture_id, const ImVec2& size, const ImVec2& uv0 = ImVec2(0, 0), const ImVec2& uv1 = ImVec2(1, 1), const ImVec4& tint_col = ImVec4(1, 1, 1, 1), const ImVec4& border_col = ImVec4(0, 0, 0, 0)); </summary>
	public static void Image(ImTextureID UserTextureId, out  Vector2 Size, out  Vector2 Uv0, out  Vector2 Uv1, out  Vector4 TintCol, out  Vector4 BorderCol)
	{
		ImGui_Image521(UserTextureId, out Size, out Uv0, out Uv1, out TintCol, out BorderCol);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_ImageButton522([MarshalAs(UnmanagedType.LPStr)]string StrId, ImTextureID UserTextureId, out  Vector2 Size, out  Vector2 Uv0, out  Vector2 Uv1, out  Vector4 BgCol, out  Vector4 TintCol);

	/// <summary><code>IMGUI_API bool          ImageButton(const char* str_id, ImTextureID user_texture_id, const ImVec2& size, const ImVec2& uv0 = ImVec2(0, 0), const ImVec2& uv1 = ImVec2(1, 1), const ImVec4& bg_col = ImVec4(0, 0, 0, 0), const ImVec4& tint_col = ImVec4(1, 1, 1, 1));</code>
		///    IMGUI_API bool          ImageButton(const char* str_id, ImTextureID user_texture_id, const ImVec2& size, const ImVec2& uv0 = ImVec2(0, 0), const ImVec2& uv1 = ImVec2(1, 1), const ImVec4& bg_col = ImVec4(0, 0, 0, 0), const ImVec4& tint_col = ImVec4(1, 1, 1, 1)); </summary>
	public static bool ImageButton(string StrId, ImTextureID UserTextureId, out  Vector2 Size)
	{
		 Vector2 param3 = new  Vector2 (0,  0);
		 Vector2 param4 = new  Vector2 (1,  1);
		 Vector4 param5 = new  Vector4 (0,  0,  0,  0);
		 Vector4 param6 = new  Vector4 (1,  1,  1,  1);
		return ImGui_ImageButton522(StrId, UserTextureId, out Size, out param3, out param4, out param5, out param6);
	}

	/// <summary><code>IMGUI_API bool          ImageButton(const char* str_id, ImTextureID user_texture_id, const ImVec2& size, const ImVec2& uv0 = ImVec2(0, 0), const ImVec2& uv1 = ImVec2(1, 1), const ImVec4& bg_col = ImVec4(0, 0, 0, 0), const ImVec4& tint_col = ImVec4(1, 1, 1, 1));</code>
		///    IMGUI_API bool          ImageButton(const char* str_id, ImTextureID user_texture_id, const ImVec2& size, const ImVec2& uv0 = ImVec2(0, 0), const ImVec2& uv1 = ImVec2(1, 1), const ImVec4& bg_col = ImVec4(0, 0, 0, 0), const ImVec4& tint_col = ImVec4(1, 1, 1, 1)); </summary>
	public static bool ImageButton(string StrId, ImTextureID UserTextureId, out  Vector2 Size, out  Vector2 Uv0)
	{
		 Vector2 param4 = new  Vector2 (1,  1);
		 Vector4 param5 = new  Vector4 (0,  0,  0,  0);
		 Vector4 param6 = new  Vector4 (1,  1,  1,  1);
		return ImGui_ImageButton522(StrId, UserTextureId, out Size, out Uv0, out param4, out param5, out param6);
	}

	/// <summary><code>IMGUI_API bool          ImageButton(const char* str_id, ImTextureID user_texture_id, const ImVec2& size, const ImVec2& uv0 = ImVec2(0, 0), const ImVec2& uv1 = ImVec2(1, 1), const ImVec4& bg_col = ImVec4(0, 0, 0, 0), const ImVec4& tint_col = ImVec4(1, 1, 1, 1));</code>
		///    IMGUI_API bool          ImageButton(const char* str_id, ImTextureID user_texture_id, const ImVec2& size, const ImVec2& uv0 = ImVec2(0, 0), const ImVec2& uv1 = ImVec2(1, 1), const ImVec4& bg_col = ImVec4(0, 0, 0, 0), const ImVec4& tint_col = ImVec4(1, 1, 1, 1)); </summary>
	public static bool ImageButton(string StrId, ImTextureID UserTextureId, out  Vector2 Size, out  Vector2 Uv0, out  Vector2 Uv1)
	{
		 Vector4 param5 = new  Vector4 (0,  0,  0,  0);
		 Vector4 param6 = new  Vector4 (1,  1,  1,  1);
		return ImGui_ImageButton522(StrId, UserTextureId, out Size, out Uv0, out Uv1, out param5, out param6);
	}

	/// <summary><code>IMGUI_API bool          ImageButton(const char* str_id, ImTextureID user_texture_id, const ImVec2& size, const ImVec2& uv0 = ImVec2(0, 0), const ImVec2& uv1 = ImVec2(1, 1), const ImVec4& bg_col = ImVec4(0, 0, 0, 0), const ImVec4& tint_col = ImVec4(1, 1, 1, 1));</code>
		///    IMGUI_API bool          ImageButton(const char* str_id, ImTextureID user_texture_id, const ImVec2& size, const ImVec2& uv0 = ImVec2(0, 0), const ImVec2& uv1 = ImVec2(1, 1), const ImVec4& bg_col = ImVec4(0, 0, 0, 0), const ImVec4& tint_col = ImVec4(1, 1, 1, 1)); </summary>
	public static bool ImageButton(string StrId, ImTextureID UserTextureId, out  Vector2 Size, out  Vector2 Uv0, out  Vector2 Uv1, out  Vector4 BgCol)
	{
		 Vector4 param6 = new  Vector4 (1,  1,  1,  1);
		return ImGui_ImageButton522(StrId, UserTextureId, out Size, out Uv0, out Uv1, out BgCol, out param6);
	}

	/// <summary><code>IMGUI_API bool          ImageButton(const char* str_id, ImTextureID user_texture_id, const ImVec2& size, const ImVec2& uv0 = ImVec2(0, 0), const ImVec2& uv1 = ImVec2(1, 1), const ImVec4& bg_col = ImVec4(0, 0, 0, 0), const ImVec4& tint_col = ImVec4(1, 1, 1, 1));</code>
		///    IMGUI_API bool          ImageButton(const char* str_id, ImTextureID user_texture_id, const ImVec2& size, const ImVec2& uv0 = ImVec2(0, 0), const ImVec2& uv1 = ImVec2(1, 1), const ImVec4& bg_col = ImVec4(0, 0, 0, 0), const ImVec4& tint_col = ImVec4(1, 1, 1, 1)); </summary>
	public static bool ImageButton(string StrId, ImTextureID UserTextureId, out  Vector2 Size, out  Vector2 Uv0, out  Vector2 Uv1, out  Vector4 BgCol, out  Vector4 TintCol)
	{
		return ImGui_ImageButton522(StrId, UserTextureId, out Size, out Uv0, out Uv1, out BgCol, out TintCol);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_BeginCombo527([MarshalAs(UnmanagedType.LPStr)]string Label, [MarshalAs(UnmanagedType.LPStr)]string PreviewValue, ImGuiComboFlags Flags);

	/// <summary><code>IMGUI_API bool          BeginCombo(const char* label, const char* preview_value, ImGuiComboFlags flags = 0);</code>
		///    IMGUI_API bool          BeginCombo(const char* label, const char* preview_value, ImGuiComboFlags flags = 0); </summary>
	public static bool BeginCombo(string Label, string PreviewValue)
	{
		return ImGui_BeginCombo527(Label, PreviewValue, (ImGuiComboFlags)0);
	}

	/// <summary><code>IMGUI_API bool          BeginCombo(const char* label, const char* preview_value, ImGuiComboFlags flags = 0);</code>
		///    IMGUI_API bool          BeginCombo(const char* label, const char* preview_value, ImGuiComboFlags flags = 0); </summary>
	public static bool BeginCombo(string Label, string PreviewValue, ImGuiComboFlags Flags)
	{
		return ImGui_BeginCombo527(Label, PreviewValue, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_EndCombo528();

	/// <summary><code>IMGUI_API void          EndCombo(); </code>
		/// only call EndCombo() if BeginCombo() returns true! </summary>
	public static void EndCombo()
	{
		ImGui_EndCombo528();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_Combo529([MarshalAs(UnmanagedType.LPStr)]string Label, out int CurrentItem, [MarshalAs(UnmanagedType.LPStr)]out string  Items, int ItemsCount, int PopupMaxHeightInItems);

	/// <summary><code>IMGUI_API bool          Combo(const char* label, int* current_item, const char* const items[], int items_count, int popup_max_height_in_items = -1);</code>
		///    IMGUI_API bool          Combo(const char* label, int* current_item, const char* const items[], int items_count, int popup_max_height_in_items = -1); </summary>
	public static bool Combo(string Label, out int CurrentItem, out string  Items, int ItemsCount)
	{
		return ImGui_Combo529(Label, out CurrentItem, out Items, ItemsCount, -1);
	}

	/// <summary><code>IMGUI_API bool          Combo(const char* label, int* current_item, const char* const items[], int items_count, int popup_max_height_in_items = -1);</code>
		///    IMGUI_API bool          Combo(const char* label, int* current_item, const char* const items[], int items_count, int popup_max_height_in_items = -1); </summary>
	public static bool Combo(string Label, out int CurrentItem, out string  Items, int ItemsCount, int PopupMaxHeightInItems)
	{
		return ImGui_Combo529(Label, out CurrentItem, out Items, ItemsCount, PopupMaxHeightInItems);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_Combo530([MarshalAs(UnmanagedType.LPStr)]string Label, out int CurrentItem, [MarshalAs(UnmanagedType.LPStr)]string ItemsSeparatedByZeros, int PopupMaxHeightInItems);

	/// <summary><code>IMGUI_API bool          Combo(const char* label, int* current_item, const char* items_separated_by_zeros, int popup_max_height_in_items = -1);      </code>
		/// Separate items with \0 within a string, end item-list with \0\0. e.g. "One\0Two\0Three\0" </summary>
	public static bool Combo(string Label, out int CurrentItem, string ItemsSeparatedByZeros)
	{
		return ImGui_Combo530(Label, out CurrentItem, ItemsSeparatedByZeros, -1);
	}

	/// <summary><code>IMGUI_API bool          Combo(const char* label, int* current_item, const char* items_separated_by_zeros, int popup_max_height_in_items = -1);      </code>
		/// Separate items with \0 within a string, end item-list with \0\0. e.g. "One\0Two\0Three\0" </summary>
	public static bool Combo(string Label, out int CurrentItem, string ItemsSeparatedByZeros, int PopupMaxHeightInItems)
	{
		return ImGui_Combo530(Label, out CurrentItem, ItemsSeparatedByZeros, PopupMaxHeightInItems);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_DragFloat545([MarshalAs(UnmanagedType.LPStr)]string Label, out float V, float VSpeed, float VMin, float VMax, [MarshalAs(UnmanagedType.LPStr)]string Format, ImGuiSliderFlags Flags);

	/// <summary><code>IMGUI_API bool          DragFloat(const char* label, float* v, float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", ImGuiSliderFlags flags = 0);     </code>
		/// If v_min >= v_max we have no bound </summary>
	public static bool DragFloat(string Label, out float V)
	{
		return ImGui_DragFloat545(Label, out V, (float)1.0f, (float)0.0f, (float)0.0f, "%.3f", (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragFloat(const char* label, float* v, float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", ImGuiSliderFlags flags = 0);     </code>
		/// If v_min >= v_max we have no bound </summary>
	public static bool DragFloat(string Label, out float V, float VSpeed)
	{
		return ImGui_DragFloat545(Label, out V, VSpeed, (float)0.0f, (float)0.0f, "%.3f", (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragFloat(const char* label, float* v, float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", ImGuiSliderFlags flags = 0);     </code>
		/// If v_min >= v_max we have no bound </summary>
	public static bool DragFloat(string Label, out float V, float VSpeed, float VMin)
	{
		return ImGui_DragFloat545(Label, out V, VSpeed, VMin, (float)0.0f, "%.3f", (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragFloat(const char* label, float* v, float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", ImGuiSliderFlags flags = 0);     </code>
		/// If v_min >= v_max we have no bound </summary>
	public static bool DragFloat(string Label, out float V, float VSpeed, float VMin, float VMax)
	{
		return ImGui_DragFloat545(Label, out V, VSpeed, VMin, VMax, "%.3f", (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragFloat(const char* label, float* v, float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", ImGuiSliderFlags flags = 0);     </code>
		/// If v_min >= v_max we have no bound </summary>
	public static bool DragFloat(string Label, out float V, float VSpeed, float VMin, float VMax, string Format)
	{
		return ImGui_DragFloat545(Label, out V, VSpeed, VMin, VMax, Format, (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragFloat(const char* label, float* v, float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", ImGuiSliderFlags flags = 0);     </code>
		/// If v_min >= v_max we have no bound </summary>
	public static bool DragFloat(string Label, out float V, float VSpeed, float VMin, float VMax, string Format, ImGuiSliderFlags Flags)
	{
		return ImGui_DragFloat545(Label, out V, VSpeed, VMin, VMax, Format, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_DragFloat2546([MarshalAs(UnmanagedType.LPStr)]string Label, out float V, float VSpeed, float VMin, float VMax, [MarshalAs(UnmanagedType.LPStr)]string Format, ImGuiSliderFlags Flags);

	/// <summary><code>IMGUI_API bool          DragFloat2(const char* label, float v[2], float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragFloat2(const char* label, float v[2], float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", ImGuiSliderFlags flags = 0); </summary>
	public static bool DragFloat2(string Label, out float V)
	{
		return ImGui_DragFloat2546(Label, out V, (float)1.0f, (float)0.0f, (float)0.0f, "%.3f", (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragFloat2(const char* label, float v[2], float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragFloat2(const char* label, float v[2], float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", ImGuiSliderFlags flags = 0); </summary>
	public static bool DragFloat2(string Label, out float V, float VSpeed)
	{
		return ImGui_DragFloat2546(Label, out V, VSpeed, (float)0.0f, (float)0.0f, "%.3f", (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragFloat2(const char* label, float v[2], float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragFloat2(const char* label, float v[2], float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", ImGuiSliderFlags flags = 0); </summary>
	public static bool DragFloat2(string Label, out float V, float VSpeed, float VMin)
	{
		return ImGui_DragFloat2546(Label, out V, VSpeed, VMin, (float)0.0f, "%.3f", (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragFloat2(const char* label, float v[2], float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragFloat2(const char* label, float v[2], float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", ImGuiSliderFlags flags = 0); </summary>
	public static bool DragFloat2(string Label, out float V, float VSpeed, float VMin, float VMax)
	{
		return ImGui_DragFloat2546(Label, out V, VSpeed, VMin, VMax, "%.3f", (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragFloat2(const char* label, float v[2], float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragFloat2(const char* label, float v[2], float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", ImGuiSliderFlags flags = 0); </summary>
	public static bool DragFloat2(string Label, out float V, float VSpeed, float VMin, float VMax, string Format)
	{
		return ImGui_DragFloat2546(Label, out V, VSpeed, VMin, VMax, Format, (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragFloat2(const char* label, float v[2], float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragFloat2(const char* label, float v[2], float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", ImGuiSliderFlags flags = 0); </summary>
	public static bool DragFloat2(string Label, out float V, float VSpeed, float VMin, float VMax, string Format, ImGuiSliderFlags Flags)
	{
		return ImGui_DragFloat2546(Label, out V, VSpeed, VMin, VMax, Format, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_DragFloat3547([MarshalAs(UnmanagedType.LPStr)]string Label, out float V, float VSpeed, float VMin, float VMax, [MarshalAs(UnmanagedType.LPStr)]string Format, ImGuiSliderFlags Flags);

	/// <summary><code>IMGUI_API bool          DragFloat3(const char* label, float v[3], float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragFloat3(const char* label, float v[3], float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", ImGuiSliderFlags flags = 0); </summary>
	public static bool DragFloat3(string Label, out float V)
	{
		return ImGui_DragFloat3547(Label, out V, (float)1.0f, (float)0.0f, (float)0.0f, "%.3f", (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragFloat3(const char* label, float v[3], float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragFloat3(const char* label, float v[3], float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", ImGuiSliderFlags flags = 0); </summary>
	public static bool DragFloat3(string Label, out float V, float VSpeed)
	{
		return ImGui_DragFloat3547(Label, out V, VSpeed, (float)0.0f, (float)0.0f, "%.3f", (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragFloat3(const char* label, float v[3], float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragFloat3(const char* label, float v[3], float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", ImGuiSliderFlags flags = 0); </summary>
	public static bool DragFloat3(string Label, out float V, float VSpeed, float VMin)
	{
		return ImGui_DragFloat3547(Label, out V, VSpeed, VMin, (float)0.0f, "%.3f", (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragFloat3(const char* label, float v[3], float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragFloat3(const char* label, float v[3], float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", ImGuiSliderFlags flags = 0); </summary>
	public static bool DragFloat3(string Label, out float V, float VSpeed, float VMin, float VMax)
	{
		return ImGui_DragFloat3547(Label, out V, VSpeed, VMin, VMax, "%.3f", (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragFloat3(const char* label, float v[3], float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragFloat3(const char* label, float v[3], float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", ImGuiSliderFlags flags = 0); </summary>
	public static bool DragFloat3(string Label, out float V, float VSpeed, float VMin, float VMax, string Format)
	{
		return ImGui_DragFloat3547(Label, out V, VSpeed, VMin, VMax, Format, (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragFloat3(const char* label, float v[3], float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragFloat3(const char* label, float v[3], float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", ImGuiSliderFlags flags = 0); </summary>
	public static bool DragFloat3(string Label, out float V, float VSpeed, float VMin, float VMax, string Format, ImGuiSliderFlags Flags)
	{
		return ImGui_DragFloat3547(Label, out V, VSpeed, VMin, VMax, Format, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_DragFloat4548([MarshalAs(UnmanagedType.LPStr)]string Label, out float V, float VSpeed, float VMin, float VMax, [MarshalAs(UnmanagedType.LPStr)]string Format, ImGuiSliderFlags Flags);

	/// <summary><code>IMGUI_API bool          DragFloat4(const char* label, float v[4], float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragFloat4(const char* label, float v[4], float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", ImGuiSliderFlags flags = 0); </summary>
	public static bool DragFloat4(string Label, out float V)
	{
		return ImGui_DragFloat4548(Label, out V, (float)1.0f, (float)0.0f, (float)0.0f, "%.3f", (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragFloat4(const char* label, float v[4], float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragFloat4(const char* label, float v[4], float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", ImGuiSliderFlags flags = 0); </summary>
	public static bool DragFloat4(string Label, out float V, float VSpeed)
	{
		return ImGui_DragFloat4548(Label, out V, VSpeed, (float)0.0f, (float)0.0f, "%.3f", (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragFloat4(const char* label, float v[4], float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragFloat4(const char* label, float v[4], float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", ImGuiSliderFlags flags = 0); </summary>
	public static bool DragFloat4(string Label, out float V, float VSpeed, float VMin)
	{
		return ImGui_DragFloat4548(Label, out V, VSpeed, VMin, (float)0.0f, "%.3f", (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragFloat4(const char* label, float v[4], float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragFloat4(const char* label, float v[4], float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", ImGuiSliderFlags flags = 0); </summary>
	public static bool DragFloat4(string Label, out float V, float VSpeed, float VMin, float VMax)
	{
		return ImGui_DragFloat4548(Label, out V, VSpeed, VMin, VMax, "%.3f", (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragFloat4(const char* label, float v[4], float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragFloat4(const char* label, float v[4], float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", ImGuiSliderFlags flags = 0); </summary>
	public static bool DragFloat4(string Label, out float V, float VSpeed, float VMin, float VMax, string Format)
	{
		return ImGui_DragFloat4548(Label, out V, VSpeed, VMin, VMax, Format, (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragFloat4(const char* label, float v[4], float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragFloat4(const char* label, float v[4], float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", ImGuiSliderFlags flags = 0); </summary>
	public static bool DragFloat4(string Label, out float V, float VSpeed, float VMin, float VMax, string Format, ImGuiSliderFlags Flags)
	{
		return ImGui_DragFloat4548(Label, out V, VSpeed, VMin, VMax, Format, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_DragFloatRange2549([MarshalAs(UnmanagedType.LPStr)]string Label, out float VCurrentMin, out float VCurrentMax, float VSpeed, float VMin, float VMax, [MarshalAs(UnmanagedType.LPStr)]string Format, [MarshalAs(UnmanagedType.LPStr)]string FormatMax, ImGuiSliderFlags Flags);

	/// <summary><code>IMGUI_API bool          DragFloatRange2(const char* label, float* v_current_min, float* v_current_max, float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", const char* format_max = NULL, ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragFloatRange2(const char* label, float* v_current_min, float* v_current_max, float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", const char* format_max = NULL, ImGuiSliderFlags flags = 0); </summary>
	public static bool DragFloatRange2(string Label, out float VCurrentMin, out float VCurrentMax)
	{
		return ImGui_DragFloatRange2549(Label, out VCurrentMin, out VCurrentMax, (float)1.0f, (float)0.0f, (float)0.0f, "%.3f", default, (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragFloatRange2(const char* label, float* v_current_min, float* v_current_max, float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", const char* format_max = NULL, ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragFloatRange2(const char* label, float* v_current_min, float* v_current_max, float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", const char* format_max = NULL, ImGuiSliderFlags flags = 0); </summary>
	public static bool DragFloatRange2(string Label, out float VCurrentMin, out float VCurrentMax, float VSpeed)
	{
		return ImGui_DragFloatRange2549(Label, out VCurrentMin, out VCurrentMax, VSpeed, (float)0.0f, (float)0.0f, "%.3f", default, (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragFloatRange2(const char* label, float* v_current_min, float* v_current_max, float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", const char* format_max = NULL, ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragFloatRange2(const char* label, float* v_current_min, float* v_current_max, float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", const char* format_max = NULL, ImGuiSliderFlags flags = 0); </summary>
	public static bool DragFloatRange2(string Label, out float VCurrentMin, out float VCurrentMax, float VSpeed, float VMin)
	{
		return ImGui_DragFloatRange2549(Label, out VCurrentMin, out VCurrentMax, VSpeed, VMin, (float)0.0f, "%.3f", default, (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragFloatRange2(const char* label, float* v_current_min, float* v_current_max, float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", const char* format_max = NULL, ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragFloatRange2(const char* label, float* v_current_min, float* v_current_max, float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", const char* format_max = NULL, ImGuiSliderFlags flags = 0); </summary>
	public static bool DragFloatRange2(string Label, out float VCurrentMin, out float VCurrentMax, float VSpeed, float VMin, float VMax)
	{
		return ImGui_DragFloatRange2549(Label, out VCurrentMin, out VCurrentMax, VSpeed, VMin, VMax, "%.3f", default, (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragFloatRange2(const char* label, float* v_current_min, float* v_current_max, float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", const char* format_max = NULL, ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragFloatRange2(const char* label, float* v_current_min, float* v_current_max, float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", const char* format_max = NULL, ImGuiSliderFlags flags = 0); </summary>
	public static bool DragFloatRange2(string Label, out float VCurrentMin, out float VCurrentMax, float VSpeed, float VMin, float VMax, string Format)
	{
		return ImGui_DragFloatRange2549(Label, out VCurrentMin, out VCurrentMax, VSpeed, VMin, VMax, Format, default, (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragFloatRange2(const char* label, float* v_current_min, float* v_current_max, float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", const char* format_max = NULL, ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragFloatRange2(const char* label, float* v_current_min, float* v_current_max, float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", const char* format_max = NULL, ImGuiSliderFlags flags = 0); </summary>
	public static bool DragFloatRange2(string Label, out float VCurrentMin, out float VCurrentMax, float VSpeed, float VMin, float VMax, string Format, string FormatMax)
	{
		return ImGui_DragFloatRange2549(Label, out VCurrentMin, out VCurrentMax, VSpeed, VMin, VMax, Format, FormatMax, (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragFloatRange2(const char* label, float* v_current_min, float* v_current_max, float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", const char* format_max = NULL, ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragFloatRange2(const char* label, float* v_current_min, float* v_current_max, float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, const char* format = "%.3f", const char* format_max = NULL, ImGuiSliderFlags flags = 0); </summary>
	public static bool DragFloatRange2(string Label, out float VCurrentMin, out float VCurrentMax, float VSpeed, float VMin, float VMax, string Format, string FormatMax, ImGuiSliderFlags Flags)
	{
		return ImGui_DragFloatRange2549(Label, out VCurrentMin, out VCurrentMax, VSpeed, VMin, VMax, Format, FormatMax, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_DragInt550([MarshalAs(UnmanagedType.LPStr)]string Label, out int V, float VSpeed, int VMin, int VMax, [MarshalAs(UnmanagedType.LPStr)]string Format, ImGuiSliderFlags Flags);

	/// <summary><code>IMGUI_API bool          DragInt(const char* label, int* v, float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", ImGuiSliderFlags flags = 0);  </code>
		/// If v_min >= v_max we have no bound </summary>
	public static bool DragInt(string Label, out int V)
	{
		return ImGui_DragInt550(Label, out V, (float)1.0f, (int)0, (int)0, "%d", (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragInt(const char* label, int* v, float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", ImGuiSliderFlags flags = 0);  </code>
		/// If v_min >= v_max we have no bound </summary>
	public static bool DragInt(string Label, out int V, float VSpeed)
	{
		return ImGui_DragInt550(Label, out V, VSpeed, (int)0, (int)0, "%d", (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragInt(const char* label, int* v, float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", ImGuiSliderFlags flags = 0);  </code>
		/// If v_min >= v_max we have no bound </summary>
	public static bool DragInt(string Label, out int V, float VSpeed, int VMin)
	{
		return ImGui_DragInt550(Label, out V, VSpeed, VMin, (int)0, "%d", (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragInt(const char* label, int* v, float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", ImGuiSliderFlags flags = 0);  </code>
		/// If v_min >= v_max we have no bound </summary>
	public static bool DragInt(string Label, out int V, float VSpeed, int VMin, int VMax)
	{
		return ImGui_DragInt550(Label, out V, VSpeed, VMin, VMax, "%d", (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragInt(const char* label, int* v, float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", ImGuiSliderFlags flags = 0);  </code>
		/// If v_min >= v_max we have no bound </summary>
	public static bool DragInt(string Label, out int V, float VSpeed, int VMin, int VMax, string Format)
	{
		return ImGui_DragInt550(Label, out V, VSpeed, VMin, VMax, Format, (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragInt(const char* label, int* v, float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", ImGuiSliderFlags flags = 0);  </code>
		/// If v_min >= v_max we have no bound </summary>
	public static bool DragInt(string Label, out int V, float VSpeed, int VMin, int VMax, string Format, ImGuiSliderFlags Flags)
	{
		return ImGui_DragInt550(Label, out V, VSpeed, VMin, VMax, Format, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_DragInt2551([MarshalAs(UnmanagedType.LPStr)]string Label, out int V, float VSpeed, int VMin, int VMax, [MarshalAs(UnmanagedType.LPStr)]string Format, ImGuiSliderFlags Flags);

	/// <summary><code>IMGUI_API bool          DragInt2(const char* label, int v[2], float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragInt2(const char* label, int v[2], float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", ImGuiSliderFlags flags = 0); </summary>
	public static bool DragInt2(string Label, out int V)
	{
		return ImGui_DragInt2551(Label, out V, (float)1.0f, (int)0, (int)0, "%d", (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragInt2(const char* label, int v[2], float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragInt2(const char* label, int v[2], float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", ImGuiSliderFlags flags = 0); </summary>
	public static bool DragInt2(string Label, out int V, float VSpeed)
	{
		return ImGui_DragInt2551(Label, out V, VSpeed, (int)0, (int)0, "%d", (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragInt2(const char* label, int v[2], float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragInt2(const char* label, int v[2], float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", ImGuiSliderFlags flags = 0); </summary>
	public static bool DragInt2(string Label, out int V, float VSpeed, int VMin)
	{
		return ImGui_DragInt2551(Label, out V, VSpeed, VMin, (int)0, "%d", (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragInt2(const char* label, int v[2], float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragInt2(const char* label, int v[2], float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", ImGuiSliderFlags flags = 0); </summary>
	public static bool DragInt2(string Label, out int V, float VSpeed, int VMin, int VMax)
	{
		return ImGui_DragInt2551(Label, out V, VSpeed, VMin, VMax, "%d", (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragInt2(const char* label, int v[2], float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragInt2(const char* label, int v[2], float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", ImGuiSliderFlags flags = 0); </summary>
	public static bool DragInt2(string Label, out int V, float VSpeed, int VMin, int VMax, string Format)
	{
		return ImGui_DragInt2551(Label, out V, VSpeed, VMin, VMax, Format, (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragInt2(const char* label, int v[2], float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragInt2(const char* label, int v[2], float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", ImGuiSliderFlags flags = 0); </summary>
	public static bool DragInt2(string Label, out int V, float VSpeed, int VMin, int VMax, string Format, ImGuiSliderFlags Flags)
	{
		return ImGui_DragInt2551(Label, out V, VSpeed, VMin, VMax, Format, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_DragInt3552([MarshalAs(UnmanagedType.LPStr)]string Label, out int V, float VSpeed, int VMin, int VMax, [MarshalAs(UnmanagedType.LPStr)]string Format, ImGuiSliderFlags Flags);

	/// <summary><code>IMGUI_API bool          DragInt3(const char* label, int v[3], float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragInt3(const char* label, int v[3], float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", ImGuiSliderFlags flags = 0); </summary>
	public static bool DragInt3(string Label, out int V)
	{
		return ImGui_DragInt3552(Label, out V, (float)1.0f, (int)0, (int)0, "%d", (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragInt3(const char* label, int v[3], float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragInt3(const char* label, int v[3], float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", ImGuiSliderFlags flags = 0); </summary>
	public static bool DragInt3(string Label, out int V, float VSpeed)
	{
		return ImGui_DragInt3552(Label, out V, VSpeed, (int)0, (int)0, "%d", (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragInt3(const char* label, int v[3], float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragInt3(const char* label, int v[3], float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", ImGuiSliderFlags flags = 0); </summary>
	public static bool DragInt3(string Label, out int V, float VSpeed, int VMin)
	{
		return ImGui_DragInt3552(Label, out V, VSpeed, VMin, (int)0, "%d", (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragInt3(const char* label, int v[3], float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragInt3(const char* label, int v[3], float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", ImGuiSliderFlags flags = 0); </summary>
	public static bool DragInt3(string Label, out int V, float VSpeed, int VMin, int VMax)
	{
		return ImGui_DragInt3552(Label, out V, VSpeed, VMin, VMax, "%d", (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragInt3(const char* label, int v[3], float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragInt3(const char* label, int v[3], float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", ImGuiSliderFlags flags = 0); </summary>
	public static bool DragInt3(string Label, out int V, float VSpeed, int VMin, int VMax, string Format)
	{
		return ImGui_DragInt3552(Label, out V, VSpeed, VMin, VMax, Format, (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragInt3(const char* label, int v[3], float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragInt3(const char* label, int v[3], float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", ImGuiSliderFlags flags = 0); </summary>
	public static bool DragInt3(string Label, out int V, float VSpeed, int VMin, int VMax, string Format, ImGuiSliderFlags Flags)
	{
		return ImGui_DragInt3552(Label, out V, VSpeed, VMin, VMax, Format, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_DragInt4553([MarshalAs(UnmanagedType.LPStr)]string Label, out int V, float VSpeed, int VMin, int VMax, [MarshalAs(UnmanagedType.LPStr)]string Format, ImGuiSliderFlags Flags);

	/// <summary><code>IMGUI_API bool          DragInt4(const char* label, int v[4], float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragInt4(const char* label, int v[4], float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", ImGuiSliderFlags flags = 0); </summary>
	public static bool DragInt4(string Label, out int V)
	{
		return ImGui_DragInt4553(Label, out V, (float)1.0f, (int)0, (int)0, "%d", (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragInt4(const char* label, int v[4], float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragInt4(const char* label, int v[4], float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", ImGuiSliderFlags flags = 0); </summary>
	public static bool DragInt4(string Label, out int V, float VSpeed)
	{
		return ImGui_DragInt4553(Label, out V, VSpeed, (int)0, (int)0, "%d", (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragInt4(const char* label, int v[4], float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragInt4(const char* label, int v[4], float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", ImGuiSliderFlags flags = 0); </summary>
	public static bool DragInt4(string Label, out int V, float VSpeed, int VMin)
	{
		return ImGui_DragInt4553(Label, out V, VSpeed, VMin, (int)0, "%d", (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragInt4(const char* label, int v[4], float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragInt4(const char* label, int v[4], float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", ImGuiSliderFlags flags = 0); </summary>
	public static bool DragInt4(string Label, out int V, float VSpeed, int VMin, int VMax)
	{
		return ImGui_DragInt4553(Label, out V, VSpeed, VMin, VMax, "%d", (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragInt4(const char* label, int v[4], float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragInt4(const char* label, int v[4], float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", ImGuiSliderFlags flags = 0); </summary>
	public static bool DragInt4(string Label, out int V, float VSpeed, int VMin, int VMax, string Format)
	{
		return ImGui_DragInt4553(Label, out V, VSpeed, VMin, VMax, Format, (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragInt4(const char* label, int v[4], float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragInt4(const char* label, int v[4], float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", ImGuiSliderFlags flags = 0); </summary>
	public static bool DragInt4(string Label, out int V, float VSpeed, int VMin, int VMax, string Format, ImGuiSliderFlags Flags)
	{
		return ImGui_DragInt4553(Label, out V, VSpeed, VMin, VMax, Format, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_DragIntRange2554([MarshalAs(UnmanagedType.LPStr)]string Label, out int VCurrentMin, out int VCurrentMax, float VSpeed, int VMin, int VMax, [MarshalAs(UnmanagedType.LPStr)]string Format, [MarshalAs(UnmanagedType.LPStr)]string FormatMax, ImGuiSliderFlags Flags);

	/// <summary><code>IMGUI_API bool          DragIntRange2(const char* label, int* v_current_min, int* v_current_max, float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", const char* format_max = NULL, ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragIntRange2(const char* label, int* v_current_min, int* v_current_max, float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", const char* format_max = NULL, ImGuiSliderFlags flags = 0); </summary>
	public static bool DragIntRange2(string Label, out int VCurrentMin, out int VCurrentMax)
	{
		return ImGui_DragIntRange2554(Label, out VCurrentMin, out VCurrentMax, (float)1.0f, (int)0, (int)0, "%d", default, (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragIntRange2(const char* label, int* v_current_min, int* v_current_max, float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", const char* format_max = NULL, ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragIntRange2(const char* label, int* v_current_min, int* v_current_max, float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", const char* format_max = NULL, ImGuiSliderFlags flags = 0); </summary>
	public static bool DragIntRange2(string Label, out int VCurrentMin, out int VCurrentMax, float VSpeed)
	{
		return ImGui_DragIntRange2554(Label, out VCurrentMin, out VCurrentMax, VSpeed, (int)0, (int)0, "%d", default, (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragIntRange2(const char* label, int* v_current_min, int* v_current_max, float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", const char* format_max = NULL, ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragIntRange2(const char* label, int* v_current_min, int* v_current_max, float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", const char* format_max = NULL, ImGuiSliderFlags flags = 0); </summary>
	public static bool DragIntRange2(string Label, out int VCurrentMin, out int VCurrentMax, float VSpeed, int VMin)
	{
		return ImGui_DragIntRange2554(Label, out VCurrentMin, out VCurrentMax, VSpeed, VMin, (int)0, "%d", default, (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragIntRange2(const char* label, int* v_current_min, int* v_current_max, float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", const char* format_max = NULL, ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragIntRange2(const char* label, int* v_current_min, int* v_current_max, float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", const char* format_max = NULL, ImGuiSliderFlags flags = 0); </summary>
	public static bool DragIntRange2(string Label, out int VCurrentMin, out int VCurrentMax, float VSpeed, int VMin, int VMax)
	{
		return ImGui_DragIntRange2554(Label, out VCurrentMin, out VCurrentMax, VSpeed, VMin, VMax, "%d", default, (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragIntRange2(const char* label, int* v_current_min, int* v_current_max, float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", const char* format_max = NULL, ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragIntRange2(const char* label, int* v_current_min, int* v_current_max, float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", const char* format_max = NULL, ImGuiSliderFlags flags = 0); </summary>
	public static bool DragIntRange2(string Label, out int VCurrentMin, out int VCurrentMax, float VSpeed, int VMin, int VMax, string Format)
	{
		return ImGui_DragIntRange2554(Label, out VCurrentMin, out VCurrentMax, VSpeed, VMin, VMax, Format, default, (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragIntRange2(const char* label, int* v_current_min, int* v_current_max, float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", const char* format_max = NULL, ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragIntRange2(const char* label, int* v_current_min, int* v_current_max, float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", const char* format_max = NULL, ImGuiSliderFlags flags = 0); </summary>
	public static bool DragIntRange2(string Label, out int VCurrentMin, out int VCurrentMax, float VSpeed, int VMin, int VMax, string Format, string FormatMax)
	{
		return ImGui_DragIntRange2554(Label, out VCurrentMin, out VCurrentMax, VSpeed, VMin, VMax, Format, FormatMax, (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragIntRange2(const char* label, int* v_current_min, int* v_current_max, float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", const char* format_max = NULL, ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragIntRange2(const char* label, int* v_current_min, int* v_current_max, float v_speed = 1.0f, int v_min = 0, int v_max = 0, const char* format = "%d", const char* format_max = NULL, ImGuiSliderFlags flags = 0); </summary>
	public static bool DragIntRange2(string Label, out int VCurrentMin, out int VCurrentMax, float VSpeed, int VMin, int VMax, string Format, string FormatMax, ImGuiSliderFlags Flags)
	{
		return ImGui_DragIntRange2554(Label, out VCurrentMin, out VCurrentMax, VSpeed, VMin, VMax, Format, FormatMax, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_DragScalar555([MarshalAs(UnmanagedType.LPStr)]string Label, ImGuiDataType DataType, IntPtr PData, float VSpeed,  IntPtr PMin,  IntPtr PMax, [MarshalAs(UnmanagedType.LPStr)]string Format, ImGuiSliderFlags Flags);

	/// <summary><code>IMGUI_API bool          DragScalar(const char* label, ImGuiDataType data_type, void* p_data, float v_speed = 1.0f, const void* p_min = NULL, const void* p_max = NULL, const char* format = NULL, ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragScalar(const char* label, ImGuiDataType data_type, void* p_data, float v_speed = 1.0f, const void* p_min = NULL, const void* p_max = NULL, const char* format = NULL, ImGuiSliderFlags flags = 0); </summary>
	public static bool DragScalar(string Label, ImGuiDataType DataType, IntPtr PData)
	{
		return ImGui_DragScalar555(Label, DataType, PData, (float)1.0f, default, default, default, (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragScalar(const char* label, ImGuiDataType data_type, void* p_data, float v_speed = 1.0f, const void* p_min = NULL, const void* p_max = NULL, const char* format = NULL, ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragScalar(const char* label, ImGuiDataType data_type, void* p_data, float v_speed = 1.0f, const void* p_min = NULL, const void* p_max = NULL, const char* format = NULL, ImGuiSliderFlags flags = 0); </summary>
	public static bool DragScalar(string Label, ImGuiDataType DataType, IntPtr PData, float VSpeed)
	{
		return ImGui_DragScalar555(Label, DataType, PData, VSpeed, default, default, default, (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragScalar(const char* label, ImGuiDataType data_type, void* p_data, float v_speed = 1.0f, const void* p_min = NULL, const void* p_max = NULL, const char* format = NULL, ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragScalar(const char* label, ImGuiDataType data_type, void* p_data, float v_speed = 1.0f, const void* p_min = NULL, const void* p_max = NULL, const char* format = NULL, ImGuiSliderFlags flags = 0); </summary>
	public static bool DragScalar(string Label, ImGuiDataType DataType, IntPtr PData, float VSpeed,  IntPtr PMin)
	{
		return ImGui_DragScalar555(Label, DataType, PData, VSpeed, PMin, default, default, (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragScalar(const char* label, ImGuiDataType data_type, void* p_data, float v_speed = 1.0f, const void* p_min = NULL, const void* p_max = NULL, const char* format = NULL, ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragScalar(const char* label, ImGuiDataType data_type, void* p_data, float v_speed = 1.0f, const void* p_min = NULL, const void* p_max = NULL, const char* format = NULL, ImGuiSliderFlags flags = 0); </summary>
	public static bool DragScalar(string Label, ImGuiDataType DataType, IntPtr PData, float VSpeed,  IntPtr PMin,  IntPtr PMax)
	{
		return ImGui_DragScalar555(Label, DataType, PData, VSpeed, PMin, PMax, default, (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragScalar(const char* label, ImGuiDataType data_type, void* p_data, float v_speed = 1.0f, const void* p_min = NULL, const void* p_max = NULL, const char* format = NULL, ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragScalar(const char* label, ImGuiDataType data_type, void* p_data, float v_speed = 1.0f, const void* p_min = NULL, const void* p_max = NULL, const char* format = NULL, ImGuiSliderFlags flags = 0); </summary>
	public static bool DragScalar(string Label, ImGuiDataType DataType, IntPtr PData, float VSpeed,  IntPtr PMin,  IntPtr PMax, string Format)
	{
		return ImGui_DragScalar555(Label, DataType, PData, VSpeed, PMin, PMax, Format, (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragScalar(const char* label, ImGuiDataType data_type, void* p_data, float v_speed = 1.0f, const void* p_min = NULL, const void* p_max = NULL, const char* format = NULL, ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragScalar(const char* label, ImGuiDataType data_type, void* p_data, float v_speed = 1.0f, const void* p_min = NULL, const void* p_max = NULL, const char* format = NULL, ImGuiSliderFlags flags = 0); </summary>
	public static bool DragScalar(string Label, ImGuiDataType DataType, IntPtr PData, float VSpeed,  IntPtr PMin,  IntPtr PMax, string Format, ImGuiSliderFlags Flags)
	{
		return ImGui_DragScalar555(Label, DataType, PData, VSpeed, PMin, PMax, Format, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_DragScalarN556([MarshalAs(UnmanagedType.LPStr)]string Label, ImGuiDataType DataType, IntPtr PData, int Components, float VSpeed,  IntPtr PMin,  IntPtr PMax, [MarshalAs(UnmanagedType.LPStr)]string Format, ImGuiSliderFlags Flags);

	/// <summary><code>IMGUI_API bool          DragScalarN(const char* label, ImGuiDataType data_type, void* p_data, int components, float v_speed = 1.0f, const void* p_min = NULL, const void* p_max = NULL, const char* format = NULL, ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragScalarN(const char* label, ImGuiDataType data_type, void* p_data, int components, float v_speed = 1.0f, const void* p_min = NULL, const void* p_max = NULL, const char* format = NULL, ImGuiSliderFlags flags = 0); </summary>
	public static bool DragScalarN(string Label, ImGuiDataType DataType, IntPtr PData, int Components)
	{
		return ImGui_DragScalarN556(Label, DataType, PData, Components, (float)1.0f, default, default, default, (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragScalarN(const char* label, ImGuiDataType data_type, void* p_data, int components, float v_speed = 1.0f, const void* p_min = NULL, const void* p_max = NULL, const char* format = NULL, ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragScalarN(const char* label, ImGuiDataType data_type, void* p_data, int components, float v_speed = 1.0f, const void* p_min = NULL, const void* p_max = NULL, const char* format = NULL, ImGuiSliderFlags flags = 0); </summary>
	public static bool DragScalarN(string Label, ImGuiDataType DataType, IntPtr PData, int Components, float VSpeed)
	{
		return ImGui_DragScalarN556(Label, DataType, PData, Components, VSpeed, default, default, default, (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragScalarN(const char* label, ImGuiDataType data_type, void* p_data, int components, float v_speed = 1.0f, const void* p_min = NULL, const void* p_max = NULL, const char* format = NULL, ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragScalarN(const char* label, ImGuiDataType data_type, void* p_data, int components, float v_speed = 1.0f, const void* p_min = NULL, const void* p_max = NULL, const char* format = NULL, ImGuiSliderFlags flags = 0); </summary>
	public static bool DragScalarN(string Label, ImGuiDataType DataType, IntPtr PData, int Components, float VSpeed,  IntPtr PMin)
	{
		return ImGui_DragScalarN556(Label, DataType, PData, Components, VSpeed, PMin, default, default, (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragScalarN(const char* label, ImGuiDataType data_type, void* p_data, int components, float v_speed = 1.0f, const void* p_min = NULL, const void* p_max = NULL, const char* format = NULL, ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragScalarN(const char* label, ImGuiDataType data_type, void* p_data, int components, float v_speed = 1.0f, const void* p_min = NULL, const void* p_max = NULL, const char* format = NULL, ImGuiSliderFlags flags = 0); </summary>
	public static bool DragScalarN(string Label, ImGuiDataType DataType, IntPtr PData, int Components, float VSpeed,  IntPtr PMin,  IntPtr PMax)
	{
		return ImGui_DragScalarN556(Label, DataType, PData, Components, VSpeed, PMin, PMax, default, (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragScalarN(const char* label, ImGuiDataType data_type, void* p_data, int components, float v_speed = 1.0f, const void* p_min = NULL, const void* p_max = NULL, const char* format = NULL, ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragScalarN(const char* label, ImGuiDataType data_type, void* p_data, int components, float v_speed = 1.0f, const void* p_min = NULL, const void* p_max = NULL, const char* format = NULL, ImGuiSliderFlags flags = 0); </summary>
	public static bool DragScalarN(string Label, ImGuiDataType DataType, IntPtr PData, int Components, float VSpeed,  IntPtr PMin,  IntPtr PMax, string Format)
	{
		return ImGui_DragScalarN556(Label, DataType, PData, Components, VSpeed, PMin, PMax, Format, (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          DragScalarN(const char* label, ImGuiDataType data_type, void* p_data, int components, float v_speed = 1.0f, const void* p_min = NULL, const void* p_max = NULL, const char* format = NULL, ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          DragScalarN(const char* label, ImGuiDataType data_type, void* p_data, int components, float v_speed = 1.0f, const void* p_min = NULL, const void* p_max = NULL, const char* format = NULL, ImGuiSliderFlags flags = 0); </summary>
	public static bool DragScalarN(string Label, ImGuiDataType DataType, IntPtr PData, int Components, float VSpeed,  IntPtr PMin,  IntPtr PMax, string Format, ImGuiSliderFlags Flags)
	{
		return ImGui_DragScalarN556(Label, DataType, PData, Components, VSpeed, PMin, PMax, Format, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_SliderFloat564([MarshalAs(UnmanagedType.LPStr)]string Label, out float V, float VMin, float VMax, [MarshalAs(UnmanagedType.LPStr)]string Format, ImGuiSliderFlags Flags);

	/// <summary><code>IMGUI_API bool          SliderFloat(const char* label, float* v, float v_min, float v_max, const char* format = "%.3f", ImGuiSliderFlags flags = 0);     </code>
		/// adjust format to decorate the value with a prefix or a suffix for in-slider labels or unit display. </summary>
	public static bool SliderFloat(string Label, out float V, float VMin, float VMax)
	{
		return ImGui_SliderFloat564(Label, out V, VMin, VMax, "%.3f", (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          SliderFloat(const char* label, float* v, float v_min, float v_max, const char* format = "%.3f", ImGuiSliderFlags flags = 0);     </code>
		/// adjust format to decorate the value with a prefix or a suffix for in-slider labels or unit display. </summary>
	public static bool SliderFloat(string Label, out float V, float VMin, float VMax, string Format)
	{
		return ImGui_SliderFloat564(Label, out V, VMin, VMax, Format, (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          SliderFloat(const char* label, float* v, float v_min, float v_max, const char* format = "%.3f", ImGuiSliderFlags flags = 0);     </code>
		/// adjust format to decorate the value with a prefix or a suffix for in-slider labels or unit display. </summary>
	public static bool SliderFloat(string Label, out float V, float VMin, float VMax, string Format, ImGuiSliderFlags Flags)
	{
		return ImGui_SliderFloat564(Label, out V, VMin, VMax, Format, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_SliderFloat2565([MarshalAs(UnmanagedType.LPStr)]string Label, out float V, float VMin, float VMax, [MarshalAs(UnmanagedType.LPStr)]string Format, ImGuiSliderFlags Flags);

	/// <summary><code>IMGUI_API bool          SliderFloat2(const char* label, float v[2], float v_min, float v_max, const char* format = "%.3f", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          SliderFloat2(const char* label, float v[2], float v_min, float v_max, const char* format = "%.3f", ImGuiSliderFlags flags = 0); </summary>
	public static bool SliderFloat2(string Label, out float V, float VMin, float VMax)
	{
		return ImGui_SliderFloat2565(Label, out V, VMin, VMax, "%.3f", (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          SliderFloat2(const char* label, float v[2], float v_min, float v_max, const char* format = "%.3f", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          SliderFloat2(const char* label, float v[2], float v_min, float v_max, const char* format = "%.3f", ImGuiSliderFlags flags = 0); </summary>
	public static bool SliderFloat2(string Label, out float V, float VMin, float VMax, string Format)
	{
		return ImGui_SliderFloat2565(Label, out V, VMin, VMax, Format, (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          SliderFloat2(const char* label, float v[2], float v_min, float v_max, const char* format = "%.3f", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          SliderFloat2(const char* label, float v[2], float v_min, float v_max, const char* format = "%.3f", ImGuiSliderFlags flags = 0); </summary>
	public static bool SliderFloat2(string Label, out float V, float VMin, float VMax, string Format, ImGuiSliderFlags Flags)
	{
		return ImGui_SliderFloat2565(Label, out V, VMin, VMax, Format, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_SliderFloat3566([MarshalAs(UnmanagedType.LPStr)]string Label, out float V, float VMin, float VMax, [MarshalAs(UnmanagedType.LPStr)]string Format, ImGuiSliderFlags Flags);

	/// <summary><code>IMGUI_API bool          SliderFloat3(const char* label, float v[3], float v_min, float v_max, const char* format = "%.3f", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          SliderFloat3(const char* label, float v[3], float v_min, float v_max, const char* format = "%.3f", ImGuiSliderFlags flags = 0); </summary>
	public static bool SliderFloat3(string Label, out float V, float VMin, float VMax)
	{
		return ImGui_SliderFloat3566(Label, out V, VMin, VMax, "%.3f", (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          SliderFloat3(const char* label, float v[3], float v_min, float v_max, const char* format = "%.3f", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          SliderFloat3(const char* label, float v[3], float v_min, float v_max, const char* format = "%.3f", ImGuiSliderFlags flags = 0); </summary>
	public static bool SliderFloat3(string Label, out float V, float VMin, float VMax, string Format)
	{
		return ImGui_SliderFloat3566(Label, out V, VMin, VMax, Format, (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          SliderFloat3(const char* label, float v[3], float v_min, float v_max, const char* format = "%.3f", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          SliderFloat3(const char* label, float v[3], float v_min, float v_max, const char* format = "%.3f", ImGuiSliderFlags flags = 0); </summary>
	public static bool SliderFloat3(string Label, out float V, float VMin, float VMax, string Format, ImGuiSliderFlags Flags)
	{
		return ImGui_SliderFloat3566(Label, out V, VMin, VMax, Format, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_SliderFloat4567([MarshalAs(UnmanagedType.LPStr)]string Label, out float V, float VMin, float VMax, [MarshalAs(UnmanagedType.LPStr)]string Format, ImGuiSliderFlags Flags);

	/// <summary><code>IMGUI_API bool          SliderFloat4(const char* label, float v[4], float v_min, float v_max, const char* format = "%.3f", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          SliderFloat4(const char* label, float v[4], float v_min, float v_max, const char* format = "%.3f", ImGuiSliderFlags flags = 0); </summary>
	public static bool SliderFloat4(string Label, out float V, float VMin, float VMax)
	{
		return ImGui_SliderFloat4567(Label, out V, VMin, VMax, "%.3f", (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          SliderFloat4(const char* label, float v[4], float v_min, float v_max, const char* format = "%.3f", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          SliderFloat4(const char* label, float v[4], float v_min, float v_max, const char* format = "%.3f", ImGuiSliderFlags flags = 0); </summary>
	public static bool SliderFloat4(string Label, out float V, float VMin, float VMax, string Format)
	{
		return ImGui_SliderFloat4567(Label, out V, VMin, VMax, Format, (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          SliderFloat4(const char* label, float v[4], float v_min, float v_max, const char* format = "%.3f", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          SliderFloat4(const char* label, float v[4], float v_min, float v_max, const char* format = "%.3f", ImGuiSliderFlags flags = 0); </summary>
	public static bool SliderFloat4(string Label, out float V, float VMin, float VMax, string Format, ImGuiSliderFlags Flags)
	{
		return ImGui_SliderFloat4567(Label, out V, VMin, VMax, Format, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_SliderAngle568([MarshalAs(UnmanagedType.LPStr)]string Label, out float VRad, float VDegreesMin, float VDegreesMax, [MarshalAs(UnmanagedType.LPStr)]string Format, ImGuiSliderFlags Flags);

	/// <summary><code>IMGUI_API bool          SliderAngle(const char* label, float* v_rad, float v_degrees_min = -360.0f, float v_degrees_max = +360.0f, const char* format = "%.0f deg", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          SliderAngle(const char* label, float* v_rad, float v_degrees_min = -360.0f, float v_degrees_max = +360.0f, const char* format = "%.0f deg", ImGuiSliderFlags flags = 0); </summary>
	public static bool SliderAngle(string Label, out float VRad)
	{
		return ImGui_SliderAngle568(Label, out VRad, -360.0f, +360.0f, "%.0f deg", (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          SliderAngle(const char* label, float* v_rad, float v_degrees_min = -360.0f, float v_degrees_max = +360.0f, const char* format = "%.0f deg", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          SliderAngle(const char* label, float* v_rad, float v_degrees_min = -360.0f, float v_degrees_max = +360.0f, const char* format = "%.0f deg", ImGuiSliderFlags flags = 0); </summary>
	public static bool SliderAngle(string Label, out float VRad, float VDegreesMin)
	{
		return ImGui_SliderAngle568(Label, out VRad, VDegreesMin, +360.0f, "%.0f deg", (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          SliderAngle(const char* label, float* v_rad, float v_degrees_min = -360.0f, float v_degrees_max = +360.0f, const char* format = "%.0f deg", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          SliderAngle(const char* label, float* v_rad, float v_degrees_min = -360.0f, float v_degrees_max = +360.0f, const char* format = "%.0f deg", ImGuiSliderFlags flags = 0); </summary>
	public static bool SliderAngle(string Label, out float VRad, float VDegreesMin, float VDegreesMax)
	{
		return ImGui_SliderAngle568(Label, out VRad, VDegreesMin, VDegreesMax, "%.0f deg", (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          SliderAngle(const char* label, float* v_rad, float v_degrees_min = -360.0f, float v_degrees_max = +360.0f, const char* format = "%.0f deg", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          SliderAngle(const char* label, float* v_rad, float v_degrees_min = -360.0f, float v_degrees_max = +360.0f, const char* format = "%.0f deg", ImGuiSliderFlags flags = 0); </summary>
	public static bool SliderAngle(string Label, out float VRad, float VDegreesMin, float VDegreesMax, string Format)
	{
		return ImGui_SliderAngle568(Label, out VRad, VDegreesMin, VDegreesMax, Format, (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          SliderAngle(const char* label, float* v_rad, float v_degrees_min = -360.0f, float v_degrees_max = +360.0f, const char* format = "%.0f deg", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          SliderAngle(const char* label, float* v_rad, float v_degrees_min = -360.0f, float v_degrees_max = +360.0f, const char* format = "%.0f deg", ImGuiSliderFlags flags = 0); </summary>
	public static bool SliderAngle(string Label, out float VRad, float VDegreesMin, float VDegreesMax, string Format, ImGuiSliderFlags Flags)
	{
		return ImGui_SliderAngle568(Label, out VRad, VDegreesMin, VDegreesMax, Format, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_SliderInt569([MarshalAs(UnmanagedType.LPStr)]string Label, out int V, int VMin, int VMax, [MarshalAs(UnmanagedType.LPStr)]string Format, ImGuiSliderFlags Flags);

	/// <summary><code>IMGUI_API bool          SliderInt(const char* label, int* v, int v_min, int v_max, const char* format = "%d", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          SliderInt(const char* label, int* v, int v_min, int v_max, const char* format = "%d", ImGuiSliderFlags flags = 0); </summary>
	public static bool SliderInt(string Label, out int V, int VMin, int VMax)
	{
		return ImGui_SliderInt569(Label, out V, VMin, VMax, "%d", (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          SliderInt(const char* label, int* v, int v_min, int v_max, const char* format = "%d", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          SliderInt(const char* label, int* v, int v_min, int v_max, const char* format = "%d", ImGuiSliderFlags flags = 0); </summary>
	public static bool SliderInt(string Label, out int V, int VMin, int VMax, string Format)
	{
		return ImGui_SliderInt569(Label, out V, VMin, VMax, Format, (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          SliderInt(const char* label, int* v, int v_min, int v_max, const char* format = "%d", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          SliderInt(const char* label, int* v, int v_min, int v_max, const char* format = "%d", ImGuiSliderFlags flags = 0); </summary>
	public static bool SliderInt(string Label, out int V, int VMin, int VMax, string Format, ImGuiSliderFlags Flags)
	{
		return ImGui_SliderInt569(Label, out V, VMin, VMax, Format, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_SliderInt2570([MarshalAs(UnmanagedType.LPStr)]string Label, out int V, int VMin, int VMax, [MarshalAs(UnmanagedType.LPStr)]string Format, ImGuiSliderFlags Flags);

	/// <summary><code>IMGUI_API bool          SliderInt2(const char* label, int v[2], int v_min, int v_max, const char* format = "%d", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          SliderInt2(const char* label, int v[2], int v_min, int v_max, const char* format = "%d", ImGuiSliderFlags flags = 0); </summary>
	public static bool SliderInt2(string Label, out int V, int VMin, int VMax)
	{
		return ImGui_SliderInt2570(Label, out V, VMin, VMax, "%d", (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          SliderInt2(const char* label, int v[2], int v_min, int v_max, const char* format = "%d", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          SliderInt2(const char* label, int v[2], int v_min, int v_max, const char* format = "%d", ImGuiSliderFlags flags = 0); </summary>
	public static bool SliderInt2(string Label, out int V, int VMin, int VMax, string Format)
	{
		return ImGui_SliderInt2570(Label, out V, VMin, VMax, Format, (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          SliderInt2(const char* label, int v[2], int v_min, int v_max, const char* format = "%d", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          SliderInt2(const char* label, int v[2], int v_min, int v_max, const char* format = "%d", ImGuiSliderFlags flags = 0); </summary>
	public static bool SliderInt2(string Label, out int V, int VMin, int VMax, string Format, ImGuiSliderFlags Flags)
	{
		return ImGui_SliderInt2570(Label, out V, VMin, VMax, Format, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_SliderInt3571([MarshalAs(UnmanagedType.LPStr)]string Label, out int V, int VMin, int VMax, [MarshalAs(UnmanagedType.LPStr)]string Format, ImGuiSliderFlags Flags);

	/// <summary><code>IMGUI_API bool          SliderInt3(const char* label, int v[3], int v_min, int v_max, const char* format = "%d", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          SliderInt3(const char* label, int v[3], int v_min, int v_max, const char* format = "%d", ImGuiSliderFlags flags = 0); </summary>
	public static bool SliderInt3(string Label, out int V, int VMin, int VMax)
	{
		return ImGui_SliderInt3571(Label, out V, VMin, VMax, "%d", (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          SliderInt3(const char* label, int v[3], int v_min, int v_max, const char* format = "%d", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          SliderInt3(const char* label, int v[3], int v_min, int v_max, const char* format = "%d", ImGuiSliderFlags flags = 0); </summary>
	public static bool SliderInt3(string Label, out int V, int VMin, int VMax, string Format)
	{
		return ImGui_SliderInt3571(Label, out V, VMin, VMax, Format, (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          SliderInt3(const char* label, int v[3], int v_min, int v_max, const char* format = "%d", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          SliderInt3(const char* label, int v[3], int v_min, int v_max, const char* format = "%d", ImGuiSliderFlags flags = 0); </summary>
	public static bool SliderInt3(string Label, out int V, int VMin, int VMax, string Format, ImGuiSliderFlags Flags)
	{
		return ImGui_SliderInt3571(Label, out V, VMin, VMax, Format, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_SliderInt4572([MarshalAs(UnmanagedType.LPStr)]string Label, out int V, int VMin, int VMax, [MarshalAs(UnmanagedType.LPStr)]string Format, ImGuiSliderFlags Flags);

	/// <summary><code>IMGUI_API bool          SliderInt4(const char* label, int v[4], int v_min, int v_max, const char* format = "%d", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          SliderInt4(const char* label, int v[4], int v_min, int v_max, const char* format = "%d", ImGuiSliderFlags flags = 0); </summary>
	public static bool SliderInt4(string Label, out int V, int VMin, int VMax)
	{
		return ImGui_SliderInt4572(Label, out V, VMin, VMax, "%d", (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          SliderInt4(const char* label, int v[4], int v_min, int v_max, const char* format = "%d", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          SliderInt4(const char* label, int v[4], int v_min, int v_max, const char* format = "%d", ImGuiSliderFlags flags = 0); </summary>
	public static bool SliderInt4(string Label, out int V, int VMin, int VMax, string Format)
	{
		return ImGui_SliderInt4572(Label, out V, VMin, VMax, Format, (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          SliderInt4(const char* label, int v[4], int v_min, int v_max, const char* format = "%d", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          SliderInt4(const char* label, int v[4], int v_min, int v_max, const char* format = "%d", ImGuiSliderFlags flags = 0); </summary>
	public static bool SliderInt4(string Label, out int V, int VMin, int VMax, string Format, ImGuiSliderFlags Flags)
	{
		return ImGui_SliderInt4572(Label, out V, VMin, VMax, Format, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_SliderScalar573([MarshalAs(UnmanagedType.LPStr)]string Label, ImGuiDataType DataType, IntPtr PData,  IntPtr PMin,  IntPtr PMax, [MarshalAs(UnmanagedType.LPStr)]string Format, ImGuiSliderFlags Flags);

	/// <summary><code>IMGUI_API bool          SliderScalar(const char* label, ImGuiDataType data_type, void* p_data, const void* p_min, const void* p_max, const char* format = NULL, ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          SliderScalar(const char* label, ImGuiDataType data_type, void* p_data, const void* p_min, const void* p_max, const char* format = NULL, ImGuiSliderFlags flags = 0); </summary>
	public static bool SliderScalar(string Label, ImGuiDataType DataType, IntPtr PData,  IntPtr PMin,  IntPtr PMax)
	{
		return ImGui_SliderScalar573(Label, DataType, PData, PMin, PMax, default, (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          SliderScalar(const char* label, ImGuiDataType data_type, void* p_data, const void* p_min, const void* p_max, const char* format = NULL, ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          SliderScalar(const char* label, ImGuiDataType data_type, void* p_data, const void* p_min, const void* p_max, const char* format = NULL, ImGuiSliderFlags flags = 0); </summary>
	public static bool SliderScalar(string Label, ImGuiDataType DataType, IntPtr PData,  IntPtr PMin,  IntPtr PMax, string Format)
	{
		return ImGui_SliderScalar573(Label, DataType, PData, PMin, PMax, Format, (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          SliderScalar(const char* label, ImGuiDataType data_type, void* p_data, const void* p_min, const void* p_max, const char* format = NULL, ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          SliderScalar(const char* label, ImGuiDataType data_type, void* p_data, const void* p_min, const void* p_max, const char* format = NULL, ImGuiSliderFlags flags = 0); </summary>
	public static bool SliderScalar(string Label, ImGuiDataType DataType, IntPtr PData,  IntPtr PMin,  IntPtr PMax, string Format, ImGuiSliderFlags Flags)
	{
		return ImGui_SliderScalar573(Label, DataType, PData, PMin, PMax, Format, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_SliderScalarN574([MarshalAs(UnmanagedType.LPStr)]string Label, ImGuiDataType DataType, IntPtr PData, int Components,  IntPtr PMin,  IntPtr PMax, [MarshalAs(UnmanagedType.LPStr)]string Format, ImGuiSliderFlags Flags);

	/// <summary><code>IMGUI_API bool          SliderScalarN(const char* label, ImGuiDataType data_type, void* p_data, int components, const void* p_min, const void* p_max, const char* format = NULL, ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          SliderScalarN(const char* label, ImGuiDataType data_type, void* p_data, int components, const void* p_min, const void* p_max, const char* format = NULL, ImGuiSliderFlags flags = 0); </summary>
	public static bool SliderScalarN(string Label, ImGuiDataType DataType, IntPtr PData, int Components,  IntPtr PMin,  IntPtr PMax)
	{
		return ImGui_SliderScalarN574(Label, DataType, PData, Components, PMin, PMax, default, (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          SliderScalarN(const char* label, ImGuiDataType data_type, void* p_data, int components, const void* p_min, const void* p_max, const char* format = NULL, ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          SliderScalarN(const char* label, ImGuiDataType data_type, void* p_data, int components, const void* p_min, const void* p_max, const char* format = NULL, ImGuiSliderFlags flags = 0); </summary>
	public static bool SliderScalarN(string Label, ImGuiDataType DataType, IntPtr PData, int Components,  IntPtr PMin,  IntPtr PMax, string Format)
	{
		return ImGui_SliderScalarN574(Label, DataType, PData, Components, PMin, PMax, Format, (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          SliderScalarN(const char* label, ImGuiDataType data_type, void* p_data, int components, const void* p_min, const void* p_max, const char* format = NULL, ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          SliderScalarN(const char* label, ImGuiDataType data_type, void* p_data, int components, const void* p_min, const void* p_max, const char* format = NULL, ImGuiSliderFlags flags = 0); </summary>
	public static bool SliderScalarN(string Label, ImGuiDataType DataType, IntPtr PData, int Components,  IntPtr PMin,  IntPtr PMax, string Format, ImGuiSliderFlags Flags)
	{
		return ImGui_SliderScalarN574(Label, DataType, PData, Components, PMin, PMax, Format, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_VSliderFloat575([MarshalAs(UnmanagedType.LPStr)]string Label, out  Vector2 Size, out float V, float VMin, float VMax, [MarshalAs(UnmanagedType.LPStr)]string Format, ImGuiSliderFlags Flags);

	/// <summary><code>IMGUI_API bool          VSliderFloat(const char* label, const ImVec2& size, float* v, float v_min, float v_max, const char* format = "%.3f", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          VSliderFloat(const char* label, const ImVec2& size, float* v, float v_min, float v_max, const char* format = "%.3f", ImGuiSliderFlags flags = 0); </summary>
	public static bool VSliderFloat(string Label, out  Vector2 Size, out float V, float VMin, float VMax)
	{
		return ImGui_VSliderFloat575(Label, out Size, out V, VMin, VMax, "%.3f", (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          VSliderFloat(const char* label, const ImVec2& size, float* v, float v_min, float v_max, const char* format = "%.3f", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          VSliderFloat(const char* label, const ImVec2& size, float* v, float v_min, float v_max, const char* format = "%.3f", ImGuiSliderFlags flags = 0); </summary>
	public static bool VSliderFloat(string Label, out  Vector2 Size, out float V, float VMin, float VMax, string Format)
	{
		return ImGui_VSliderFloat575(Label, out Size, out V, VMin, VMax, Format, (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          VSliderFloat(const char* label, const ImVec2& size, float* v, float v_min, float v_max, const char* format = "%.3f", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          VSliderFloat(const char* label, const ImVec2& size, float* v, float v_min, float v_max, const char* format = "%.3f", ImGuiSliderFlags flags = 0); </summary>
	public static bool VSliderFloat(string Label, out  Vector2 Size, out float V, float VMin, float VMax, string Format, ImGuiSliderFlags Flags)
	{
		return ImGui_VSliderFloat575(Label, out Size, out V, VMin, VMax, Format, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_VSliderInt576([MarshalAs(UnmanagedType.LPStr)]string Label, out  Vector2 Size, out int V, int VMin, int VMax, [MarshalAs(UnmanagedType.LPStr)]string Format, ImGuiSliderFlags Flags);

	/// <summary><code>IMGUI_API bool          VSliderInt(const char* label, const ImVec2& size, int* v, int v_min, int v_max, const char* format = "%d", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          VSliderInt(const char* label, const ImVec2& size, int* v, int v_min, int v_max, const char* format = "%d", ImGuiSliderFlags flags = 0); </summary>
	public static bool VSliderInt(string Label, out  Vector2 Size, out int V, int VMin, int VMax)
	{
		return ImGui_VSliderInt576(Label, out Size, out V, VMin, VMax, "%d", (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          VSliderInt(const char* label, const ImVec2& size, int* v, int v_min, int v_max, const char* format = "%d", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          VSliderInt(const char* label, const ImVec2& size, int* v, int v_min, int v_max, const char* format = "%d", ImGuiSliderFlags flags = 0); </summary>
	public static bool VSliderInt(string Label, out  Vector2 Size, out int V, int VMin, int VMax, string Format)
	{
		return ImGui_VSliderInt576(Label, out Size, out V, VMin, VMax, Format, (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          VSliderInt(const char* label, const ImVec2& size, int* v, int v_min, int v_max, const char* format = "%d", ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          VSliderInt(const char* label, const ImVec2& size, int* v, int v_min, int v_max, const char* format = "%d", ImGuiSliderFlags flags = 0); </summary>
	public static bool VSliderInt(string Label, out  Vector2 Size, out int V, int VMin, int VMax, string Format, ImGuiSliderFlags Flags)
	{
		return ImGui_VSliderInt576(Label, out Size, out V, VMin, VMax, Format, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_VSliderScalar577([MarshalAs(UnmanagedType.LPStr)]string Label, out  Vector2 Size, ImGuiDataType DataType, IntPtr PData,  IntPtr PMin,  IntPtr PMax, [MarshalAs(UnmanagedType.LPStr)]string Format, ImGuiSliderFlags Flags);

	/// <summary><code>IMGUI_API bool          VSliderScalar(const char* label, const ImVec2& size, ImGuiDataType data_type, void* p_data, const void* p_min, const void* p_max, const char* format = NULL, ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          VSliderScalar(const char* label, const ImVec2& size, ImGuiDataType data_type, void* p_data, const void* p_min, const void* p_max, const char* format = NULL, ImGuiSliderFlags flags = 0); </summary>
	public static bool VSliderScalar(string Label, out  Vector2 Size, ImGuiDataType DataType, IntPtr PData,  IntPtr PMin,  IntPtr PMax)
	{
		return ImGui_VSliderScalar577(Label, out Size, DataType, PData, PMin, PMax, default, (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          VSliderScalar(const char* label, const ImVec2& size, ImGuiDataType data_type, void* p_data, const void* p_min, const void* p_max, const char* format = NULL, ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          VSliderScalar(const char* label, const ImVec2& size, ImGuiDataType data_type, void* p_data, const void* p_min, const void* p_max, const char* format = NULL, ImGuiSliderFlags flags = 0); </summary>
	public static bool VSliderScalar(string Label, out  Vector2 Size, ImGuiDataType DataType, IntPtr PData,  IntPtr PMin,  IntPtr PMax, string Format)
	{
		return ImGui_VSliderScalar577(Label, out Size, DataType, PData, PMin, PMax, Format, (ImGuiSliderFlags)0);
	}

	/// <summary><code>IMGUI_API bool          VSliderScalar(const char* label, const ImVec2& size, ImGuiDataType data_type, void* p_data, const void* p_min, const void* p_max, const char* format = NULL, ImGuiSliderFlags flags = 0);</code>
		///    IMGUI_API bool          VSliderScalar(const char* label, const ImVec2& size, ImGuiDataType data_type, void* p_data, const void* p_min, const void* p_max, const char* format = NULL, ImGuiSliderFlags flags = 0); </summary>
	public static bool VSliderScalar(string Label, out  Vector2 Size, ImGuiDataType DataType, IntPtr PData,  IntPtr PMin,  IntPtr PMax, string Format, ImGuiSliderFlags Flags)
	{
		return ImGui_VSliderScalar577(Label, out Size, DataType, PData, PMin, PMax, Format, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_InputText582([MarshalAs(UnmanagedType.LPStr)]string Label, [MarshalAs(UnmanagedType.LPStr)]ref string Buf, long BufSize, ImGuiInputTextFlags Flags, ImGuiInputTextCallback Callback, IntPtr UserData);

	/// <summary><code>IMGUI_API bool          InputText(const char* label, char* buf, size_t buf_size, ImGuiInputTextFlags flags = 0, ImGuiInputTextCallback callback = NULL, void* user_data = NULL);</code>
		///    IMGUI_API bool          InputText(const char* label, char* buf, size_t buf_size, ImGuiInputTextFlags flags = 0, ImGuiInputTextCallback callback = NULL, void* user_data = NULL); </summary>
	public static bool InputText(string Label, ref string Buf, long BufSize)
	{
		return ImGui_InputText582(Label, ref Buf, BufSize, (ImGuiInputTextFlags)0, default, default);
	}

	/// <summary><code>IMGUI_API bool          InputText(const char* label, char* buf, size_t buf_size, ImGuiInputTextFlags flags = 0, ImGuiInputTextCallback callback = NULL, void* user_data = NULL);</code>
		///    IMGUI_API bool          InputText(const char* label, char* buf, size_t buf_size, ImGuiInputTextFlags flags = 0, ImGuiInputTextCallback callback = NULL, void* user_data = NULL); </summary>
	public static bool InputText(string Label, ref string Buf, long BufSize, ImGuiInputTextFlags Flags)
	{
		return ImGui_InputText582(Label, ref Buf, BufSize, Flags, default, default);
	}

	/// <summary><code>IMGUI_API bool          InputText(const char* label, char* buf, size_t buf_size, ImGuiInputTextFlags flags = 0, ImGuiInputTextCallback callback = NULL, void* user_data = NULL);</code>
		///    IMGUI_API bool          InputText(const char* label, char* buf, size_t buf_size, ImGuiInputTextFlags flags = 0, ImGuiInputTextCallback callback = NULL, void* user_data = NULL); </summary>
	public static bool InputText(string Label, ref string Buf, long BufSize, ImGuiInputTextFlags Flags, ImGuiInputTextCallback Callback)
	{
		return ImGui_InputText582(Label, ref Buf, BufSize, Flags, Callback, default);
	}

	/// <summary><code>IMGUI_API bool          InputText(const char* label, char* buf, size_t buf_size, ImGuiInputTextFlags flags = 0, ImGuiInputTextCallback callback = NULL, void* user_data = NULL);</code>
		///    IMGUI_API bool          InputText(const char* label, char* buf, size_t buf_size, ImGuiInputTextFlags flags = 0, ImGuiInputTextCallback callback = NULL, void* user_data = NULL); </summary>
	public static bool InputText(string Label, ref string Buf, long BufSize, ImGuiInputTextFlags Flags, ImGuiInputTextCallback Callback, IntPtr UserData)
	{
		return ImGui_InputText582(Label, ref Buf, BufSize, Flags, Callback, UserData);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_InputTextMultiline583([MarshalAs(UnmanagedType.LPStr)]string Label, [MarshalAs(UnmanagedType.LPStr)]ref string Buf, long BufSize, out  Vector2 Size, ImGuiInputTextFlags Flags, ImGuiInputTextCallback Callback, IntPtr UserData);

	/// <summary><code>IMGUI_API bool          InputTextMultiline(const char* label, char* buf, size_t buf_size, const ImVec2& size = ImVec2(0, 0), ImGuiInputTextFlags flags = 0, ImGuiInputTextCallback callback = NULL, void* user_data = NULL);</code>
		///    IMGUI_API bool          InputTextMultiline(const char* label, char* buf, size_t buf_size, const ImVec2& size = ImVec2(0, 0), ImGuiInputTextFlags flags = 0, ImGuiInputTextCallback callback = NULL, void* user_data = NULL); </summary>
	public static bool InputTextMultiline(string Label, ref string Buf, long BufSize)
	{
		 Vector2 param3 = new  Vector2 (0,  0);
		return ImGui_InputTextMultiline583(Label, ref Buf, BufSize, out param3, (ImGuiInputTextFlags)0, default, default);
	}

	/// <summary><code>IMGUI_API bool          InputTextMultiline(const char* label, char* buf, size_t buf_size, const ImVec2& size = ImVec2(0, 0), ImGuiInputTextFlags flags = 0, ImGuiInputTextCallback callback = NULL, void* user_data = NULL);</code>
		///    IMGUI_API bool          InputTextMultiline(const char* label, char* buf, size_t buf_size, const ImVec2& size = ImVec2(0, 0), ImGuiInputTextFlags flags = 0, ImGuiInputTextCallback callback = NULL, void* user_data = NULL); </summary>
	public static bool InputTextMultiline(string Label, ref string Buf, long BufSize, out  Vector2 Size)
	{
		return ImGui_InputTextMultiline583(Label, ref Buf, BufSize, out Size, (ImGuiInputTextFlags)0, default, default);
	}

	/// <summary><code>IMGUI_API bool          InputTextMultiline(const char* label, char* buf, size_t buf_size, const ImVec2& size = ImVec2(0, 0), ImGuiInputTextFlags flags = 0, ImGuiInputTextCallback callback = NULL, void* user_data = NULL);</code>
		///    IMGUI_API bool          InputTextMultiline(const char* label, char* buf, size_t buf_size, const ImVec2& size = ImVec2(0, 0), ImGuiInputTextFlags flags = 0, ImGuiInputTextCallback callback = NULL, void* user_data = NULL); </summary>
	public static bool InputTextMultiline(string Label, ref string Buf, long BufSize, out  Vector2 Size, ImGuiInputTextFlags Flags)
	{
		return ImGui_InputTextMultiline583(Label, ref Buf, BufSize, out Size, Flags, default, default);
	}

	/// <summary><code>IMGUI_API bool          InputTextMultiline(const char* label, char* buf, size_t buf_size, const ImVec2& size = ImVec2(0, 0), ImGuiInputTextFlags flags = 0, ImGuiInputTextCallback callback = NULL, void* user_data = NULL);</code>
		///    IMGUI_API bool          InputTextMultiline(const char* label, char* buf, size_t buf_size, const ImVec2& size = ImVec2(0, 0), ImGuiInputTextFlags flags = 0, ImGuiInputTextCallback callback = NULL, void* user_data = NULL); </summary>
	public static bool InputTextMultiline(string Label, ref string Buf, long BufSize, out  Vector2 Size, ImGuiInputTextFlags Flags, ImGuiInputTextCallback Callback)
	{
		return ImGui_InputTextMultiline583(Label, ref Buf, BufSize, out Size, Flags, Callback, default);
	}

	/// <summary><code>IMGUI_API bool          InputTextMultiline(const char* label, char* buf, size_t buf_size, const ImVec2& size = ImVec2(0, 0), ImGuiInputTextFlags flags = 0, ImGuiInputTextCallback callback = NULL, void* user_data = NULL);</code>
		///    IMGUI_API bool          InputTextMultiline(const char* label, char* buf, size_t buf_size, const ImVec2& size = ImVec2(0, 0), ImGuiInputTextFlags flags = 0, ImGuiInputTextCallback callback = NULL, void* user_data = NULL); </summary>
	public static bool InputTextMultiline(string Label, ref string Buf, long BufSize, out  Vector2 Size, ImGuiInputTextFlags Flags, ImGuiInputTextCallback Callback, IntPtr UserData)
	{
		return ImGui_InputTextMultiline583(Label, ref Buf, BufSize, out Size, Flags, Callback, UserData);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_InputTextWithHint584([MarshalAs(UnmanagedType.LPStr)]string Label, [MarshalAs(UnmanagedType.LPStr)]string Hint, [MarshalAs(UnmanagedType.LPStr)]ref string Buf, long BufSize, ImGuiInputTextFlags Flags, ImGuiInputTextCallback Callback, IntPtr UserData);

	/// <summary><code>IMGUI_API bool          InputTextWithHint(const char* label, const char* hint, char* buf, size_t buf_size, ImGuiInputTextFlags flags = 0, ImGuiInputTextCallback callback = NULL, void* user_data = NULL);</code>
		///    IMGUI_API bool          InputTextWithHint(const char* label, const char* hint, char* buf, size_t buf_size, ImGuiInputTextFlags flags = 0, ImGuiInputTextCallback callback = NULL, void* user_data = NULL); </summary>
	public static bool InputTextWithHint(string Label, string Hint, ref string Buf, long BufSize)
	{
		return ImGui_InputTextWithHint584(Label, Hint, ref Buf, BufSize, (ImGuiInputTextFlags)0, default, default);
	}

	/// <summary><code>IMGUI_API bool          InputTextWithHint(const char* label, const char* hint, char* buf, size_t buf_size, ImGuiInputTextFlags flags = 0, ImGuiInputTextCallback callback = NULL, void* user_data = NULL);</code>
		///    IMGUI_API bool          InputTextWithHint(const char* label, const char* hint, char* buf, size_t buf_size, ImGuiInputTextFlags flags = 0, ImGuiInputTextCallback callback = NULL, void* user_data = NULL); </summary>
	public static bool InputTextWithHint(string Label, string Hint, ref string Buf, long BufSize, ImGuiInputTextFlags Flags)
	{
		return ImGui_InputTextWithHint584(Label, Hint, ref Buf, BufSize, Flags, default, default);
	}

	/// <summary><code>IMGUI_API bool          InputTextWithHint(const char* label, const char* hint, char* buf, size_t buf_size, ImGuiInputTextFlags flags = 0, ImGuiInputTextCallback callback = NULL, void* user_data = NULL);</code>
		///    IMGUI_API bool          InputTextWithHint(const char* label, const char* hint, char* buf, size_t buf_size, ImGuiInputTextFlags flags = 0, ImGuiInputTextCallback callback = NULL, void* user_data = NULL); </summary>
	public static bool InputTextWithHint(string Label, string Hint, ref string Buf, long BufSize, ImGuiInputTextFlags Flags, ImGuiInputTextCallback Callback)
	{
		return ImGui_InputTextWithHint584(Label, Hint, ref Buf, BufSize, Flags, Callback, default);
	}

	/// <summary><code>IMGUI_API bool          InputTextWithHint(const char* label, const char* hint, char* buf, size_t buf_size, ImGuiInputTextFlags flags = 0, ImGuiInputTextCallback callback = NULL, void* user_data = NULL);</code>
		///    IMGUI_API bool          InputTextWithHint(const char* label, const char* hint, char* buf, size_t buf_size, ImGuiInputTextFlags flags = 0, ImGuiInputTextCallback callback = NULL, void* user_data = NULL); </summary>
	public static bool InputTextWithHint(string Label, string Hint, ref string Buf, long BufSize, ImGuiInputTextFlags Flags, ImGuiInputTextCallback Callback, IntPtr UserData)
	{
		return ImGui_InputTextWithHint584(Label, Hint, ref Buf, BufSize, Flags, Callback, UserData);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_InputFloat585([MarshalAs(UnmanagedType.LPStr)]string Label, out float V, float Step, float StepFast, [MarshalAs(UnmanagedType.LPStr)]string Format, ImGuiInputTextFlags Flags);

	/// <summary><code>IMGUI_API bool          InputFloat(const char* label, float* v, float step = 0.0f, float step_fast = 0.0f, const char* format = "%.3f", ImGuiInputTextFlags flags = 0);</code>
		///    IMGUI_API bool          InputFloat(const char* label, float* v, float step = 0.0f, float step_fast = 0.0f, const char* format = "%.3f", ImGuiInputTextFlags flags = 0); </summary>
	public static bool InputFloat(string Label, out float V)
	{
		return ImGui_InputFloat585(Label, out V, (float)0.0f, (float)0.0f, "%.3f", (ImGuiInputTextFlags)0);
	}

	/// <summary><code>IMGUI_API bool          InputFloat(const char* label, float* v, float step = 0.0f, float step_fast = 0.0f, const char* format = "%.3f", ImGuiInputTextFlags flags = 0);</code>
		///    IMGUI_API bool          InputFloat(const char* label, float* v, float step = 0.0f, float step_fast = 0.0f, const char* format = "%.3f", ImGuiInputTextFlags flags = 0); </summary>
	public static bool InputFloat(string Label, out float V, float Step)
	{
		return ImGui_InputFloat585(Label, out V, Step, (float)0.0f, "%.3f", (ImGuiInputTextFlags)0);
	}

	/// <summary><code>IMGUI_API bool          InputFloat(const char* label, float* v, float step = 0.0f, float step_fast = 0.0f, const char* format = "%.3f", ImGuiInputTextFlags flags = 0);</code>
		///    IMGUI_API bool          InputFloat(const char* label, float* v, float step = 0.0f, float step_fast = 0.0f, const char* format = "%.3f", ImGuiInputTextFlags flags = 0); </summary>
	public static bool InputFloat(string Label, out float V, float Step, float StepFast)
	{
		return ImGui_InputFloat585(Label, out V, Step, StepFast, "%.3f", (ImGuiInputTextFlags)0);
	}

	/// <summary><code>IMGUI_API bool          InputFloat(const char* label, float* v, float step = 0.0f, float step_fast = 0.0f, const char* format = "%.3f", ImGuiInputTextFlags flags = 0);</code>
		///    IMGUI_API bool          InputFloat(const char* label, float* v, float step = 0.0f, float step_fast = 0.0f, const char* format = "%.3f", ImGuiInputTextFlags flags = 0); </summary>
	public static bool InputFloat(string Label, out float V, float Step, float StepFast, string Format)
	{
		return ImGui_InputFloat585(Label, out V, Step, StepFast, Format, (ImGuiInputTextFlags)0);
	}

	/// <summary><code>IMGUI_API bool          InputFloat(const char* label, float* v, float step = 0.0f, float step_fast = 0.0f, const char* format = "%.3f", ImGuiInputTextFlags flags = 0);</code>
		///    IMGUI_API bool          InputFloat(const char* label, float* v, float step = 0.0f, float step_fast = 0.0f, const char* format = "%.3f", ImGuiInputTextFlags flags = 0); </summary>
	public static bool InputFloat(string Label, out float V, float Step, float StepFast, string Format, ImGuiInputTextFlags Flags)
	{
		return ImGui_InputFloat585(Label, out V, Step, StepFast, Format, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_InputFloat2586([MarshalAs(UnmanagedType.LPStr)]string Label, out float V, [MarshalAs(UnmanagedType.LPStr)]string Format, ImGuiInputTextFlags Flags);

	/// <summary><code>IMGUI_API bool          InputFloat2(const char* label, float v[2], const char* format = "%.3f", ImGuiInputTextFlags flags = 0);</code>
		///    IMGUI_API bool          InputFloat2(const char* label, float v[2], const char* format = "%.3f", ImGuiInputTextFlags flags = 0); </summary>
	public static bool InputFloat2(string Label, out float V)
	{
		return ImGui_InputFloat2586(Label, out V, "%.3f", (ImGuiInputTextFlags)0);
	}

	/// <summary><code>IMGUI_API bool          InputFloat2(const char* label, float v[2], const char* format = "%.3f", ImGuiInputTextFlags flags = 0);</code>
		///    IMGUI_API bool          InputFloat2(const char* label, float v[2], const char* format = "%.3f", ImGuiInputTextFlags flags = 0); </summary>
	public static bool InputFloat2(string Label, out float V, string Format)
	{
		return ImGui_InputFloat2586(Label, out V, Format, (ImGuiInputTextFlags)0);
	}

	/// <summary><code>IMGUI_API bool          InputFloat2(const char* label, float v[2], const char* format = "%.3f", ImGuiInputTextFlags flags = 0);</code>
		///    IMGUI_API bool          InputFloat2(const char* label, float v[2], const char* format = "%.3f", ImGuiInputTextFlags flags = 0); </summary>
	public static bool InputFloat2(string Label, out float V, string Format, ImGuiInputTextFlags Flags)
	{
		return ImGui_InputFloat2586(Label, out V, Format, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_InputFloat3587([MarshalAs(UnmanagedType.LPStr)]string Label, out float V, [MarshalAs(UnmanagedType.LPStr)]string Format, ImGuiInputTextFlags Flags);

	/// <summary><code>IMGUI_API bool          InputFloat3(const char* label, float v[3], const char* format = "%.3f", ImGuiInputTextFlags flags = 0);</code>
		///    IMGUI_API bool          InputFloat3(const char* label, float v[3], const char* format = "%.3f", ImGuiInputTextFlags flags = 0); </summary>
	public static bool InputFloat3(string Label, out float V)
	{
		return ImGui_InputFloat3587(Label, out V, "%.3f", (ImGuiInputTextFlags)0);
	}

	/// <summary><code>IMGUI_API bool          InputFloat3(const char* label, float v[3], const char* format = "%.3f", ImGuiInputTextFlags flags = 0);</code>
		///    IMGUI_API bool          InputFloat3(const char* label, float v[3], const char* format = "%.3f", ImGuiInputTextFlags flags = 0); </summary>
	public static bool InputFloat3(string Label, out float V, string Format)
	{
		return ImGui_InputFloat3587(Label, out V, Format, (ImGuiInputTextFlags)0);
	}

	/// <summary><code>IMGUI_API bool          InputFloat3(const char* label, float v[3], const char* format = "%.3f", ImGuiInputTextFlags flags = 0);</code>
		///    IMGUI_API bool          InputFloat3(const char* label, float v[3], const char* format = "%.3f", ImGuiInputTextFlags flags = 0); </summary>
	public static bool InputFloat3(string Label, out float V, string Format, ImGuiInputTextFlags Flags)
	{
		return ImGui_InputFloat3587(Label, out V, Format, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_InputFloat4588([MarshalAs(UnmanagedType.LPStr)]string Label, out float V, [MarshalAs(UnmanagedType.LPStr)]string Format, ImGuiInputTextFlags Flags);

	/// <summary><code>IMGUI_API bool          InputFloat4(const char* label, float v[4], const char* format = "%.3f", ImGuiInputTextFlags flags = 0);</code>
		///    IMGUI_API bool          InputFloat4(const char* label, float v[4], const char* format = "%.3f", ImGuiInputTextFlags flags = 0); </summary>
	public static bool InputFloat4(string Label, out float V)
	{
		return ImGui_InputFloat4588(Label, out V, "%.3f", (ImGuiInputTextFlags)0);
	}

	/// <summary><code>IMGUI_API bool          InputFloat4(const char* label, float v[4], const char* format = "%.3f", ImGuiInputTextFlags flags = 0);</code>
		///    IMGUI_API bool          InputFloat4(const char* label, float v[4], const char* format = "%.3f", ImGuiInputTextFlags flags = 0); </summary>
	public static bool InputFloat4(string Label, out float V, string Format)
	{
		return ImGui_InputFloat4588(Label, out V, Format, (ImGuiInputTextFlags)0);
	}

	/// <summary><code>IMGUI_API bool          InputFloat4(const char* label, float v[4], const char* format = "%.3f", ImGuiInputTextFlags flags = 0);</code>
		///    IMGUI_API bool          InputFloat4(const char* label, float v[4], const char* format = "%.3f", ImGuiInputTextFlags flags = 0); </summary>
	public static bool InputFloat4(string Label, out float V, string Format, ImGuiInputTextFlags Flags)
	{
		return ImGui_InputFloat4588(Label, out V, Format, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_InputInt589([MarshalAs(UnmanagedType.LPStr)]string Label, out int V, int Step, int StepFast, ImGuiInputTextFlags Flags);

	/// <summary><code>IMGUI_API bool          InputInt(const char* label, int* v, int step = 1, int step_fast = 100, ImGuiInputTextFlags flags = 0);</code>
		///    IMGUI_API bool          InputInt(const char* label, int* v, int step = 1, int step_fast = 100, ImGuiInputTextFlags flags = 0); </summary>
	public static bool InputInt(string Label, out int V)
	{
		return ImGui_InputInt589(Label, out V, (int)1, (int)100, (ImGuiInputTextFlags)0);
	}

	/// <summary><code>IMGUI_API bool          InputInt(const char* label, int* v, int step = 1, int step_fast = 100, ImGuiInputTextFlags flags = 0);</code>
		///    IMGUI_API bool          InputInt(const char* label, int* v, int step = 1, int step_fast = 100, ImGuiInputTextFlags flags = 0); </summary>
	public static bool InputInt(string Label, out int V, int Step)
	{
		return ImGui_InputInt589(Label, out V, Step, (int)100, (ImGuiInputTextFlags)0);
	}

	/// <summary><code>IMGUI_API bool          InputInt(const char* label, int* v, int step = 1, int step_fast = 100, ImGuiInputTextFlags flags = 0);</code>
		///    IMGUI_API bool          InputInt(const char* label, int* v, int step = 1, int step_fast = 100, ImGuiInputTextFlags flags = 0); </summary>
	public static bool InputInt(string Label, out int V, int Step, int StepFast)
	{
		return ImGui_InputInt589(Label, out V, Step, StepFast, (ImGuiInputTextFlags)0);
	}

	/// <summary><code>IMGUI_API bool          InputInt(const char* label, int* v, int step = 1, int step_fast = 100, ImGuiInputTextFlags flags = 0);</code>
		///    IMGUI_API bool          InputInt(const char* label, int* v, int step = 1, int step_fast = 100, ImGuiInputTextFlags flags = 0); </summary>
	public static bool InputInt(string Label, out int V, int Step, int StepFast, ImGuiInputTextFlags Flags)
	{
		return ImGui_InputInt589(Label, out V, Step, StepFast, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_InputInt2590([MarshalAs(UnmanagedType.LPStr)]string Label, out int V, ImGuiInputTextFlags Flags);

	/// <summary><code>IMGUI_API bool          InputInt2(const char* label, int v[2], ImGuiInputTextFlags flags = 0);</code>
		///    IMGUI_API bool          InputInt2(const char* label, int v[2], ImGuiInputTextFlags flags = 0); </summary>
	public static bool InputInt2(string Label, out int V)
	{
		return ImGui_InputInt2590(Label, out V, (ImGuiInputTextFlags)0);
	}

	/// <summary><code>IMGUI_API bool          InputInt2(const char* label, int v[2], ImGuiInputTextFlags flags = 0);</code>
		///    IMGUI_API bool          InputInt2(const char* label, int v[2], ImGuiInputTextFlags flags = 0); </summary>
	public static bool InputInt2(string Label, out int V, ImGuiInputTextFlags Flags)
	{
		return ImGui_InputInt2590(Label, out V, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_InputInt3591([MarshalAs(UnmanagedType.LPStr)]string Label, out int V, ImGuiInputTextFlags Flags);

	/// <summary><code>IMGUI_API bool          InputInt3(const char* label, int v[3], ImGuiInputTextFlags flags = 0);</code>
		///    IMGUI_API bool          InputInt3(const char* label, int v[3], ImGuiInputTextFlags flags = 0); </summary>
	public static bool InputInt3(string Label, out int V)
	{
		return ImGui_InputInt3591(Label, out V, (ImGuiInputTextFlags)0);
	}

	/// <summary><code>IMGUI_API bool          InputInt3(const char* label, int v[3], ImGuiInputTextFlags flags = 0);</code>
		///    IMGUI_API bool          InputInt3(const char* label, int v[3], ImGuiInputTextFlags flags = 0); </summary>
	public static bool InputInt3(string Label, out int V, ImGuiInputTextFlags Flags)
	{
		return ImGui_InputInt3591(Label, out V, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_InputInt4592([MarshalAs(UnmanagedType.LPStr)]string Label, out int V, ImGuiInputTextFlags Flags);

	/// <summary><code>IMGUI_API bool          InputInt4(const char* label, int v[4], ImGuiInputTextFlags flags = 0);</code>
		///    IMGUI_API bool          InputInt4(const char* label, int v[4], ImGuiInputTextFlags flags = 0); </summary>
	public static bool InputInt4(string Label, out int V)
	{
		return ImGui_InputInt4592(Label, out V, (ImGuiInputTextFlags)0);
	}

	/// <summary><code>IMGUI_API bool          InputInt4(const char* label, int v[4], ImGuiInputTextFlags flags = 0);</code>
		///    IMGUI_API bool          InputInt4(const char* label, int v[4], ImGuiInputTextFlags flags = 0); </summary>
	public static bool InputInt4(string Label, out int V, ImGuiInputTextFlags Flags)
	{
		return ImGui_InputInt4592(Label, out V, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_InputDouble593([MarshalAs(UnmanagedType.LPStr)]string Label, out double V, double Step, double StepFast, [MarshalAs(UnmanagedType.LPStr)]string Format, ImGuiInputTextFlags Flags);

	/// <summary><code>IMGUI_API bool          InputDouble(const char* label, double* v, double step = 0.0, double step_fast = 0.0, const char* format = "%.6f", ImGuiInputTextFlags flags = 0);</code>
		///    IMGUI_API bool          InputDouble(const char* label, double* v, double step = 0.0, double step_fast = 0.0, const char* format = "%.6f", ImGuiInputTextFlags flags = 0); </summary>
	public static bool InputDouble(string Label, out double V)
	{
		return ImGui_InputDouble593(Label, out V, (double)0.0, (double)0.0, "%.6f", (ImGuiInputTextFlags)0);
	}

	/// <summary><code>IMGUI_API bool          InputDouble(const char* label, double* v, double step = 0.0, double step_fast = 0.0, const char* format = "%.6f", ImGuiInputTextFlags flags = 0);</code>
		///    IMGUI_API bool          InputDouble(const char* label, double* v, double step = 0.0, double step_fast = 0.0, const char* format = "%.6f", ImGuiInputTextFlags flags = 0); </summary>
	public static bool InputDouble(string Label, out double V, double Step)
	{
		return ImGui_InputDouble593(Label, out V, Step, (double)0.0, "%.6f", (ImGuiInputTextFlags)0);
	}

	/// <summary><code>IMGUI_API bool          InputDouble(const char* label, double* v, double step = 0.0, double step_fast = 0.0, const char* format = "%.6f", ImGuiInputTextFlags flags = 0);</code>
		///    IMGUI_API bool          InputDouble(const char* label, double* v, double step = 0.0, double step_fast = 0.0, const char* format = "%.6f", ImGuiInputTextFlags flags = 0); </summary>
	public static bool InputDouble(string Label, out double V, double Step, double StepFast)
	{
		return ImGui_InputDouble593(Label, out V, Step, StepFast, "%.6f", (ImGuiInputTextFlags)0);
	}

	/// <summary><code>IMGUI_API bool          InputDouble(const char* label, double* v, double step = 0.0, double step_fast = 0.0, const char* format = "%.6f", ImGuiInputTextFlags flags = 0);</code>
		///    IMGUI_API bool          InputDouble(const char* label, double* v, double step = 0.0, double step_fast = 0.0, const char* format = "%.6f", ImGuiInputTextFlags flags = 0); </summary>
	public static bool InputDouble(string Label, out double V, double Step, double StepFast, string Format)
	{
		return ImGui_InputDouble593(Label, out V, Step, StepFast, Format, (ImGuiInputTextFlags)0);
	}

	/// <summary><code>IMGUI_API bool          InputDouble(const char* label, double* v, double step = 0.0, double step_fast = 0.0, const char* format = "%.6f", ImGuiInputTextFlags flags = 0);</code>
		///    IMGUI_API bool          InputDouble(const char* label, double* v, double step = 0.0, double step_fast = 0.0, const char* format = "%.6f", ImGuiInputTextFlags flags = 0); </summary>
	public static bool InputDouble(string Label, out double V, double Step, double StepFast, string Format, ImGuiInputTextFlags Flags)
	{
		return ImGui_InputDouble593(Label, out V, Step, StepFast, Format, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_InputScalar594([MarshalAs(UnmanagedType.LPStr)]string Label, ImGuiDataType DataType, IntPtr PData,  IntPtr PStep,  IntPtr PStepFast, [MarshalAs(UnmanagedType.LPStr)]string Format, ImGuiInputTextFlags Flags);

	/// <summary><code>IMGUI_API bool          InputScalar(const char* label, ImGuiDataType data_type, void* p_data, const void* p_step = NULL, const void* p_step_fast = NULL, const char* format = NULL, ImGuiInputTextFlags flags = 0);</code>
		///    IMGUI_API bool          InputScalar(const char* label, ImGuiDataType data_type, void* p_data, const void* p_step = NULL, const void* p_step_fast = NULL, const char* format = NULL, ImGuiInputTextFlags flags = 0); </summary>
	public static bool InputScalar(string Label, ImGuiDataType DataType, IntPtr PData)
	{
		return ImGui_InputScalar594(Label, DataType, PData, default, default, default, (ImGuiInputTextFlags)0);
	}

	/// <summary><code>IMGUI_API bool          InputScalar(const char* label, ImGuiDataType data_type, void* p_data, const void* p_step = NULL, const void* p_step_fast = NULL, const char* format = NULL, ImGuiInputTextFlags flags = 0);</code>
		///    IMGUI_API bool          InputScalar(const char* label, ImGuiDataType data_type, void* p_data, const void* p_step = NULL, const void* p_step_fast = NULL, const char* format = NULL, ImGuiInputTextFlags flags = 0); </summary>
	public static bool InputScalar(string Label, ImGuiDataType DataType, IntPtr PData,  IntPtr PStep)
	{
		return ImGui_InputScalar594(Label, DataType, PData, PStep, default, default, (ImGuiInputTextFlags)0);
	}

	/// <summary><code>IMGUI_API bool          InputScalar(const char* label, ImGuiDataType data_type, void* p_data, const void* p_step = NULL, const void* p_step_fast = NULL, const char* format = NULL, ImGuiInputTextFlags flags = 0);</code>
		///    IMGUI_API bool          InputScalar(const char* label, ImGuiDataType data_type, void* p_data, const void* p_step = NULL, const void* p_step_fast = NULL, const char* format = NULL, ImGuiInputTextFlags flags = 0); </summary>
	public static bool InputScalar(string Label, ImGuiDataType DataType, IntPtr PData,  IntPtr PStep,  IntPtr PStepFast)
	{
		return ImGui_InputScalar594(Label, DataType, PData, PStep, PStepFast, default, (ImGuiInputTextFlags)0);
	}

	/// <summary><code>IMGUI_API bool          InputScalar(const char* label, ImGuiDataType data_type, void* p_data, const void* p_step = NULL, const void* p_step_fast = NULL, const char* format = NULL, ImGuiInputTextFlags flags = 0);</code>
		///    IMGUI_API bool          InputScalar(const char* label, ImGuiDataType data_type, void* p_data, const void* p_step = NULL, const void* p_step_fast = NULL, const char* format = NULL, ImGuiInputTextFlags flags = 0); </summary>
	public static bool InputScalar(string Label, ImGuiDataType DataType, IntPtr PData,  IntPtr PStep,  IntPtr PStepFast, string Format)
	{
		return ImGui_InputScalar594(Label, DataType, PData, PStep, PStepFast, Format, (ImGuiInputTextFlags)0);
	}

	/// <summary><code>IMGUI_API bool          InputScalar(const char* label, ImGuiDataType data_type, void* p_data, const void* p_step = NULL, const void* p_step_fast = NULL, const char* format = NULL, ImGuiInputTextFlags flags = 0);</code>
		///    IMGUI_API bool          InputScalar(const char* label, ImGuiDataType data_type, void* p_data, const void* p_step = NULL, const void* p_step_fast = NULL, const char* format = NULL, ImGuiInputTextFlags flags = 0); </summary>
	public static bool InputScalar(string Label, ImGuiDataType DataType, IntPtr PData,  IntPtr PStep,  IntPtr PStepFast, string Format, ImGuiInputTextFlags Flags)
	{
		return ImGui_InputScalar594(Label, DataType, PData, PStep, PStepFast, Format, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_InputScalarN595([MarshalAs(UnmanagedType.LPStr)]string Label, ImGuiDataType DataType, IntPtr PData, int Components,  IntPtr PStep,  IntPtr PStepFast, [MarshalAs(UnmanagedType.LPStr)]string Format, ImGuiInputTextFlags Flags);

	/// <summary><code>IMGUI_API bool          InputScalarN(const char* label, ImGuiDataType data_type, void* p_data, int components, const void* p_step = NULL, const void* p_step_fast = NULL, const char* format = NULL, ImGuiInputTextFlags flags = 0);</code>
		///    IMGUI_API bool          InputScalarN(const char* label, ImGuiDataType data_type, void* p_data, int components, const void* p_step = NULL, const void* p_step_fast = NULL, const char* format = NULL, ImGuiInputTextFlags flags = 0); </summary>
	public static bool InputScalarN(string Label, ImGuiDataType DataType, IntPtr PData, int Components)
	{
		return ImGui_InputScalarN595(Label, DataType, PData, Components, default, default, default, (ImGuiInputTextFlags)0);
	}

	/// <summary><code>IMGUI_API bool          InputScalarN(const char* label, ImGuiDataType data_type, void* p_data, int components, const void* p_step = NULL, const void* p_step_fast = NULL, const char* format = NULL, ImGuiInputTextFlags flags = 0);</code>
		///    IMGUI_API bool          InputScalarN(const char* label, ImGuiDataType data_type, void* p_data, int components, const void* p_step = NULL, const void* p_step_fast = NULL, const char* format = NULL, ImGuiInputTextFlags flags = 0); </summary>
	public static bool InputScalarN(string Label, ImGuiDataType DataType, IntPtr PData, int Components,  IntPtr PStep)
	{
		return ImGui_InputScalarN595(Label, DataType, PData, Components, PStep, default, default, (ImGuiInputTextFlags)0);
	}

	/// <summary><code>IMGUI_API bool          InputScalarN(const char* label, ImGuiDataType data_type, void* p_data, int components, const void* p_step = NULL, const void* p_step_fast = NULL, const char* format = NULL, ImGuiInputTextFlags flags = 0);</code>
		///    IMGUI_API bool          InputScalarN(const char* label, ImGuiDataType data_type, void* p_data, int components, const void* p_step = NULL, const void* p_step_fast = NULL, const char* format = NULL, ImGuiInputTextFlags flags = 0); </summary>
	public static bool InputScalarN(string Label, ImGuiDataType DataType, IntPtr PData, int Components,  IntPtr PStep,  IntPtr PStepFast)
	{
		return ImGui_InputScalarN595(Label, DataType, PData, Components, PStep, PStepFast, default, (ImGuiInputTextFlags)0);
	}

	/// <summary><code>IMGUI_API bool          InputScalarN(const char* label, ImGuiDataType data_type, void* p_data, int components, const void* p_step = NULL, const void* p_step_fast = NULL, const char* format = NULL, ImGuiInputTextFlags flags = 0);</code>
		///    IMGUI_API bool          InputScalarN(const char* label, ImGuiDataType data_type, void* p_data, int components, const void* p_step = NULL, const void* p_step_fast = NULL, const char* format = NULL, ImGuiInputTextFlags flags = 0); </summary>
	public static bool InputScalarN(string Label, ImGuiDataType DataType, IntPtr PData, int Components,  IntPtr PStep,  IntPtr PStepFast, string Format)
	{
		return ImGui_InputScalarN595(Label, DataType, PData, Components, PStep, PStepFast, Format, (ImGuiInputTextFlags)0);
	}

	/// <summary><code>IMGUI_API bool          InputScalarN(const char* label, ImGuiDataType data_type, void* p_data, int components, const void* p_step = NULL, const void* p_step_fast = NULL, const char* format = NULL, ImGuiInputTextFlags flags = 0);</code>
		///    IMGUI_API bool          InputScalarN(const char* label, ImGuiDataType data_type, void* p_data, int components, const void* p_step = NULL, const void* p_step_fast = NULL, const char* format = NULL, ImGuiInputTextFlags flags = 0); </summary>
	public static bool InputScalarN(string Label, ImGuiDataType DataType, IntPtr PData, int Components,  IntPtr PStep,  IntPtr PStepFast, string Format, ImGuiInputTextFlags Flags)
	{
		return ImGui_InputScalarN595(Label, DataType, PData, Components, PStep, PStepFast, Format, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_ColorEdit3600([MarshalAs(UnmanagedType.LPStr)]string Label, out float Col, ImGuiColorEditFlags Flags);

	/// <summary><code>IMGUI_API bool          ColorEdit3(const char* label, float col[3], ImGuiColorEditFlags flags = 0);</code>
		///    IMGUI_API bool          ColorEdit3(const char* label, float col[3], ImGuiColorEditFlags flags = 0); </summary>
	public static bool ColorEdit3(string Label, out float Col)
	{
		return ImGui_ColorEdit3600(Label, out Col, (ImGuiColorEditFlags)0);
	}

	/// <summary><code>IMGUI_API bool          ColorEdit3(const char* label, float col[3], ImGuiColorEditFlags flags = 0);</code>
		///    IMGUI_API bool          ColorEdit3(const char* label, float col[3], ImGuiColorEditFlags flags = 0); </summary>
	public static bool ColorEdit3(string Label, out float Col, ImGuiColorEditFlags Flags)
	{
		return ImGui_ColorEdit3600(Label, out Col, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_ColorEdit4601([MarshalAs(UnmanagedType.LPStr)]string Label, out float Col, ImGuiColorEditFlags Flags);

	/// <summary><code>IMGUI_API bool          ColorEdit4(const char* label, float col[4], ImGuiColorEditFlags flags = 0);</code>
		///    IMGUI_API bool          ColorEdit4(const char* label, float col[4], ImGuiColorEditFlags flags = 0); </summary>
	public static bool ColorEdit4(string Label, out float Col)
	{
		return ImGui_ColorEdit4601(Label, out Col, (ImGuiColorEditFlags)0);
	}

	/// <summary><code>IMGUI_API bool          ColorEdit4(const char* label, float col[4], ImGuiColorEditFlags flags = 0);</code>
		///    IMGUI_API bool          ColorEdit4(const char* label, float col[4], ImGuiColorEditFlags flags = 0); </summary>
	public static bool ColorEdit4(string Label, out float Col, ImGuiColorEditFlags Flags)
	{
		return ImGui_ColorEdit4601(Label, out Col, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_ColorPicker3602([MarshalAs(UnmanagedType.LPStr)]string Label, out float Col, ImGuiColorEditFlags Flags);

	/// <summary><code>IMGUI_API bool          ColorPicker3(const char* label, float col[3], ImGuiColorEditFlags flags = 0);</code>
		///    IMGUI_API bool          ColorPicker3(const char* label, float col[3], ImGuiColorEditFlags flags = 0); </summary>
	public static bool ColorPicker3(string Label, out float Col)
	{
		return ImGui_ColorPicker3602(Label, out Col, (ImGuiColorEditFlags)0);
	}

	/// <summary><code>IMGUI_API bool          ColorPicker3(const char* label, float col[3], ImGuiColorEditFlags flags = 0);</code>
		///    IMGUI_API bool          ColorPicker3(const char* label, float col[3], ImGuiColorEditFlags flags = 0); </summary>
	public static bool ColorPicker3(string Label, out float Col, ImGuiColorEditFlags Flags)
	{
		return ImGui_ColorPicker3602(Label, out Col, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_ColorPicker4603([MarshalAs(UnmanagedType.LPStr)]string Label, out float Col, ImGuiColorEditFlags Flags, out  float RefCol);

	/// <summary><code>IMGUI_API bool          ColorPicker4(const char* label, float col[4], ImGuiColorEditFlags flags = 0, const float* ref_col = NULL);</code>
		///    IMGUI_API bool          ColorPicker4(const char* label, float col[4], ImGuiColorEditFlags flags = 0, const float* ref_col = NULL); </summary>
	public static bool ColorPicker4(string Label, out float Col)
	{
		return ImGui_ColorPicker4603(Label, out Col, (ImGuiColorEditFlags)0, out _);
	}

	/// <summary><code>IMGUI_API bool          ColorPicker4(const char* label, float col[4], ImGuiColorEditFlags flags = 0, const float* ref_col = NULL);</code>
		///    IMGUI_API bool          ColorPicker4(const char* label, float col[4], ImGuiColorEditFlags flags = 0, const float* ref_col = NULL); </summary>
	public static bool ColorPicker4(string Label, out float Col, ImGuiColorEditFlags Flags)
	{
		return ImGui_ColorPicker4603(Label, out Col, Flags, out _);
	}

	/// <summary><code>IMGUI_API bool          ColorPicker4(const char* label, float col[4], ImGuiColorEditFlags flags = 0, const float* ref_col = NULL);</code>
		///    IMGUI_API bool          ColorPicker4(const char* label, float col[4], ImGuiColorEditFlags flags = 0, const float* ref_col = NULL); </summary>
	public static bool ColorPicker4(string Label, out float Col, ImGuiColorEditFlags Flags, out  float RefCol)
	{
		return ImGui_ColorPicker4603(Label, out Col, Flags, out RefCol);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_ColorButton604([MarshalAs(UnmanagedType.LPStr)]string DescId, out  Vector4 Col, ImGuiColorEditFlags Flags, out  Vector2 Size);

	/// <summary><code>IMGUI_API bool          ColorButton(const char* desc_id, const ImVec4& col, ImGuiColorEditFlags flags = 0, const ImVec2& size = ImVec2(0, 0)); </code>
		/// display a color square/button, hover for details, return true when pressed. </summary>
	public static bool ColorButton(string DescId, out  Vector4 Col)
	{
		 Vector2 param3 = new  Vector2 (0,  0);
		return ImGui_ColorButton604(DescId, out Col, (ImGuiColorEditFlags)0, out param3);
	}

	/// <summary><code>IMGUI_API bool          ColorButton(const char* desc_id, const ImVec4& col, ImGuiColorEditFlags flags = 0, const ImVec2& size = ImVec2(0, 0)); </code>
		/// display a color square/button, hover for details, return true when pressed. </summary>
	public static bool ColorButton(string DescId, out  Vector4 Col, ImGuiColorEditFlags Flags)
	{
		 Vector2 param3 = new  Vector2 (0,  0);
		return ImGui_ColorButton604(DescId, out Col, Flags, out param3);
	}

	/// <summary><code>IMGUI_API bool          ColorButton(const char* desc_id, const ImVec4& col, ImGuiColorEditFlags flags = 0, const ImVec2& size = ImVec2(0, 0)); </code>
		/// display a color square/button, hover for details, return true when pressed. </summary>
	public static bool ColorButton(string DescId, out  Vector4 Col, ImGuiColorEditFlags Flags, out  Vector2 Size)
	{
		return ImGui_ColorButton604(DescId, out Col, Flags, out Size);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_SetColorEditOptions605(ImGuiColorEditFlags Flags);

	/// <summary><code>IMGUI_API void          SetColorEditOptions(ImGuiColorEditFlags flags);                     </code>
		/// initialize current options (generally on application startup) if you want to select a default format, picker type, etc. User will be able to change many settings, unless you pass the _NoOptions flag to your calls. </summary>
	public static void SetColorEditOptions(ImGuiColorEditFlags Flags)
	{
		ImGui_SetColorEditOptions605(Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_TreeNode609([MarshalAs(UnmanagedType.LPStr)]string Label);

	/// <summary><code>IMGUI_API bool          TreeNode(const char* label);</code>
		///    IMGUI_API bool          TreeNode(const char* label); </summary>
	public static bool TreeNode(string Label)
	{
		return ImGui_TreeNode609(Label);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_TreeNodeEx614([MarshalAs(UnmanagedType.LPStr)]string Label, ImGuiTreeNodeFlags Flags);

	/// <summary><code>IMGUI_API bool          TreeNodeEx(const char* label, ImGuiTreeNodeFlags flags = 0);</code>
		///    IMGUI_API bool          TreeNodeEx(const char* label, ImGuiTreeNodeFlags flags = 0); </summary>
	public static bool TreeNodeEx(string Label)
	{
		return ImGui_TreeNodeEx614(Label, (ImGuiTreeNodeFlags)0);
	}

	/// <summary><code>IMGUI_API bool          TreeNodeEx(const char* label, ImGuiTreeNodeFlags flags = 0);</code>
		///    IMGUI_API bool          TreeNodeEx(const char* label, ImGuiTreeNodeFlags flags = 0); </summary>
	public static bool TreeNodeEx(string Label, ImGuiTreeNodeFlags Flags)
	{
		return ImGui_TreeNodeEx614(Label, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_TreePush619([MarshalAs(UnmanagedType.LPStr)]string StrId);

	/// <summary><code>IMGUI_API void          TreePush(const char* str_id);                                       </code>
		/// ~ Indent()+PushId(). Already called by TreeNode() when returning true, but you can call TreePush/TreePop yourself if desired. </summary>
	public static void TreePush(string StrId)
	{
		ImGui_TreePush619(StrId);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_TreePush620( IntPtr PtrId);

	/// <summary><code>IMGUI_API void          TreePush(const void* ptr_id);                                       </code>
		/// " </summary>
	public static void TreePush( IntPtr PtrId)
	{
		ImGui_TreePush620(PtrId);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_TreePop621();

	/// <summary><code>IMGUI_API void          TreePop();                                                          </code>
		/// ~ Unindent()+PopId() </summary>
	public static void TreePop()
	{
		ImGui_TreePop621();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGui_GetTreeNodeToLabelSpacing622();

	/// <summary><code>IMGUI_API float         GetTreeNodeToLabelSpacing();                                        </code>
		/// horizontal distance preceding label when using TreeNode*() or Bullet() == (g.FontSize + style.FramePadding.x*2) for a regular unframed TreeNode </summary>
	public static float GetTreeNodeToLabelSpacing()
	{
		return ImGui_GetTreeNodeToLabelSpacing622();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_CollapsingHeader623([MarshalAs(UnmanagedType.LPStr)]string Label, ImGuiTreeNodeFlags Flags);

	/// <summary><code>IMGUI_API bool          CollapsingHeader(const char* label, ImGuiTreeNodeFlags flags = 0);  </code>
		/// if returning 'true' the header is open. doesn't indent nor push on ID stack. user doesn't have to call TreePop(). </summary>
	public static bool CollapsingHeader(string Label)
	{
		return ImGui_CollapsingHeader623(Label, (ImGuiTreeNodeFlags)0);
	}

	/// <summary><code>IMGUI_API bool          CollapsingHeader(const char* label, ImGuiTreeNodeFlags flags = 0);  </code>
		/// if returning 'true' the header is open. doesn't indent nor push on ID stack. user doesn't have to call TreePop(). </summary>
	public static bool CollapsingHeader(string Label, ImGuiTreeNodeFlags Flags)
	{
		return ImGui_CollapsingHeader623(Label, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_CollapsingHeader624([MarshalAs(UnmanagedType.LPStr)]string Label, [MarshalAs(UnmanagedType.I1)]out bool PVisible, ImGuiTreeNodeFlags Flags);

	/// <summary><code>IMGUI_API bool          CollapsingHeader(const char* label, bool* p_visible, ImGuiTreeNodeFlags flags = 0); </code>
		/// when 'p_visible != NULL': if '*p_visible==true' display an additional small close button on upper right of the header which will set the bool to false when clicked, if '*p_visible==false' don't display the header. </summary>
	public static bool CollapsingHeader(string Label, out bool PVisible)
	{
		return ImGui_CollapsingHeader624(Label, out PVisible, (ImGuiTreeNodeFlags)0);
	}

	/// <summary><code>IMGUI_API bool          CollapsingHeader(const char* label, bool* p_visible, ImGuiTreeNodeFlags flags = 0); </code>
		/// when 'p_visible != NULL': if '*p_visible==true' display an additional small close button on upper right of the header which will set the bool to false when clicked, if '*p_visible==false' don't display the header. </summary>
	public static bool CollapsingHeader(string Label, out bool PVisible, ImGuiTreeNodeFlags Flags)
	{
		return ImGui_CollapsingHeader624(Label, out PVisible, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_SetNextItemOpen625([MarshalAs(UnmanagedType.I1)]bool IsOpen, ImGuiCond Cond);

	/// <summary><code>IMGUI_API void          SetNextItemOpen(bool is_open, ImGuiCond cond = 0);                  </code>
		/// set next TreeNode/CollapsingHeader open state. </summary>
	public static void SetNextItemOpen(bool IsOpen)
	{
		ImGui_SetNextItemOpen625(IsOpen, (ImGuiCond)0);
	}

	/// <summary><code>IMGUI_API void          SetNextItemOpen(bool is_open, ImGuiCond cond = 0);                  </code>
		/// set next TreeNode/CollapsingHeader open state. </summary>
	public static void SetNextItemOpen(bool IsOpen, ImGuiCond Cond)
	{
		ImGui_SetNextItemOpen625(IsOpen, Cond);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_Selectable630([MarshalAs(UnmanagedType.LPStr)]string Label, [MarshalAs(UnmanagedType.I1)]bool Selected, ImGuiSelectableFlags Flags, out  Vector2 Size);

	/// <summary><code>IMGUI_API bool          Selectable(const char* label, bool selected = false, ImGuiSelectableFlags flags = 0, const ImVec2& size = ImVec2(0, 0)); </code>
		/// "bool selected" carry the selection state (read-only). Selectable() is clicked is returns true so you can modify your selection state. size.x==0.0: use remaining width, size.x>0.0: specify width. size.y==0.0: use label height, size.y>0.0: specify height </summary>
	public static bool Selectable(string Label)
	{
		 Vector2 param3 = new  Vector2 (0,  0);
		return ImGui_Selectable630(Label, false, (ImGuiSelectableFlags)0, out param3);
	}

	/// <summary><code>IMGUI_API bool          Selectable(const char* label, bool selected = false, ImGuiSelectableFlags flags = 0, const ImVec2& size = ImVec2(0, 0)); </code>
		/// "bool selected" carry the selection state (read-only). Selectable() is clicked is returns true so you can modify your selection state. size.x==0.0: use remaining width, size.x>0.0: specify width. size.y==0.0: use label height, size.y>0.0: specify height </summary>
	public static bool Selectable(string Label, bool Selected)
	{
		 Vector2 param3 = new  Vector2 (0,  0);
		return ImGui_Selectable630(Label, Selected, (ImGuiSelectableFlags)0, out param3);
	}

	/// <summary><code>IMGUI_API bool          Selectable(const char* label, bool selected = false, ImGuiSelectableFlags flags = 0, const ImVec2& size = ImVec2(0, 0)); </code>
		/// "bool selected" carry the selection state (read-only). Selectable() is clicked is returns true so you can modify your selection state. size.x==0.0: use remaining width, size.x>0.0: specify width. size.y==0.0: use label height, size.y>0.0: specify height </summary>
	public static bool Selectable(string Label, bool Selected, ImGuiSelectableFlags Flags)
	{
		 Vector2 param3 = new  Vector2 (0,  0);
		return ImGui_Selectable630(Label, Selected, Flags, out param3);
	}

	/// <summary><code>IMGUI_API bool          Selectable(const char* label, bool selected = false, ImGuiSelectableFlags flags = 0, const ImVec2& size = ImVec2(0, 0)); </code>
		/// "bool selected" carry the selection state (read-only). Selectable() is clicked is returns true so you can modify your selection state. size.x==0.0: use remaining width, size.x>0.0: specify width. size.y==0.0: use label height, size.y>0.0: specify height </summary>
	public static bool Selectable(string Label, bool Selected, ImGuiSelectableFlags Flags, out  Vector2 Size)
	{
		return ImGui_Selectable630(Label, Selected, Flags, out Size);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_Selectable631([MarshalAs(UnmanagedType.LPStr)]string Label, [MarshalAs(UnmanagedType.I1)]out bool PSelected, ImGuiSelectableFlags Flags, out  Vector2 Size);

	/// <summary><code>IMGUI_API bool          Selectable(const char* label, bool* p_selected, ImGuiSelectableFlags flags = 0, const ImVec2& size = ImVec2(0, 0));      </code>
		/// "bool* p_selected" point to the selection state (read-write), as a convenient helper. </summary>
	public static bool Selectable(string Label, out bool PSelected)
	{
		 Vector2 param3 = new  Vector2 (0,  0);
		return ImGui_Selectable631(Label, out PSelected, (ImGuiSelectableFlags)0, out param3);
	}

	/// <summary><code>IMGUI_API bool          Selectable(const char* label, bool* p_selected, ImGuiSelectableFlags flags = 0, const ImVec2& size = ImVec2(0, 0));      </code>
		/// "bool* p_selected" point to the selection state (read-write), as a convenient helper. </summary>
	public static bool Selectable(string Label, out bool PSelected, ImGuiSelectableFlags Flags)
	{
		 Vector2 param3 = new  Vector2 (0,  0);
		return ImGui_Selectable631(Label, out PSelected, Flags, out param3);
	}

	/// <summary><code>IMGUI_API bool          Selectable(const char* label, bool* p_selected, ImGuiSelectableFlags flags = 0, const ImVec2& size = ImVec2(0, 0));      </code>
		/// "bool* p_selected" point to the selection state (read-write), as a convenient helper. </summary>
	public static bool Selectable(string Label, out bool PSelected, ImGuiSelectableFlags Flags, out  Vector2 Size)
	{
		return ImGui_Selectable631(Label, out PSelected, Flags, out Size);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_BeginListBox639([MarshalAs(UnmanagedType.LPStr)]string Label, out  Vector2 Size);

	/// <summary><code>IMGUI_API bool          BeginListBox(const char* label, const ImVec2& size = ImVec2(0, 0)); </code>
		/// open a framed scrolling region </summary>
	public static bool BeginListBox(string Label)
	{
		 Vector2 param1 = new  Vector2 (0,  0);
		return ImGui_BeginListBox639(Label, out param1);
	}

	/// <summary><code>IMGUI_API bool          BeginListBox(const char* label, const ImVec2& size = ImVec2(0, 0)); </code>
		/// open a framed scrolling region </summary>
	public static bool BeginListBox(string Label, out  Vector2 Size)
	{
		return ImGui_BeginListBox639(Label, out Size);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_EndListBox640();

	/// <summary><code>IMGUI_API void          EndListBox();                                                       </code>
		/// only call EndListBox() if BeginListBox() returned true! </summary>
	public static void EndListBox()
	{
		ImGui_EndListBox640();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_ListBox641([MarshalAs(UnmanagedType.LPStr)]string Label, out int CurrentItem, [MarshalAs(UnmanagedType.LPStr)]out string  Items, int ItemsCount, int HeightInItems);

	/// <summary><code>IMGUI_API bool          ListBox(const char* label, int* current_item, const char* const items[], int items_count, int height_in_items = -1);</code>
		///    IMGUI_API bool          ListBox(const char* label, int* current_item, const char* const items[], int items_count, int height_in_items = -1); </summary>
	public static bool ListBox(string Label, out int CurrentItem, out string  Items, int ItemsCount)
	{
		return ImGui_ListBox641(Label, out CurrentItem, out Items, ItemsCount, -1);
	}

	/// <summary><code>IMGUI_API bool          ListBox(const char* label, int* current_item, const char* const items[], int items_count, int height_in_items = -1);</code>
		///    IMGUI_API bool          ListBox(const char* label, int* current_item, const char* const items[], int items_count, int height_in_items = -1); </summary>
	public static bool ListBox(string Label, out int CurrentItem, out string  Items, int ItemsCount, int HeightInItems)
	{
		return ImGui_ListBox641(Label, out CurrentItem, out Items, ItemsCount, HeightInItems);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_PlotLines646([MarshalAs(UnmanagedType.LPStr)]string Label, out  float Values, int ValuesCount, int ValuesOffset, [MarshalAs(UnmanagedType.LPStr)]string OverlayText, float ScaleMin, float ScaleMax, Vector2 GraphSize, int Stride);

	/// <summary><code>IMGUI_API void          PlotLines(const char* label, const float* values, int values_count, int values_offset = 0, const char* overlay_text = NULL, float scale_min = FLT_MAX, float scale_max = FLT_MAX, ImVec2 graph_size = ImVec2(0, 0), int stride = sizeof(float));</code>
		///    IMGUI_API void          PlotLines(const char* label, const float* values, int values_count, int values_offset = 0, const char* overlay_text = NULL, float scale_min = FLT_MAX, float scale_max = FLT_MAX, ImVec2 graph_size = ImVec2(0, 0), int stride = sizeof(float)); </summary>
	public static void PlotLines(string Label, out  float Values, int ValuesCount)
	{
		Vector2 param7 = new Vector2 (0,  0);
		ImGui_PlotLines646(Label, out Values, ValuesCount, (int)0, default, float.MaxValue, float.MaxValue, param7, sizeof(float));
	}

	/// <summary><code>IMGUI_API void          PlotLines(const char* label, const float* values, int values_count, int values_offset = 0, const char* overlay_text = NULL, float scale_min = FLT_MAX, float scale_max = FLT_MAX, ImVec2 graph_size = ImVec2(0, 0), int stride = sizeof(float));</code>
		///    IMGUI_API void          PlotLines(const char* label, const float* values, int values_count, int values_offset = 0, const char* overlay_text = NULL, float scale_min = FLT_MAX, float scale_max = FLT_MAX, ImVec2 graph_size = ImVec2(0, 0), int stride = sizeof(float)); </summary>
	public static void PlotLines(string Label, out  float Values, int ValuesCount, int ValuesOffset)
	{
		Vector2 param7 = new Vector2 (0,  0);
		ImGui_PlotLines646(Label, out Values, ValuesCount, ValuesOffset, default, float.MaxValue, float.MaxValue, param7, sizeof(float));
	}

	/// <summary><code>IMGUI_API void          PlotLines(const char* label, const float* values, int values_count, int values_offset = 0, const char* overlay_text = NULL, float scale_min = FLT_MAX, float scale_max = FLT_MAX, ImVec2 graph_size = ImVec2(0, 0), int stride = sizeof(float));</code>
		///    IMGUI_API void          PlotLines(const char* label, const float* values, int values_count, int values_offset = 0, const char* overlay_text = NULL, float scale_min = FLT_MAX, float scale_max = FLT_MAX, ImVec2 graph_size = ImVec2(0, 0), int stride = sizeof(float)); </summary>
	public static void PlotLines(string Label, out  float Values, int ValuesCount, int ValuesOffset, string OverlayText)
	{
		Vector2 param7 = new Vector2 (0,  0);
		ImGui_PlotLines646(Label, out Values, ValuesCount, ValuesOffset, OverlayText, float.MaxValue, float.MaxValue, param7, sizeof(float));
	}

	/// <summary><code>IMGUI_API void          PlotLines(const char* label, const float* values, int values_count, int values_offset = 0, const char* overlay_text = NULL, float scale_min = FLT_MAX, float scale_max = FLT_MAX, ImVec2 graph_size = ImVec2(0, 0), int stride = sizeof(float));</code>
		///    IMGUI_API void          PlotLines(const char* label, const float* values, int values_count, int values_offset = 0, const char* overlay_text = NULL, float scale_min = FLT_MAX, float scale_max = FLT_MAX, ImVec2 graph_size = ImVec2(0, 0), int stride = sizeof(float)); </summary>
	public static void PlotLines(string Label, out  float Values, int ValuesCount, int ValuesOffset, string OverlayText, float ScaleMin)
	{
		Vector2 param7 = new Vector2 (0,  0);
		ImGui_PlotLines646(Label, out Values, ValuesCount, ValuesOffset, OverlayText, ScaleMin, float.MaxValue, param7, sizeof(float));
	}

	/// <summary><code>IMGUI_API void          PlotLines(const char* label, const float* values, int values_count, int values_offset = 0, const char* overlay_text = NULL, float scale_min = FLT_MAX, float scale_max = FLT_MAX, ImVec2 graph_size = ImVec2(0, 0), int stride = sizeof(float));</code>
		///    IMGUI_API void          PlotLines(const char* label, const float* values, int values_count, int values_offset = 0, const char* overlay_text = NULL, float scale_min = FLT_MAX, float scale_max = FLT_MAX, ImVec2 graph_size = ImVec2(0, 0), int stride = sizeof(float)); </summary>
	public static void PlotLines(string Label, out  float Values, int ValuesCount, int ValuesOffset, string OverlayText, float ScaleMin, float ScaleMax)
	{
		Vector2 param7 = new Vector2 (0,  0);
		ImGui_PlotLines646(Label, out Values, ValuesCount, ValuesOffset, OverlayText, ScaleMin, ScaleMax, param7, sizeof(float));
	}

	/// <summary><code>IMGUI_API void          PlotLines(const char* label, const float* values, int values_count, int values_offset = 0, const char* overlay_text = NULL, float scale_min = FLT_MAX, float scale_max = FLT_MAX, ImVec2 graph_size = ImVec2(0, 0), int stride = sizeof(float));</code>
		///    IMGUI_API void          PlotLines(const char* label, const float* values, int values_count, int values_offset = 0, const char* overlay_text = NULL, float scale_min = FLT_MAX, float scale_max = FLT_MAX, ImVec2 graph_size = ImVec2(0, 0), int stride = sizeof(float)); </summary>
	public static void PlotLines(string Label, out  float Values, int ValuesCount, int ValuesOffset, string OverlayText, float ScaleMin, float ScaleMax, Vector2 GraphSize)
	{
		ImGui_PlotLines646(Label, out Values, ValuesCount, ValuesOffset, OverlayText, ScaleMin, ScaleMax, GraphSize, sizeof(float));
	}

	/// <summary><code>IMGUI_API void          PlotLines(const char* label, const float* values, int values_count, int values_offset = 0, const char* overlay_text = NULL, float scale_min = FLT_MAX, float scale_max = FLT_MAX, ImVec2 graph_size = ImVec2(0, 0), int stride = sizeof(float));</code>
		///    IMGUI_API void          PlotLines(const char* label, const float* values, int values_count, int values_offset = 0, const char* overlay_text = NULL, float scale_min = FLT_MAX, float scale_max = FLT_MAX, ImVec2 graph_size = ImVec2(0, 0), int stride = sizeof(float)); </summary>
	public static void PlotLines(string Label, out  float Values, int ValuesCount, int ValuesOffset, string OverlayText, float ScaleMin, float ScaleMax, Vector2 GraphSize, int Stride)
	{
		ImGui_PlotLines646(Label, out Values, ValuesCount, ValuesOffset, OverlayText, ScaleMin, ScaleMax, GraphSize, Stride);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_PlotHistogram648([MarshalAs(UnmanagedType.LPStr)]string Label, out  float Values, int ValuesCount, int ValuesOffset, [MarshalAs(UnmanagedType.LPStr)]string OverlayText, float ScaleMin, float ScaleMax, Vector2 GraphSize, int Stride);

	/// <summary><code>IMGUI_API void          PlotHistogram(const char* label, const float* values, int values_count, int values_offset = 0, const char* overlay_text = NULL, float scale_min = FLT_MAX, float scale_max = FLT_MAX, ImVec2 graph_size = ImVec2(0, 0), int stride = sizeof(float));</code>
		///    IMGUI_API void          PlotHistogram(const char* label, const float* values, int values_count, int values_offset = 0, const char* overlay_text = NULL, float scale_min = FLT_MAX, float scale_max = FLT_MAX, ImVec2 graph_size = ImVec2(0, 0), int stride = sizeof(float)); </summary>
	public static void PlotHistogram(string Label, out  float Values, int ValuesCount)
	{
		Vector2 param7 = new Vector2 (0,  0);
		ImGui_PlotHistogram648(Label, out Values, ValuesCount, (int)0, default, float.MaxValue, float.MaxValue, param7, sizeof(float));
	}

	/// <summary><code>IMGUI_API void          PlotHistogram(const char* label, const float* values, int values_count, int values_offset = 0, const char* overlay_text = NULL, float scale_min = FLT_MAX, float scale_max = FLT_MAX, ImVec2 graph_size = ImVec2(0, 0), int stride = sizeof(float));</code>
		///    IMGUI_API void          PlotHistogram(const char* label, const float* values, int values_count, int values_offset = 0, const char* overlay_text = NULL, float scale_min = FLT_MAX, float scale_max = FLT_MAX, ImVec2 graph_size = ImVec2(0, 0), int stride = sizeof(float)); </summary>
	public static void PlotHistogram(string Label, out  float Values, int ValuesCount, int ValuesOffset)
	{
		Vector2 param7 = new Vector2 (0,  0);
		ImGui_PlotHistogram648(Label, out Values, ValuesCount, ValuesOffset, default, float.MaxValue, float.MaxValue, param7, sizeof(float));
	}

	/// <summary><code>IMGUI_API void          PlotHistogram(const char* label, const float* values, int values_count, int values_offset = 0, const char* overlay_text = NULL, float scale_min = FLT_MAX, float scale_max = FLT_MAX, ImVec2 graph_size = ImVec2(0, 0), int stride = sizeof(float));</code>
		///    IMGUI_API void          PlotHistogram(const char* label, const float* values, int values_count, int values_offset = 0, const char* overlay_text = NULL, float scale_min = FLT_MAX, float scale_max = FLT_MAX, ImVec2 graph_size = ImVec2(0, 0), int stride = sizeof(float)); </summary>
	public static void PlotHistogram(string Label, out  float Values, int ValuesCount, int ValuesOffset, string OverlayText)
	{
		Vector2 param7 = new Vector2 (0,  0);
		ImGui_PlotHistogram648(Label, out Values, ValuesCount, ValuesOffset, OverlayText, float.MaxValue, float.MaxValue, param7, sizeof(float));
	}

	/// <summary><code>IMGUI_API void          PlotHistogram(const char* label, const float* values, int values_count, int values_offset = 0, const char* overlay_text = NULL, float scale_min = FLT_MAX, float scale_max = FLT_MAX, ImVec2 graph_size = ImVec2(0, 0), int stride = sizeof(float));</code>
		///    IMGUI_API void          PlotHistogram(const char* label, const float* values, int values_count, int values_offset = 0, const char* overlay_text = NULL, float scale_min = FLT_MAX, float scale_max = FLT_MAX, ImVec2 graph_size = ImVec2(0, 0), int stride = sizeof(float)); </summary>
	public static void PlotHistogram(string Label, out  float Values, int ValuesCount, int ValuesOffset, string OverlayText, float ScaleMin)
	{
		Vector2 param7 = new Vector2 (0,  0);
		ImGui_PlotHistogram648(Label, out Values, ValuesCount, ValuesOffset, OverlayText, ScaleMin, float.MaxValue, param7, sizeof(float));
	}

	/// <summary><code>IMGUI_API void          PlotHistogram(const char* label, const float* values, int values_count, int values_offset = 0, const char* overlay_text = NULL, float scale_min = FLT_MAX, float scale_max = FLT_MAX, ImVec2 graph_size = ImVec2(0, 0), int stride = sizeof(float));</code>
		///    IMGUI_API void          PlotHistogram(const char* label, const float* values, int values_count, int values_offset = 0, const char* overlay_text = NULL, float scale_min = FLT_MAX, float scale_max = FLT_MAX, ImVec2 graph_size = ImVec2(0, 0), int stride = sizeof(float)); </summary>
	public static void PlotHistogram(string Label, out  float Values, int ValuesCount, int ValuesOffset, string OverlayText, float ScaleMin, float ScaleMax)
	{
		Vector2 param7 = new Vector2 (0,  0);
		ImGui_PlotHistogram648(Label, out Values, ValuesCount, ValuesOffset, OverlayText, ScaleMin, ScaleMax, param7, sizeof(float));
	}

	/// <summary><code>IMGUI_API void          PlotHistogram(const char* label, const float* values, int values_count, int values_offset = 0, const char* overlay_text = NULL, float scale_min = FLT_MAX, float scale_max = FLT_MAX, ImVec2 graph_size = ImVec2(0, 0), int stride = sizeof(float));</code>
		///    IMGUI_API void          PlotHistogram(const char* label, const float* values, int values_count, int values_offset = 0, const char* overlay_text = NULL, float scale_min = FLT_MAX, float scale_max = FLT_MAX, ImVec2 graph_size = ImVec2(0, 0), int stride = sizeof(float)); </summary>
	public static void PlotHistogram(string Label, out  float Values, int ValuesCount, int ValuesOffset, string OverlayText, float ScaleMin, float ScaleMax, Vector2 GraphSize)
	{
		ImGui_PlotHistogram648(Label, out Values, ValuesCount, ValuesOffset, OverlayText, ScaleMin, ScaleMax, GraphSize, sizeof(float));
	}

	/// <summary><code>IMGUI_API void          PlotHistogram(const char* label, const float* values, int values_count, int values_offset = 0, const char* overlay_text = NULL, float scale_min = FLT_MAX, float scale_max = FLT_MAX, ImVec2 graph_size = ImVec2(0, 0), int stride = sizeof(float));</code>
		///    IMGUI_API void          PlotHistogram(const char* label, const float* values, int values_count, int values_offset = 0, const char* overlay_text = NULL, float scale_min = FLT_MAX, float scale_max = FLT_MAX, ImVec2 graph_size = ImVec2(0, 0), int stride = sizeof(float)); </summary>
	public static void PlotHistogram(string Label, out  float Values, int ValuesCount, int ValuesOffset, string OverlayText, float ScaleMin, float ScaleMax, Vector2 GraphSize, int Stride)
	{
		ImGui_PlotHistogram648(Label, out Values, ValuesCount, ValuesOffset, OverlayText, ScaleMin, ScaleMax, GraphSize, Stride);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_Value653([MarshalAs(UnmanagedType.LPStr)]string Prefix, [MarshalAs(UnmanagedType.I1)]bool B);

	/// <summary><code>IMGUI_API void          Value(const char* prefix, bool b);</code>
		///    IMGUI_API void          Value(const char* prefix, bool b); </summary>
	public static void Value(string Prefix, bool B)
	{
		ImGui_Value653(Prefix, B);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_Value654([MarshalAs(UnmanagedType.LPStr)]string Prefix, int V);

	/// <summary><code>IMGUI_API void          Value(const char* prefix, int v);</code>
		///    IMGUI_API void          Value(const char* prefix, int v); </summary>
	public static void Value(string Prefix, int V)
	{
		ImGui_Value654(Prefix, V);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_Value655([MarshalAs(UnmanagedType.LPStr)]string Prefix, uint V);

	/// <summary><code>IMGUI_API void          Value(const char* prefix, unsigned int v);</code>
		///    IMGUI_API void          Value(const char* prefix, unsigned int v); </summary>
	public static void Value(string Prefix, uint V)
	{
		ImGui_Value655(Prefix, V);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_Value656([MarshalAs(UnmanagedType.LPStr)]string Prefix, float V, [MarshalAs(UnmanagedType.LPStr)]string FloatFormat);

	/// <summary><code>IMGUI_API void          Value(const char* prefix, float v, const char* float_format = NULL);</code>
		///    IMGUI_API void          Value(const char* prefix, float v, const char* float_format = NULL); </summary>
	public static void Value(string Prefix, float V)
	{
		ImGui_Value656(Prefix, V, default);
	}

	/// <summary><code>IMGUI_API void          Value(const char* prefix, float v, const char* float_format = NULL);</code>
		///    IMGUI_API void          Value(const char* prefix, float v, const char* float_format = NULL); </summary>
	public static void Value(string Prefix, float V, string FloatFormat)
	{
		ImGui_Value656(Prefix, V, FloatFormat);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_BeginMenuBar663();

	/// <summary><code>IMGUI_API bool          BeginMenuBar();                                                     </code>
		/// append to menu-bar of current window (requires ImGuiWindowFlags_MenuBar flag set on parent window). </summary>
	public static bool BeginMenuBar()
	{
		return ImGui_BeginMenuBar663();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_EndMenuBar664();

	/// <summary><code>IMGUI_API void          EndMenuBar();                                                       </code>
		/// only call EndMenuBar() if BeginMenuBar() returns true! </summary>
	public static void EndMenuBar()
	{
		ImGui_EndMenuBar664();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_BeginMainMenuBar665();

	/// <summary><code>IMGUI_API bool          BeginMainMenuBar();                                                 </code>
		/// create and append to a full screen menu-bar. </summary>
	public static bool BeginMainMenuBar()
	{
		return ImGui_BeginMainMenuBar665();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_EndMainMenuBar666();

	/// <summary><code>IMGUI_API void          EndMainMenuBar();                                                   </code>
		/// only call EndMainMenuBar() if BeginMainMenuBar() returns true! </summary>
	public static void EndMainMenuBar()
	{
		ImGui_EndMainMenuBar666();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_BeginMenu667([MarshalAs(UnmanagedType.LPStr)]string Label, [MarshalAs(UnmanagedType.I1)]bool Enabled);

	/// <summary><code>IMGUI_API bool          BeginMenu(const char* label, bool enabled = true);                  </code>
		/// create a sub-menu entry. only call EndMenu() if this returns true! </summary>
	public static bool BeginMenu(string Label)
	{
		return ImGui_BeginMenu667(Label, true);
	}

	/// <summary><code>IMGUI_API bool          BeginMenu(const char* label, bool enabled = true);                  </code>
		/// create a sub-menu entry. only call EndMenu() if this returns true! </summary>
	public static bool BeginMenu(string Label, bool Enabled)
	{
		return ImGui_BeginMenu667(Label, Enabled);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_EndMenu668();

	/// <summary><code>IMGUI_API void          EndMenu();                                                          </code>
		/// only call EndMenu() if BeginMenu() returns true! </summary>
	public static void EndMenu()
	{
		ImGui_EndMenu668();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_MenuItem669([MarshalAs(UnmanagedType.LPStr)]string Label, [MarshalAs(UnmanagedType.LPStr)]string Shortcut, [MarshalAs(UnmanagedType.I1)]bool Selected, [MarshalAs(UnmanagedType.I1)]bool Enabled);

	/// <summary><code>IMGUI_API bool          MenuItem(const char* label, const char* shortcut = NULL, bool selected = false, bool enabled = true);  </code>
		/// return true when activated. </summary>
	public static bool MenuItem(string Label)
	{
		return ImGui_MenuItem669(Label, default, false, true);
	}

	/// <summary><code>IMGUI_API bool          MenuItem(const char* label, const char* shortcut = NULL, bool selected = false, bool enabled = true);  </code>
		/// return true when activated. </summary>
	public static bool MenuItem(string Label, string Shortcut)
	{
		return ImGui_MenuItem669(Label, Shortcut, false, true);
	}

	/// <summary><code>IMGUI_API bool          MenuItem(const char* label, const char* shortcut = NULL, bool selected = false, bool enabled = true);  </code>
		/// return true when activated. </summary>
	public static bool MenuItem(string Label, string Shortcut, bool Selected)
	{
		return ImGui_MenuItem669(Label, Shortcut, Selected, true);
	}

	/// <summary><code>IMGUI_API bool          MenuItem(const char* label, const char* shortcut = NULL, bool selected = false, bool enabled = true);  </code>
		/// return true when activated. </summary>
	public static bool MenuItem(string Label, string Shortcut, bool Selected, bool Enabled)
	{
		return ImGui_MenuItem669(Label, Shortcut, Selected, Enabled);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_MenuItem670([MarshalAs(UnmanagedType.LPStr)]string Label, [MarshalAs(UnmanagedType.LPStr)]string Shortcut, [MarshalAs(UnmanagedType.I1)]out bool PSelected, [MarshalAs(UnmanagedType.I1)]bool Enabled);

	/// <summary><code>IMGUI_API bool          MenuItem(const char* label, const char* shortcut, bool* p_selected, bool enabled = true);              </code>
		/// return true when activated + toggle (*p_selected) if p_selected != NULL </summary>
	public static bool MenuItem(string Label, string Shortcut, out bool PSelected)
	{
		return ImGui_MenuItem670(Label, Shortcut, out PSelected, true);
	}

	/// <summary><code>IMGUI_API bool          MenuItem(const char* label, const char* shortcut, bool* p_selected, bool enabled = true);              </code>
		/// return true when activated + toggle (*p_selected) if p_selected != NULL </summary>
	public static bool MenuItem(string Label, string Shortcut, out bool PSelected, bool Enabled)
	{
		return ImGui_MenuItem670(Label, Shortcut, out PSelected, Enabled);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_BeginTooltip675();

	/// <summary><code>IMGUI_API bool          BeginTooltip();                                                     </code>
		/// begin/append a tooltip window. </summary>
	public static bool BeginTooltip()
	{
		return ImGui_BeginTooltip675();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_EndTooltip676();

	/// <summary><code>IMGUI_API void          EndTooltip();                                                       </code>
		/// only call EndTooltip() if BeginTooltip()/BeginItemTooltip() returns true! </summary>
	public static void EndTooltip()
	{
		ImGui_EndTooltip676();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_BeginItemTooltip684();

	/// <summary><code>IMGUI_API bool          BeginItemTooltip();                                                 </code>
		/// begin/append a tooltip window if preceding item was hovered. </summary>
	public static bool BeginItemTooltip()
	{
		return ImGui_BeginItemTooltip684();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_BeginPopup700([MarshalAs(UnmanagedType.LPStr)]string StrId, ImGuiWindowFlags Flags);

	/// <summary><code>IMGUI_API bool          BeginPopup(const char* str_id, ImGuiWindowFlags flags = 0);                         </code>
		/// return true if the popup is open, and you can start outputting to it. </summary>
	public static bool BeginPopup(string StrId)
	{
		return ImGui_BeginPopup700(StrId, (ImGuiWindowFlags)0);
	}

	/// <summary><code>IMGUI_API bool          BeginPopup(const char* str_id, ImGuiWindowFlags flags = 0);                         </code>
		/// return true if the popup is open, and you can start outputting to it. </summary>
	public static bool BeginPopup(string StrId, ImGuiWindowFlags Flags)
	{
		return ImGui_BeginPopup700(StrId, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_BeginPopupModal701([MarshalAs(UnmanagedType.LPStr)]string Name, [MarshalAs(UnmanagedType.I1)]out bool POpen, ImGuiWindowFlags Flags);

	/// <summary><code>IMGUI_API bool          BeginPopupModal(const char* name, bool* p_open = NULL, ImGuiWindowFlags flags = 0); </code>
		/// return true if the modal is open, and you can start outputting to it. </summary>
	public static bool BeginPopupModal(string Name)
	{
		return ImGui_BeginPopupModal701(Name, out _, (ImGuiWindowFlags)0);
	}

	/// <summary><code>IMGUI_API bool          BeginPopupModal(const char* name, bool* p_open = NULL, ImGuiWindowFlags flags = 0); </code>
		/// return true if the modal is open, and you can start outputting to it. </summary>
	public static bool BeginPopupModal(string Name, out bool POpen)
	{
		return ImGui_BeginPopupModal701(Name, out POpen, (ImGuiWindowFlags)0);
	}

	/// <summary><code>IMGUI_API bool          BeginPopupModal(const char* name, bool* p_open = NULL, ImGuiWindowFlags flags = 0); </code>
		/// return true if the modal is open, and you can start outputting to it. </summary>
	public static bool BeginPopupModal(string Name, out bool POpen, ImGuiWindowFlags Flags)
	{
		return ImGui_BeginPopupModal701(Name, out POpen, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_EndPopup702();

	/// <summary><code>IMGUI_API void          EndPopup();                                                                         </code>
		/// only call EndPopup() if BeginPopupXXX() returns true! </summary>
	public static void EndPopup()
	{
		ImGui_EndPopup702();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_OpenPopup712([MarshalAs(UnmanagedType.LPStr)]string StrId, ImGuiPopupFlags PopupFlags);

	/// <summary><code>IMGUI_API void          OpenPopup(const char* str_id, ImGuiPopupFlags popup_flags = 0);                     </code>
		/// call to mark popup as open (don't call every frame!). </summary>
	public static void OpenPopup(string StrId)
	{
		ImGui_OpenPopup712(StrId, (ImGuiPopupFlags)0);
	}

	/// <summary><code>IMGUI_API void          OpenPopup(const char* str_id, ImGuiPopupFlags popup_flags = 0);                     </code>
		/// call to mark popup as open (don't call every frame!). </summary>
	public static void OpenPopup(string StrId, ImGuiPopupFlags PopupFlags)
	{
		ImGui_OpenPopup712(StrId, PopupFlags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_OpenPopup713(ImGuiID Id, ImGuiPopupFlags PopupFlags);

	/// <summary><code>IMGUI_API void          OpenPopup(ImGuiID id, ImGuiPopupFlags popup_flags = 0);                             </code>
		/// id overload to facilitate calling from nested stacks </summary>
	public static void OpenPopup(ImGuiID Id)
	{
		ImGui_OpenPopup713(Id, (ImGuiPopupFlags)0);
	}

	/// <summary><code>IMGUI_API void          OpenPopup(ImGuiID id, ImGuiPopupFlags popup_flags = 0);                             </code>
		/// id overload to facilitate calling from nested stacks </summary>
	public static void OpenPopup(ImGuiID Id, ImGuiPopupFlags PopupFlags)
	{
		ImGui_OpenPopup713(Id, PopupFlags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_OpenPopupOnItemClick714([MarshalAs(UnmanagedType.LPStr)]string StrId, ImGuiPopupFlags PopupFlags);

	/// <summary><code>IMGUI_API void          OpenPopupOnItemClick(const char* str_id = NULL, ImGuiPopupFlags popup_flags = 1);   </code>
		/// helper to open popup when clicked on last item. Default to ImGuiPopupFlags_MouseButtonRight == 1. (note: actually triggers on the mouse _released_ event to be consistent with popup behaviors) </summary>
	public static void OpenPopupOnItemClick()
	{
		ImGui_OpenPopupOnItemClick714(default, (ImGuiPopupFlags)1);
	}

	/// <summary><code>IMGUI_API void          OpenPopupOnItemClick(const char* str_id = NULL, ImGuiPopupFlags popup_flags = 1);   </code>
		/// helper to open popup when clicked on last item. Default to ImGuiPopupFlags_MouseButtonRight == 1. (note: actually triggers on the mouse _released_ event to be consistent with popup behaviors) </summary>
	public static void OpenPopupOnItemClick(string StrId)
	{
		ImGui_OpenPopupOnItemClick714(StrId, (ImGuiPopupFlags)1);
	}

	/// <summary><code>IMGUI_API void          OpenPopupOnItemClick(const char* str_id = NULL, ImGuiPopupFlags popup_flags = 1);   </code>
		/// helper to open popup when clicked on last item. Default to ImGuiPopupFlags_MouseButtonRight == 1. (note: actually triggers on the mouse _released_ event to be consistent with popup behaviors) </summary>
	public static void OpenPopupOnItemClick(string StrId, ImGuiPopupFlags PopupFlags)
	{
		ImGui_OpenPopupOnItemClick714(StrId, PopupFlags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_CloseCurrentPopup715();

	/// <summary><code>IMGUI_API void          CloseCurrentPopup();                                                                </code>
		/// manually close the popup we have begin-ed into. </summary>
	public static void CloseCurrentPopup()
	{
		ImGui_CloseCurrentPopup715();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_BeginPopupContextItem722([MarshalAs(UnmanagedType.LPStr)]string StrId, ImGuiPopupFlags PopupFlags);

	/// <summary><code>IMGUI_API bool          BeginPopupContextItem(const char* str_id = NULL, ImGuiPopupFlags popup_flags = 1);  </code>
		/// open+begin popup when clicked on last item. Use str_id==NULL to associate the popup to previous item. If you want to use that on a non-interactive item such as Text() you need to pass in an explicit ID here. read comments in .cpp! </summary>
	public static bool BeginPopupContextItem()
	{
		return ImGui_BeginPopupContextItem722(default, (ImGuiPopupFlags)1);
	}

	/// <summary><code>IMGUI_API bool          BeginPopupContextItem(const char* str_id = NULL, ImGuiPopupFlags popup_flags = 1);  </code>
		/// open+begin popup when clicked on last item. Use str_id==NULL to associate the popup to previous item. If you want to use that on a non-interactive item such as Text() you need to pass in an explicit ID here. read comments in .cpp! </summary>
	public static bool BeginPopupContextItem(string StrId)
	{
		return ImGui_BeginPopupContextItem722(StrId, (ImGuiPopupFlags)1);
	}

	/// <summary><code>IMGUI_API bool          BeginPopupContextItem(const char* str_id = NULL, ImGuiPopupFlags popup_flags = 1);  </code>
		/// open+begin popup when clicked on last item. Use str_id==NULL to associate the popup to previous item. If you want to use that on a non-interactive item such as Text() you need to pass in an explicit ID here. read comments in .cpp! </summary>
	public static bool BeginPopupContextItem(string StrId, ImGuiPopupFlags PopupFlags)
	{
		return ImGui_BeginPopupContextItem722(StrId, PopupFlags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_BeginPopupContextWindow723([MarshalAs(UnmanagedType.LPStr)]string StrId, ImGuiPopupFlags PopupFlags);

	/// <summary><code>IMGUI_API bool          BeginPopupContextWindow(const char* str_id = NULL, ImGuiPopupFlags popup_flags = 1);</code>
		/// open+begin popup when clicked on current window. </summary>
	public static bool BeginPopupContextWindow()
	{
		return ImGui_BeginPopupContextWindow723(default, (ImGuiPopupFlags)1);
	}

	/// <summary><code>IMGUI_API bool          BeginPopupContextWindow(const char* str_id = NULL, ImGuiPopupFlags popup_flags = 1);</code>
		/// open+begin popup when clicked on current window. </summary>
	public static bool BeginPopupContextWindow(string StrId)
	{
		return ImGui_BeginPopupContextWindow723(StrId, (ImGuiPopupFlags)1);
	}

	/// <summary><code>IMGUI_API bool          BeginPopupContextWindow(const char* str_id = NULL, ImGuiPopupFlags popup_flags = 1);</code>
		/// open+begin popup when clicked on current window. </summary>
	public static bool BeginPopupContextWindow(string StrId, ImGuiPopupFlags PopupFlags)
	{
		return ImGui_BeginPopupContextWindow723(StrId, PopupFlags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_BeginPopupContextVoid724([MarshalAs(UnmanagedType.LPStr)]string StrId, ImGuiPopupFlags PopupFlags);

	/// <summary><code>IMGUI_API bool          BeginPopupContextVoid(const char* str_id = NULL, ImGuiPopupFlags popup_flags = 1);  </code>
		/// open+begin popup when clicked in void (where there are no windows). </summary>
	public static bool BeginPopupContextVoid()
	{
		return ImGui_BeginPopupContextVoid724(default, (ImGuiPopupFlags)1);
	}

	/// <summary><code>IMGUI_API bool          BeginPopupContextVoid(const char* str_id = NULL, ImGuiPopupFlags popup_flags = 1);  </code>
		/// open+begin popup when clicked in void (where there are no windows). </summary>
	public static bool BeginPopupContextVoid(string StrId)
	{
		return ImGui_BeginPopupContextVoid724(StrId, (ImGuiPopupFlags)1);
	}

	/// <summary><code>IMGUI_API bool          BeginPopupContextVoid(const char* str_id = NULL, ImGuiPopupFlags popup_flags = 1);  </code>
		/// open+begin popup when clicked in void (where there are no windows). </summary>
	public static bool BeginPopupContextVoid(string StrId, ImGuiPopupFlags PopupFlags)
	{
		return ImGui_BeginPopupContextVoid724(StrId, PopupFlags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_IsPopupOpen730([MarshalAs(UnmanagedType.LPStr)]string StrId, ImGuiPopupFlags Flags);

	/// <summary><code>IMGUI_API bool          IsPopupOpen(const char* str_id, ImGuiPopupFlags flags = 0);                         </code>
		/// return true if the popup is open. </summary>
	public static bool IsPopupOpen(string StrId)
	{
		return ImGui_IsPopupOpen730(StrId, (ImGuiPopupFlags)0);
	}

	/// <summary><code>IMGUI_API bool          IsPopupOpen(const char* str_id, ImGuiPopupFlags flags = 0);                         </code>
		/// return true if the popup is open. </summary>
	public static bool IsPopupOpen(string StrId, ImGuiPopupFlags Flags)
	{
		return ImGui_IsPopupOpen730(StrId, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_BeginTable755([MarshalAs(UnmanagedType.LPStr)]string StrId, int Column, ImGuiTableFlags Flags, out  Vector2 OuterSize, float InnerWidth);

	/// <summary><code>IMGUI_API bool          BeginTable(const char* str_id, int column, ImGuiTableFlags flags = 0, const ImVec2& outer_size = ImVec2(0.0f, 0.0f), float inner_width = 0.0f);</code>
		///    IMGUI_API bool          BeginTable(const char* str_id, int column, ImGuiTableFlags flags = 0, const ImVec2& outer_size = ImVec2(0.0f, 0.0f), float inner_width = 0.0f); </summary>
	public static bool BeginTable(string StrId, int Column)
	{
		 Vector2 param3 = new  Vector2 (0.0f,  0.0f);
		return ImGui_BeginTable755(StrId, Column, (ImGuiTableFlags)0, out param3, (float)0.0f);
	}

	/// <summary><code>IMGUI_API bool          BeginTable(const char* str_id, int column, ImGuiTableFlags flags = 0, const ImVec2& outer_size = ImVec2(0.0f, 0.0f), float inner_width = 0.0f);</code>
		///    IMGUI_API bool          BeginTable(const char* str_id, int column, ImGuiTableFlags flags = 0, const ImVec2& outer_size = ImVec2(0.0f, 0.0f), float inner_width = 0.0f); </summary>
	public static bool BeginTable(string StrId, int Column, ImGuiTableFlags Flags)
	{
		 Vector2 param3 = new  Vector2 (0.0f,  0.0f);
		return ImGui_BeginTable755(StrId, Column, Flags, out param3, (float)0.0f);
	}

	/// <summary><code>IMGUI_API bool          BeginTable(const char* str_id, int column, ImGuiTableFlags flags = 0, const ImVec2& outer_size = ImVec2(0.0f, 0.0f), float inner_width = 0.0f);</code>
		///    IMGUI_API bool          BeginTable(const char* str_id, int column, ImGuiTableFlags flags = 0, const ImVec2& outer_size = ImVec2(0.0f, 0.0f), float inner_width = 0.0f); </summary>
	public static bool BeginTable(string StrId, int Column, ImGuiTableFlags Flags, out  Vector2 OuterSize)
	{
		return ImGui_BeginTable755(StrId, Column, Flags, out OuterSize, (float)0.0f);
	}

	/// <summary><code>IMGUI_API bool          BeginTable(const char* str_id, int column, ImGuiTableFlags flags = 0, const ImVec2& outer_size = ImVec2(0.0f, 0.0f), float inner_width = 0.0f);</code>
		///    IMGUI_API bool          BeginTable(const char* str_id, int column, ImGuiTableFlags flags = 0, const ImVec2& outer_size = ImVec2(0.0f, 0.0f), float inner_width = 0.0f); </summary>
	public static bool BeginTable(string StrId, int Column, ImGuiTableFlags Flags, out  Vector2 OuterSize, float InnerWidth)
	{
		return ImGui_BeginTable755(StrId, Column, Flags, out OuterSize, InnerWidth);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_EndTable756();

	/// <summary><code>IMGUI_API void          EndTable();                                         </code>
		/// only call EndTable() if BeginTable() returns true! </summary>
	public static void EndTable()
	{
		ImGui_EndTable756();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_TableNextRow757(ImGuiTableRowFlags RowFlags, float MinRowHeight);

	/// <summary><code>IMGUI_API void          TableNextRow(ImGuiTableRowFlags row_flags = 0, float min_row_height = 0.0f); </code>
		/// append into the first cell of a new row. </summary>
	public static void TableNextRow()
	{
		ImGui_TableNextRow757((ImGuiTableRowFlags)0, (float)0.0f);
	}

	/// <summary><code>IMGUI_API void          TableNextRow(ImGuiTableRowFlags row_flags = 0, float min_row_height = 0.0f); </code>
		/// append into the first cell of a new row. </summary>
	public static void TableNextRow(ImGuiTableRowFlags RowFlags)
	{
		ImGui_TableNextRow757(RowFlags, (float)0.0f);
	}

	/// <summary><code>IMGUI_API void          TableNextRow(ImGuiTableRowFlags row_flags = 0, float min_row_height = 0.0f); </code>
		/// append into the first cell of a new row. </summary>
	public static void TableNextRow(ImGuiTableRowFlags RowFlags, float MinRowHeight)
	{
		ImGui_TableNextRow757(RowFlags, MinRowHeight);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_TableNextColumn758();

	/// <summary><code>IMGUI_API bool          TableNextColumn();                                  </code>
		/// append into the next column (or first column of next row if currently in last column). Return true when column is visible. </summary>
	public static bool TableNextColumn()
	{
		return ImGui_TableNextColumn758();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_TableSetColumnIndex759(int ColumnN);

	/// <summary><code>IMGUI_API bool          TableSetColumnIndex(int column_n);                  </code>
		/// append into the specified column. Return true when column is visible. </summary>
	public static bool TableSetColumnIndex(int ColumnN)
	{
		return ImGui_TableSetColumnIndex759(ColumnN);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_TableSetupColumn769([MarshalAs(UnmanagedType.LPStr)]string Label, ImGuiTableColumnFlags Flags, float InitWidthOrWeight, ImGuiID UserId);

	/// <summary><code>IMGUI_API void          TableSetupColumn(const char* label, ImGuiTableColumnFlags flags = 0, float init_width_or_weight = 0.0f, ImGuiID user_id = 0);</code>
		///    IMGUI_API void          TableSetupColumn(const char* label, ImGuiTableColumnFlags flags = 0, float init_width_or_weight = 0.0f, ImGuiID user_id = 0); </summary>
	public static void TableSetupColumn(string Label)
	{
		ImGui_TableSetupColumn769(Label, (ImGuiTableColumnFlags)0, (float)0.0f, (ImGuiID)0);
	}

	/// <summary><code>IMGUI_API void          TableSetupColumn(const char* label, ImGuiTableColumnFlags flags = 0, float init_width_or_weight = 0.0f, ImGuiID user_id = 0);</code>
		///    IMGUI_API void          TableSetupColumn(const char* label, ImGuiTableColumnFlags flags = 0, float init_width_or_weight = 0.0f, ImGuiID user_id = 0); </summary>
	public static void TableSetupColumn(string Label, ImGuiTableColumnFlags Flags)
	{
		ImGui_TableSetupColumn769(Label, Flags, (float)0.0f, (ImGuiID)0);
	}

	/// <summary><code>IMGUI_API void          TableSetupColumn(const char* label, ImGuiTableColumnFlags flags = 0, float init_width_or_weight = 0.0f, ImGuiID user_id = 0);</code>
		///    IMGUI_API void          TableSetupColumn(const char* label, ImGuiTableColumnFlags flags = 0, float init_width_or_weight = 0.0f, ImGuiID user_id = 0); </summary>
	public static void TableSetupColumn(string Label, ImGuiTableColumnFlags Flags, float InitWidthOrWeight)
	{
		ImGui_TableSetupColumn769(Label, Flags, InitWidthOrWeight, (ImGuiID)0);
	}

	/// <summary><code>IMGUI_API void          TableSetupColumn(const char* label, ImGuiTableColumnFlags flags = 0, float init_width_or_weight = 0.0f, ImGuiID user_id = 0);</code>
		///    IMGUI_API void          TableSetupColumn(const char* label, ImGuiTableColumnFlags flags = 0, float init_width_or_weight = 0.0f, ImGuiID user_id = 0); </summary>
	public static void TableSetupColumn(string Label, ImGuiTableColumnFlags Flags, float InitWidthOrWeight, ImGuiID UserId)
	{
		ImGui_TableSetupColumn769(Label, Flags, InitWidthOrWeight, UserId);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_TableSetupScrollFreeze770(int Cols, int Rows);

	/// <summary><code>IMGUI_API void          TableSetupScrollFreeze(int cols, int rows);         </code>
		/// lock columns/rows so they stay visible when scrolled. </summary>
	public static void TableSetupScrollFreeze(int Cols, int Rows)
	{
		ImGui_TableSetupScrollFreeze770(Cols, Rows);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_TableHeadersRow771();

	/// <summary><code>IMGUI_API void          TableHeadersRow();                                  </code>
		/// submit all headers cells based on data provided to TableSetupColumn() + submit context menu </summary>
	public static void TableHeadersRow()
	{
		ImGui_TableHeadersRow771();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_TableHeader772([MarshalAs(UnmanagedType.LPStr)]string Label);

	/// <summary><code>IMGUI_API void          TableHeader(const char* label);                     </code>
		/// submit one header cell manually (rarely used) </summary>
	public static void TableHeader(string Label)
	{
		ImGui_TableHeader772(Label);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern IntPtr ImGui_TableGetSortSpecs780();

	/// <summary><code>IMGUI_API ImGuiTableSortSpecs*  TableGetSortSpecs();                        </code>
		/// get latest sort specs for the table (NULL if not sorting).  Lifetime: don't hold on this pointer over multiple frames or past any subsequent call to BeginTable(). </summary>
	public static ImGuiTableSortSpecs TableGetSortSpecs()
	{
		return new ImGuiTableSortSpecs(ImGui_TableGetSortSpecs780());
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern int ImGui_TableGetColumnCount781();

	/// <summary><code>IMGUI_API int                   TableGetColumnCount();                      </code>
		/// return number of columns (value passed to BeginTable) </summary>
	public static int TableGetColumnCount()
	{
		return ImGui_TableGetColumnCount781();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern int ImGui_TableGetColumnIndex782();

	/// <summary><code>IMGUI_API int                   TableGetColumnIndex();                      </code>
		/// return current column index. </summary>
	public static int TableGetColumnIndex()
	{
		return ImGui_TableGetColumnIndex782();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern int ImGui_TableGetRowIndex783();

	/// <summary><code>IMGUI_API int                   TableGetRowIndex();                         </code>
		/// return current row index. </summary>
	public static int TableGetRowIndex()
	{
		return ImGui_TableGetRowIndex783();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.LPStr)]
	private static extern string ImGui_TableGetColumnName784(int ColumnN);

	/// <summary><code>IMGUI_API const char*           TableGetColumnName(int column_n = -1);      </code>
		/// return "" if column didn't have a name declared by TableSetupColumn(). Pass -1 to use current column. </summary>
	public static string TableGetColumnName()
	{
		return ImGui_TableGetColumnName784(-1);
	}

	/// <summary><code>IMGUI_API const char*           TableGetColumnName(int column_n = -1);      </code>
		/// return "" if column didn't have a name declared by TableSetupColumn(). Pass -1 to use current column. </summary>
	public static string TableGetColumnName(int ColumnN)
	{
		return ImGui_TableGetColumnName784(ColumnN);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern ImGuiTableColumnFlags ImGui_TableGetColumnFlags785(int ColumnN);

	/// <summary><code>IMGUI_API ImGuiTableColumnFlags TableGetColumnFlags(int column_n = -1);     </code>
		/// return column flags so you can query their Enabled/Visible/Sorted/Hovered status flags. Pass -1 to use current column. </summary>
	public static ImGuiTableColumnFlags TableGetColumnFlags()
	{
		return ImGui_TableGetColumnFlags785(-1);
	}

	/// <summary><code>IMGUI_API ImGuiTableColumnFlags TableGetColumnFlags(int column_n = -1);     </code>
		/// return column flags so you can query their Enabled/Visible/Sorted/Hovered status flags. Pass -1 to use current column. </summary>
	public static ImGuiTableColumnFlags TableGetColumnFlags(int ColumnN)
	{
		return ImGui_TableGetColumnFlags785(ColumnN);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_TableSetColumnEnabled786(int ColumnN, [MarshalAs(UnmanagedType.I1)]bool V);

	/// <summary><code>IMGUI_API void                  TableSetColumnEnabled(int column_n, bool v);</code>
		/// change user accessible enabled/disabled state of a column. Set to false to hide the column. User can use the context menu to change this themselves (right-click in headers, or right-click in columns body with ImGuiTableFlags_ContextMenuInBody) </summary>
	public static void TableSetColumnEnabled(int ColumnN, bool V)
	{
		ImGui_TableSetColumnEnabled786(ColumnN, V);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_TableSetBgColor787(ImGuiTableBgTarget Target, uint Color, int ColumnN);

	/// <summary><code>IMGUI_API void                  TableSetBgColor(ImGuiTableBgTarget target, ImU32 color, int column_n = -1);  </code>
		/// change the color of a cell, row, or column. See ImGuiTableBgTarget_ flags for details. </summary>
	public static void TableSetBgColor(ImGuiTableBgTarget Target, uint Color)
	{
		ImGui_TableSetBgColor787(Target, Color, -1);
	}

	/// <summary><code>IMGUI_API void                  TableSetBgColor(ImGuiTableBgTarget target, ImU32 color, int column_n = -1);  </code>
		/// change the color of a cell, row, or column. See ImGuiTableBgTarget_ flags for details. </summary>
	public static void TableSetBgColor(ImGuiTableBgTarget Target, uint Color, int ColumnN)
	{
		ImGui_TableSetBgColor787(Target, Color, ColumnN);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_Columns791(int Count, [MarshalAs(UnmanagedType.LPStr)]string Id, [MarshalAs(UnmanagedType.I1)]bool Border);

	/// <summary><code>IMGUI_API void          Columns(int count = 1, const char* id = NULL, bool border = true);</code>
		///    IMGUI_API void          Columns(int count = 1, const char* id = NULL, bool border = true); </summary>
	public static void Columns()
	{
		ImGui_Columns791((int)1, default, true);
	}

	/// <summary><code>IMGUI_API void          Columns(int count = 1, const char* id = NULL, bool border = true);</code>
		///    IMGUI_API void          Columns(int count = 1, const char* id = NULL, bool border = true); </summary>
	public static void Columns(int Count)
	{
		ImGui_Columns791(Count, default, true);
	}

	/// <summary><code>IMGUI_API void          Columns(int count = 1, const char* id = NULL, bool border = true);</code>
		///    IMGUI_API void          Columns(int count = 1, const char* id = NULL, bool border = true); </summary>
	public static void Columns(int Count, string Id)
	{
		ImGui_Columns791(Count, Id, true);
	}

	/// <summary><code>IMGUI_API void          Columns(int count = 1, const char* id = NULL, bool border = true);</code>
		///    IMGUI_API void          Columns(int count = 1, const char* id = NULL, bool border = true); </summary>
	public static void Columns(int Count, string Id, bool Border)
	{
		ImGui_Columns791(Count, Id, Border);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_NextColumn792();

	/// <summary><code>IMGUI_API void          NextColumn();                                                       </code>
		/// next column, defaults to current row or next row if the current row is finished </summary>
	public static void NextColumn()
	{
		ImGui_NextColumn792();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern int ImGui_GetColumnIndex793();

	/// <summary><code>IMGUI_API int           GetColumnIndex();                                                   </code>
		/// get current column index </summary>
	public static int GetColumnIndex()
	{
		return ImGui_GetColumnIndex793();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGui_GetColumnWidth794(int ColumnIndex);

	/// <summary><code>IMGUI_API float         GetColumnWidth(int column_index = -1);                              </code>
		/// get column width (in pixels). pass -1 to use current column </summary>
	public static float GetColumnWidth()
	{
		return ImGui_GetColumnWidth794(-1);
	}

	/// <summary><code>IMGUI_API float         GetColumnWidth(int column_index = -1);                              </code>
		/// get column width (in pixels). pass -1 to use current column </summary>
	public static float GetColumnWidth(int ColumnIndex)
	{
		return ImGui_GetColumnWidth794(ColumnIndex);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_SetColumnWidth795(int ColumnIndex, float Width);

	/// <summary><code>IMGUI_API void          SetColumnWidth(int column_index, float width);                      </code>
		/// set column width (in pixels). pass -1 to use current column </summary>
	public static void SetColumnWidth(int ColumnIndex, float Width)
	{
		ImGui_SetColumnWidth795(ColumnIndex, Width);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGui_GetColumnOffset796(int ColumnIndex);

	/// <summary><code>IMGUI_API float         GetColumnOffset(int column_index = -1);                             </code>
		/// get position of column line (in pixels, from the left side of the contents region). pass -1 to use current column, otherwise 0..GetColumnsCount() inclusive. column 0 is typically 0.0f </summary>
	public static float GetColumnOffset()
	{
		return ImGui_GetColumnOffset796(-1);
	}

	/// <summary><code>IMGUI_API float         GetColumnOffset(int column_index = -1);                             </code>
		/// get position of column line (in pixels, from the left side of the contents region). pass -1 to use current column, otherwise 0..GetColumnsCount() inclusive. column 0 is typically 0.0f </summary>
	public static float GetColumnOffset(int ColumnIndex)
	{
		return ImGui_GetColumnOffset796(ColumnIndex);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_SetColumnOffset797(int ColumnIndex, float OffsetX);

	/// <summary><code>IMGUI_API void          SetColumnOffset(int column_index, float offset_x);                  </code>
		/// set position of column line (in pixels, from the left side of the contents region). pass -1 to use current column </summary>
	public static void SetColumnOffset(int ColumnIndex, float OffsetX)
	{
		ImGui_SetColumnOffset797(ColumnIndex, OffsetX);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern int ImGui_GetColumnsCount798();

	/// <summary><code>IMGUI_API int           GetColumnsCount();</code>
		///    IMGUI_API int           GetColumnsCount(); </summary>
	public static int GetColumnsCount()
	{
		return ImGui_GetColumnsCount798();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_BeginTabBar802([MarshalAs(UnmanagedType.LPStr)]string StrId, ImGuiTabBarFlags Flags);

	/// <summary><code>IMGUI_API bool          BeginTabBar(const char* str_id, ImGuiTabBarFlags flags = 0);        </code>
		/// create and append into a TabBar </summary>
	public static bool BeginTabBar(string StrId)
	{
		return ImGui_BeginTabBar802(StrId, (ImGuiTabBarFlags)0);
	}

	/// <summary><code>IMGUI_API bool          BeginTabBar(const char* str_id, ImGuiTabBarFlags flags = 0);        </code>
		/// create and append into a TabBar </summary>
	public static bool BeginTabBar(string StrId, ImGuiTabBarFlags Flags)
	{
		return ImGui_BeginTabBar802(StrId, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_EndTabBar803();

	/// <summary><code>IMGUI_API void          EndTabBar();                                                        </code>
		/// only call EndTabBar() if BeginTabBar() returns true! </summary>
	public static void EndTabBar()
	{
		ImGui_EndTabBar803();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_BeginTabItem804([MarshalAs(UnmanagedType.LPStr)]string Label, [MarshalAs(UnmanagedType.I1)]out bool POpen, ImGuiTabItemFlags Flags);

	/// <summary><code>IMGUI_API bool          BeginTabItem(const char* label, bool* p_open = NULL, ImGuiTabItemFlags flags = 0); </code>
		/// create a Tab. Returns true if the Tab is selected. </summary>
	public static bool BeginTabItem(string Label)
	{
		return ImGui_BeginTabItem804(Label, out _, (ImGuiTabItemFlags)0);
	}

	/// <summary><code>IMGUI_API bool          BeginTabItem(const char* label, bool* p_open = NULL, ImGuiTabItemFlags flags = 0); </code>
		/// create a Tab. Returns true if the Tab is selected. </summary>
	public static bool BeginTabItem(string Label, out bool POpen)
	{
		return ImGui_BeginTabItem804(Label, out POpen, (ImGuiTabItemFlags)0);
	}

	/// <summary><code>IMGUI_API bool          BeginTabItem(const char* label, bool* p_open = NULL, ImGuiTabItemFlags flags = 0); </code>
		/// create a Tab. Returns true if the Tab is selected. </summary>
	public static bool BeginTabItem(string Label, out bool POpen, ImGuiTabItemFlags Flags)
	{
		return ImGui_BeginTabItem804(Label, out POpen, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_EndTabItem805();

	/// <summary><code>IMGUI_API void          EndTabItem();                                                       </code>
		/// only call EndTabItem() if BeginTabItem() returns true! </summary>
	public static void EndTabItem()
	{
		ImGui_EndTabItem805();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_TabItemButton806([MarshalAs(UnmanagedType.LPStr)]string Label, ImGuiTabItemFlags Flags);

	/// <summary><code>IMGUI_API bool          TabItemButton(const char* label, ImGuiTabItemFlags flags = 0);      </code>
		/// create a Tab behaving like a button. return true when clicked. cannot be selected in the tab bar. </summary>
	public static bool TabItemButton(string Label)
	{
		return ImGui_TabItemButton806(Label, (ImGuiTabItemFlags)0);
	}

	/// <summary><code>IMGUI_API bool          TabItemButton(const char* label, ImGuiTabItemFlags flags = 0);      </code>
		/// create a Tab behaving like a button. return true when clicked. cannot be selected in the tab bar. </summary>
	public static bool TabItemButton(string Label, ImGuiTabItemFlags Flags)
	{
		return ImGui_TabItemButton806(Label, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_SetTabItemClosed807([MarshalAs(UnmanagedType.LPStr)]string TabOrDockedWindowLabel);

	/// <summary><code>IMGUI_API void          SetTabItemClosed(const char* tab_or_docked_window_label);           </code>
		/// notify TabBar or Docking system of a closed tab/window ahead (useful to reduce visual flicker on reorderable tab bars). For tab-bar: call after BeginTabBar() and before Tab submissions. Otherwise call with a window name. </summary>
	public static void SetTabItemClosed(string TabOrDockedWindowLabel)
	{
		ImGui_SetTabItemClosed807(TabOrDockedWindowLabel);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_LogToTTY811(int AutoOpenDepth);

	/// <summary><code>IMGUI_API void          LogToTTY(int auto_open_depth = -1);                                 </code>
		/// start logging to tty (stdout) </summary>
	public static void LogToTTY()
	{
		ImGui_LogToTTY811(-1);
	}

	/// <summary><code>IMGUI_API void          LogToTTY(int auto_open_depth = -1);                                 </code>
		/// start logging to tty (stdout) </summary>
	public static void LogToTTY(int AutoOpenDepth)
	{
		ImGui_LogToTTY811(AutoOpenDepth);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_LogToFile812(int AutoOpenDepth, [MarshalAs(UnmanagedType.LPStr)]string Filename);

	/// <summary><code>IMGUI_API void          LogToFile(int auto_open_depth = -1, const char* filename = NULL);   </code>
		/// start logging to file </summary>
	public static void LogToFile()
	{
		ImGui_LogToFile812(-1, default);
	}

	/// <summary><code>IMGUI_API void          LogToFile(int auto_open_depth = -1, const char* filename = NULL);   </code>
		/// start logging to file </summary>
	public static void LogToFile(int AutoOpenDepth)
	{
		ImGui_LogToFile812(AutoOpenDepth, default);
	}

	/// <summary><code>IMGUI_API void          LogToFile(int auto_open_depth = -1, const char* filename = NULL);   </code>
		/// start logging to file </summary>
	public static void LogToFile(int AutoOpenDepth, string Filename)
	{
		ImGui_LogToFile812(AutoOpenDepth, Filename);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_LogToClipboard813(int AutoOpenDepth);

	/// <summary><code>IMGUI_API void          LogToClipboard(int auto_open_depth = -1);                           </code>
		/// start logging to OS clipboard </summary>
	public static void LogToClipboard()
	{
		ImGui_LogToClipboard813(-1);
	}

	/// <summary><code>IMGUI_API void          LogToClipboard(int auto_open_depth = -1);                           </code>
		/// start logging to OS clipboard </summary>
	public static void LogToClipboard(int AutoOpenDepth)
	{
		ImGui_LogToClipboard813(AutoOpenDepth);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_LogFinish814();

	/// <summary><code>IMGUI_API void          LogFinish();                                                        </code>
		/// stop logging (close file, etc.) </summary>
	public static void LogFinish()
	{
		ImGui_LogFinish814();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_LogButtons815();

	/// <summary><code>IMGUI_API void          LogButtons();                                                       </code>
		/// helper to display buttons for logging to tty/file/clipboard </summary>
	public static void LogButtons()
	{
		ImGui_LogButtons815();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_BeginDragDropSource824(ImGuiDragDropFlags Flags);

	/// <summary><code>IMGUI_API bool          BeginDragDropSource(ImGuiDragDropFlags flags = 0);                                      </code>
		/// call after submitting an item which may be dragged. when this return true, you can call SetDragDropPayload() + EndDragDropSource() </summary>
	public static bool BeginDragDropSource()
	{
		return ImGui_BeginDragDropSource824((ImGuiDragDropFlags)0);
	}

	/// <summary><code>IMGUI_API bool          BeginDragDropSource(ImGuiDragDropFlags flags = 0);                                      </code>
		/// call after submitting an item which may be dragged. when this return true, you can call SetDragDropPayload() + EndDragDropSource() </summary>
	public static bool BeginDragDropSource(ImGuiDragDropFlags Flags)
	{
		return ImGui_BeginDragDropSource824(Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_SetDragDropPayload825([MarshalAs(UnmanagedType.LPStr)]string Type,  IntPtr Data, long Sz, ImGuiCond Cond);

	/// <summary><code>IMGUI_API bool          SetDragDropPayload(const char* type, const void* data, size_t sz, ImGuiCond cond = 0);  </code>
		/// type is a user defined string of maximum 32 characters. Strings starting with '_' are reserved for dear imgui internal types. Data is copied and held by imgui. Return true when payload has been accepted. </summary>
	public static bool SetDragDropPayload(string Type,  IntPtr Data, long Sz)
	{
		return ImGui_SetDragDropPayload825(Type, Data, Sz, (ImGuiCond)0);
	}

	/// <summary><code>IMGUI_API bool          SetDragDropPayload(const char* type, const void* data, size_t sz, ImGuiCond cond = 0);  </code>
		/// type is a user defined string of maximum 32 characters. Strings starting with '_' are reserved for dear imgui internal types. Data is copied and held by imgui. Return true when payload has been accepted. </summary>
	public static bool SetDragDropPayload(string Type,  IntPtr Data, long Sz, ImGuiCond Cond)
	{
		return ImGui_SetDragDropPayload825(Type, Data, Sz, Cond);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_EndDragDropSource826();

	/// <summary><code>IMGUI_API void          EndDragDropSource();                                                                    </code>
		/// only call EndDragDropSource() if BeginDragDropSource() returns true! </summary>
	public static void EndDragDropSource()
	{
		ImGui_EndDragDropSource826();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_BeginDragDropTarget827();

	/// <summary><code>IMGUI_API bool                  BeginDragDropTarget();                                                          </code>
		/// call after submitting an item that may receive a payload. If this returns true, you can call AcceptDragDropPayload() + EndDragDropTarget() </summary>
	public static bool BeginDragDropTarget()
	{
		return ImGui_BeginDragDropTarget827();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern IntPtr ImGui_AcceptDragDropPayload828([MarshalAs(UnmanagedType.LPStr)]string Type, ImGuiDragDropFlags Flags);

	/// <summary><code>IMGUI_API const ImGuiPayload*   AcceptDragDropPayload(const char* type, ImGuiDragDropFlags flags = 0);          </code>
		/// accept contents of a given type. If ImGuiDragDropFlags_AcceptBeforeDelivery is set you can peek into the payload before the mouse button is released. </summary>
	public static  ImGuiPayload AcceptDragDropPayload(string Type)
	{
		return new  ImGuiPayload(ImGui_AcceptDragDropPayload828(Type, (ImGuiDragDropFlags)0));
	}

	/// <summary><code>IMGUI_API const ImGuiPayload*   AcceptDragDropPayload(const char* type, ImGuiDragDropFlags flags = 0);          </code>
		/// accept contents of a given type. If ImGuiDragDropFlags_AcceptBeforeDelivery is set you can peek into the payload before the mouse button is released. </summary>
	public static  ImGuiPayload AcceptDragDropPayload(string Type, ImGuiDragDropFlags Flags)
	{
		return new  ImGuiPayload(ImGui_AcceptDragDropPayload828(Type, Flags));
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_EndDragDropTarget829();

	/// <summary><code>IMGUI_API void                  EndDragDropTarget();                                                            </code>
		/// only call EndDragDropTarget() if BeginDragDropTarget() returns true! </summary>
	public static void EndDragDropTarget()
	{
		ImGui_EndDragDropTarget829();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern IntPtr ImGui_GetDragDropPayload830();

	/// <summary><code>IMGUI_API const ImGuiPayload*   GetDragDropPayload();                                                           </code>
		/// peek directly into the current payload from anywhere. may return NULL. use ImGuiPayload::IsDataType() to test for the payload type. </summary>
	public static  ImGuiPayload GetDragDropPayload()
	{
		return new  ImGuiPayload(ImGui_GetDragDropPayload830());
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_BeginDisabled836([MarshalAs(UnmanagedType.I1)]bool Disabled);

	/// <summary><code>IMGUI_API void          BeginDisabled(bool disabled = true);</code>
		///    IMGUI_API void          BeginDisabled(bool disabled = true); </summary>
	public static void BeginDisabled()
	{
		ImGui_BeginDisabled836(true);
	}

	/// <summary><code>IMGUI_API void          BeginDisabled(bool disabled = true);</code>
		///    IMGUI_API void          BeginDisabled(bool disabled = true); </summary>
	public static void BeginDisabled(bool Disabled)
	{
		ImGui_BeginDisabled836(Disabled);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_EndDisabled837();

	/// <summary><code>IMGUI_API void          EndDisabled();</code>
		///    IMGUI_API void          EndDisabled(); </summary>
	public static void EndDisabled()
	{
		ImGui_EndDisabled837();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_PushClipRect841(out  Vector2 ClipRectMin, out  Vector2 ClipRectMax, [MarshalAs(UnmanagedType.I1)]bool IntersectWithCurrentClipRect);

	/// <summary><code>IMGUI_API void          PushClipRect(const ImVec2& clip_rect_min, const ImVec2& clip_rect_max, bool intersect_with_current_clip_rect);</code>
		///    IMGUI_API void          PushClipRect(const ImVec2& clip_rect_min, const ImVec2& clip_rect_max, bool intersect_with_current_clip_rect); </summary>
	public static void PushClipRect(out  Vector2 ClipRectMin, out  Vector2 ClipRectMax, bool IntersectWithCurrentClipRect)
	{
		ImGui_PushClipRect841(out ClipRectMin, out ClipRectMax, IntersectWithCurrentClipRect);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_PopClipRect842();

	/// <summary><code>IMGUI_API void          PopClipRect();</code>
		///    IMGUI_API void          PopClipRect(); </summary>
	public static void PopClipRect()
	{
		ImGui_PopClipRect842();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_SetItemDefaultFocus846();

	/// <summary><code>IMGUI_API void          SetItemDefaultFocus();                                              </code>
		/// make last item the default focused item of a window. </summary>
	public static void SetItemDefaultFocus()
	{
		ImGui_SetItemDefaultFocus846();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_SetKeyboardFocusHere847(int Offset);

	/// <summary><code>IMGUI_API void          SetKeyboardFocusHere(int offset = 0);                               </code>
		/// focus keyboard on the next widget. Use positive 'offset' to access sub components of a multiple component widget. Use -1 to access previous widget. </summary>
	public static void SetKeyboardFocusHere()
	{
		ImGui_SetKeyboardFocusHere847((int)0);
	}

	/// <summary><code>IMGUI_API void          SetKeyboardFocusHere(int offset = 0);                               </code>
		/// focus keyboard on the next widget. Use positive 'offset' to access sub components of a multiple component widget. Use -1 to access previous widget. </summary>
	public static void SetKeyboardFocusHere(int Offset)
	{
		ImGui_SetKeyboardFocusHere847(Offset);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_SetNextItemAllowOverlap850();

	/// <summary><code>IMGUI_API void          SetNextItemAllowOverlap();                                          </code>
		/// allow next item to be overlapped by a subsequent item. Useful with invisible buttons, selectable, treenode covering an area where subsequent items may need to be added. Note that both Selectable() and TreeNode() have dedicated flags doing this. </summary>
	public static void SetNextItemAllowOverlap()
	{
		ImGui_SetNextItemAllowOverlap850();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_IsItemHovered855(ImGuiHoveredFlags Flags);

	/// <summary><code>IMGUI_API bool          IsItemHovered(ImGuiHoveredFlags flags = 0);                         </code>
		/// is the last item hovered? (and usable, aka not blocked by a popup, etc.). See ImGuiHoveredFlags for more options. </summary>
	public static bool IsItemHovered()
	{
		return ImGui_IsItemHovered855((ImGuiHoveredFlags)0);
	}

	/// <summary><code>IMGUI_API bool          IsItemHovered(ImGuiHoveredFlags flags = 0);                         </code>
		/// is the last item hovered? (and usable, aka not blocked by a popup, etc.). See ImGuiHoveredFlags for more options. </summary>
	public static bool IsItemHovered(ImGuiHoveredFlags Flags)
	{
		return ImGui_IsItemHovered855(Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_IsItemActive856();

	/// <summary><code>IMGUI_API bool          IsItemActive();                                                     </code>
		/// is the last item active? (e.g. button being held, text field being edited. This will continuously return true while holding mouse button on an item. Items that don't interact will always return false) </summary>
	public static bool IsItemActive()
	{
		return ImGui_IsItemActive856();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_IsItemFocused857();

	/// <summary><code>IMGUI_API bool          IsItemFocused();                                                    </code>
		/// is the last item focused for keyboard/gamepad navigation? </summary>
	public static bool IsItemFocused()
	{
		return ImGui_IsItemFocused857();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_IsItemClicked858(ImGuiMouseButton MouseButton);

	/// <summary><code>IMGUI_API bool          IsItemClicked(ImGuiMouseButton mouse_button = 0);                   </code>
		/// is the last item hovered and mouse clicked on? (**)  == IsMouseClicked(mouse_button) && IsItemHovered()Important. (**) this is NOT equivalent to the behavior of e.g. Button(). Read comments in function definition. </summary>
	public static bool IsItemClicked()
	{
		return ImGui_IsItemClicked858((ImGuiMouseButton)0);
	}

	/// <summary><code>IMGUI_API bool          IsItemClicked(ImGuiMouseButton mouse_button = 0);                   </code>
		/// is the last item hovered and mouse clicked on? (**)  == IsMouseClicked(mouse_button) && IsItemHovered()Important. (**) this is NOT equivalent to the behavior of e.g. Button(). Read comments in function definition. </summary>
	public static bool IsItemClicked(ImGuiMouseButton MouseButton)
	{
		return ImGui_IsItemClicked858(MouseButton);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_IsItemVisible859();

	/// <summary><code>IMGUI_API bool          IsItemVisible();                                                    </code>
		/// is the last item visible? (items may be out of sight because of clipping/scrolling) </summary>
	public static bool IsItemVisible()
	{
		return ImGui_IsItemVisible859();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_IsItemEdited860();

	/// <summary><code>IMGUI_API bool          IsItemEdited();                                                     </code>
		/// did the last item modify its underlying value this frame? or was pressed? This is generally the same as the "bool" return value of many widgets. </summary>
	public static bool IsItemEdited()
	{
		return ImGui_IsItemEdited860();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_IsItemActivated861();

	/// <summary><code>IMGUI_API bool          IsItemActivated();                                                  </code>
		/// was the last item just made active (item was previously inactive). </summary>
	public static bool IsItemActivated()
	{
		return ImGui_IsItemActivated861();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_IsItemDeactivated862();

	/// <summary><code>IMGUI_API bool          IsItemDeactivated();                                                </code>
		/// was the last item just made inactive (item was previously active). Useful for Undo/Redo patterns with widgets that require continuous editing. </summary>
	public static bool IsItemDeactivated()
	{
		return ImGui_IsItemDeactivated862();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_IsItemDeactivatedAfterEdit863();

	/// <summary><code>IMGUI_API bool          IsItemDeactivatedAfterEdit();                                       </code>
		/// was the last item just made inactive and made a value change when it was active? (e.g. Slider/Drag moved). Useful for Undo/Redo patterns with widgets that require continuous editing. Note that you may get false positives (some widgets such as Combo()/ListBox()/Selectable() will return true even when clicking an already selected item). </summary>
	public static bool IsItemDeactivatedAfterEdit()
	{
		return ImGui_IsItemDeactivatedAfterEdit863();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_IsItemToggledOpen864();

	/// <summary><code>IMGUI_API bool          IsItemToggledOpen();                                                </code>
		/// was the last item open state toggled? set by TreeNode(). </summary>
	public static bool IsItemToggledOpen()
	{
		return ImGui_IsItemToggledOpen864();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_IsAnyItemHovered865();

	/// <summary><code>IMGUI_API bool          IsAnyItemHovered();                                                 </code>
		/// is any item hovered? </summary>
	public static bool IsAnyItemHovered()
	{
		return ImGui_IsAnyItemHovered865();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_IsAnyItemActive866();

	/// <summary><code>IMGUI_API bool          IsAnyItemActive();                                                  </code>
		/// is any item active? </summary>
	public static bool IsAnyItemActive()
	{
		return ImGui_IsAnyItemActive866();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_IsAnyItemFocused867();

	/// <summary><code>IMGUI_API bool          IsAnyItemFocused();                                                 </code>
		/// is any item focused? </summary>
	public static bool IsAnyItemFocused()
	{
		return ImGui_IsAnyItemFocused867();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern ImGuiID ImGui_GetItemID868();

	/// <summary><code>IMGUI_API ImGuiID       GetItemID();                                                        </code>
		/// get ID of last item (~~ often same ImGui::GetID(label) beforehand) </summary>
	public static ImGuiID GetItemID()
	{
		return ImGui_GetItemID868();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern Vector2 ImGui_GetItemRectMin869();

	/// <summary><code>IMGUI_API ImVec2        GetItemRectMin();                                                   </code>
		/// get upper-left bounding rectangle of the last item (screen space) </summary>
	public static Vector2 GetItemRectMin()
	{
		return ImGui_GetItemRectMin869();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
    private static extern Vector2 ImGui_GetItemRectMax870();

	/// <summary><code>IMGUI_API ImVec2        GetItemRectMax();                                                   </code>
		/// get lower-right bounding rectangle of the last item (screen space) </summary>
	public static Vector2 GetItemRectMax()
	{
		return ImGui_GetItemRectMax870();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern Vector2 ImGui_GetItemRectSize871();

	/// <summary><code>IMGUI_API ImVec2        GetItemRectSize();                                                  </code>
		/// get size of last item </summary>
	public static Vector2 GetItemRectSize()
	{
		return ImGui_GetItemRectSize871();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern IntPtr ImGui_GetMainViewport877();

	/// <summary><code>IMGUI_API ImGuiViewport* GetMainViewport();                                                 </code>
		/// return primary/default viewport. This can never be NULL. </summary>
	public static ImGuiViewport GetMainViewport()
	{
		return new ImGuiViewport(ImGui_GetMainViewport877());
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern IntPtr ImGui_GetBackgroundDrawList880();

	/// <summary><code>IMGUI_API ImDrawList*   GetBackgroundDrawList();                                            </code>
		/// this draw list will be the first rendered one. Useful to quickly draw shapes/text behind dear imgui contents. </summary>
	public static ImDrawList GetBackgroundDrawList()
	{
		return new ImDrawList(ImGui_GetBackgroundDrawList880());
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern IntPtr ImGui_GetForegroundDrawList881();

	/// <summary><code>IMGUI_API ImDrawList*   GetForegroundDrawList();                                            </code>
		/// this draw list will be the last rendered one. Useful to quickly draw shapes/text over dear imgui contents. </summary>
	public static ImDrawList GetForegroundDrawList()
	{
		return new ImDrawList(ImGui_GetForegroundDrawList881());
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_IsRectVisible884(out  Vector2 Size);

	/// <summary><code>IMGUI_API bool          IsRectVisible(const ImVec2& size);                                  </code>
		/// test if rectangle (of given size, starting from cursor position) is visible / not clipped. </summary>
	public static bool IsRectVisible(out  Vector2 Size)
	{
		return ImGui_IsRectVisible884(out Size);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_IsRectVisible885(out  Vector2 RectMin, out  Vector2 RectMax);

	/// <summary><code>IMGUI_API bool          IsRectVisible(const ImVec2& rect_min, const ImVec2& rect_max);      </code>
		/// test if rectangle (in screen space) is visible / not clipped. to perform coarse clipping on user's side. </summary>
	public static bool IsRectVisible(out  Vector2 RectMin, out  Vector2 RectMax)
	{
		return ImGui_IsRectVisible885(out RectMin, out RectMax);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern double ImGui_GetTime886();

	/// <summary><code>IMGUI_API double        GetTime();                                                          </code>
		/// get global imgui time. incremented by io.DeltaTime every frame. </summary>
	public static double GetTime()
	{
		return ImGui_GetTime886();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern int ImGui_GetFrameCount887();

	/// <summary><code>IMGUI_API int           GetFrameCount();                                                    </code>
		/// get global imgui frame count. incremented by 1 every frame. </summary>
	public static int GetFrameCount()
	{
		return ImGui_GetFrameCount887();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.LPStr)]
	private static extern string ImGui_GetStyleColorName889(ImGuiCol Idx);

	/// <summary><code>IMGUI_API const char*   GetStyleColorName(ImGuiCol idx);                                    </code>
		/// get a string corresponding to the enum value (for display, saving, etc.). </summary>
	public static string GetStyleColorName(ImGuiCol Idx)
	{
		return ImGui_GetStyleColorName889(Idx);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_SetStateStorage890(IntPtr Storage);

	/// <summary><code>IMGUI_API void          SetStateStorage(ImGuiStorage* storage);                             </code>
		/// replace current window storage with our own (if you want to manipulate it yourself, typically clear subsection of it) </summary>
	public static void SetStateStorage(ImGuiStorage Storage)
	{
		ImGui_SetStateStorage890(Storage.AsPtr);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern IntPtr ImGui_GetStateStorage891();

	/// <summary><code>IMGUI_API ImGuiStorage* GetStateStorage();</code>
		///    IMGUI_API ImGuiStorage* GetStateStorage(); </summary>
	public static ImGuiStorage GetStateStorage()
	{
		return new ImGuiStorage(ImGui_GetStateStorage891());
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_BeginChildFrame892(ImGuiID Id, out  Vector2 Size, ImGuiWindowFlags Flags);

	/// <summary><code>IMGUI_API bool          BeginChildFrame(ImGuiID id, const ImVec2& size, ImGuiWindowFlags flags = 0); </code>
		/// helper to create a child window / scrolling region that looks like a normal widget frame </summary>
	public static bool BeginChildFrame(ImGuiID Id, out  Vector2 Size)
	{
		return ImGui_BeginChildFrame892(Id, out Size, (ImGuiWindowFlags)0);
	}

	/// <summary><code>IMGUI_API bool          BeginChildFrame(ImGuiID id, const ImVec2& size, ImGuiWindowFlags flags = 0); </code>
		/// helper to create a child window / scrolling region that looks like a normal widget frame </summary>
	public static bool BeginChildFrame(ImGuiID Id, out  Vector2 Size, ImGuiWindowFlags Flags)
	{
		return ImGui_BeginChildFrame892(Id, out Size, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_EndChildFrame893();

	/// <summary><code>IMGUI_API void          EndChildFrame();                                                    </code>
		/// always call EndChildFrame() regardless of BeginChildFrame() return values (which indicates a collapsed/clipped window) </summary>
	public static void EndChildFrame()
	{
		ImGui_EndChildFrame893();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern Vector2 ImGui_CalcTextSize896([MarshalAs(UnmanagedType.LPStr)]string Text, [MarshalAs(UnmanagedType.LPStr)]string TextEnd, [MarshalAs(UnmanagedType.I1)]bool HideTextAfterDoubleHash, float WrapWidth);

	/// <summary><code>IMGUI_API ImVec2        CalcTextSize(const char* text, const char* text_end = NULL, bool hide_text_after_double_hash = false, float wrap_width = -1.0f);</code>
		///    IMGUI_API ImVec2        CalcTextSize(const char* text, const char* text_end = NULL, bool hide_text_after_double_hash = false, float wrap_width = -1.0f); </summary>
	public static Vector2 CalcTextSize(string Text)
	{
		return ImGui_CalcTextSize896(Text, default, false, -1.0f);
	}

	/// <summary><code>IMGUI_API ImVec2        CalcTextSize(const char* text, const char* text_end = NULL, bool hide_text_after_double_hash = false, float wrap_width = -1.0f);</code>
		///    IMGUI_API ImVec2        CalcTextSize(const char* text, const char* text_end = NULL, bool hide_text_after_double_hash = false, float wrap_width = -1.0f); </summary>
	public static Vector2 CalcTextSize(string Text, string TextEnd)
	{
		return ImGui_CalcTextSize896(Text, TextEnd, false, -1.0f);
	}

	/// <summary><code>IMGUI_API ImVec2        CalcTextSize(const char* text, const char* text_end = NULL, bool hide_text_after_double_hash = false, float wrap_width = -1.0f);</code>
		///    IMGUI_API ImVec2        CalcTextSize(const char* text, const char* text_end = NULL, bool hide_text_after_double_hash = false, float wrap_width = -1.0f); </summary>
	public static Vector2 CalcTextSize(string Text, string TextEnd, bool HideTextAfterDoubleHash)
	{
		return ImGui_CalcTextSize896(Text, TextEnd, HideTextAfterDoubleHash, -1.0f);
	}

	/// <summary><code>IMGUI_API ImVec2        CalcTextSize(const char* text, const char* text_end = NULL, bool hide_text_after_double_hash = false, float wrap_width = -1.0f);</code>
		///    IMGUI_API ImVec2        CalcTextSize(const char* text, const char* text_end = NULL, bool hide_text_after_double_hash = false, float wrap_width = -1.0f); </summary>
	public static Vector2 CalcTextSize(string Text, string TextEnd, bool HideTextAfterDoubleHash, float WrapWidth)
	{
		return ImGui_CalcTextSize896(Text, TextEnd, HideTextAfterDoubleHash, WrapWidth);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern Vector4 ImGui_ColorConvertU32ToFloat4899(uint In);

	/// <summary><code>IMGUI_API ImVec4        ColorConvertU32ToFloat4(ImU32 in);</code>
		///    IMGUI_API ImVec4        ColorConvertU32ToFloat4(ImU32 in); </summary>
	public static Vector4 ColorConvertU32ToFloat4(uint In)
	{
		return ImGui_ColorConvertU32ToFloat4899(In);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern uint ImGui_ColorConvertFloat4ToU32900(out  Vector4 In);

	/// <summary><code>IMGUI_API ImU32         ColorConvertFloat4ToU32(const ImVec4& in);</code>
		///    IMGUI_API ImU32         ColorConvertFloat4ToU32(const ImVec4& in); </summary>
	public static uint ColorConvertFloat4ToU32(out  Vector4 In)
	{
		return ImGui_ColorConvertFloat4ToU32900(out In);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_ColorConvertRGBtoHSV901(float R, float G, float B, ref float OutH, ref float OutS, ref float OutV);

	/// <summary><code>IMGUI_API void          ColorConvertRGBtoHSV(float r, float g, float b, float& out_h, float& out_s, float& out_v);</code>
		///    IMGUI_API void          ColorConvertRGBtoHSV(float r, float g, float b, float& out_h, float& out_s, float& out_v); </summary>
	public static void ColorConvertRGBtoHSV(float R, float G, float B, ref float OutH, ref float OutS, ref float OutV)
	{
		ImGui_ColorConvertRGBtoHSV901(R, G, B, ref OutH, ref OutS, ref OutV);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_ColorConvertHSVtoRGB902(float H, float S, float V, ref float OutR, ref float OutG, ref float OutB);

	/// <summary><code>IMGUI_API void          ColorConvertHSVtoRGB(float h, float s, float v, float& out_r, float& out_g, float& out_b);</code>
		///    IMGUI_API void          ColorConvertHSVtoRGB(float h, float s, float v, float& out_r, float& out_g, float& out_b); </summary>
	public static void ColorConvertHSVtoRGB(float H, float S, float V, ref float OutR, ref float OutG, ref float OutB)
	{
		ImGui_ColorConvertHSVtoRGB902(H, S, V, ref OutR, ref OutG, ref OutB);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_IsKeyDown909(ImGuiKey Key);

	/// <summary><code>IMGUI_API bool          IsKeyDown(ImGuiKey key);                                            </code>
		/// is key being held. </summary>
	public static bool IsKeyDown(ImGuiKey Key)
	{
		return ImGui_IsKeyDown909(Key);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_IsKeyPressed910(ImGuiKey Key, [MarshalAs(UnmanagedType.I1)]bool Repeat);

	/// <summary><code>IMGUI_API bool          IsKeyPressed(ImGuiKey key, bool repeat = true);                     </code>
		/// was key pressed (went from !Down to Down)? if repeat=true, uses io.KeyRepeatDelay / KeyRepeatRate </summary>
	public static bool IsKeyPressed(ImGuiKey Key)
	{
		return ImGui_IsKeyPressed910(Key, true);
	}

	/// <summary><code>IMGUI_API bool          IsKeyPressed(ImGuiKey key, bool repeat = true);                     </code>
		/// was key pressed (went from !Down to Down)? if repeat=true, uses io.KeyRepeatDelay / KeyRepeatRate </summary>
	public static bool IsKeyPressed(ImGuiKey Key, bool Repeat)
	{
		return ImGui_IsKeyPressed910(Key, Repeat);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_IsKeyReleased911(ImGuiKey Key);

	/// <summary><code>IMGUI_API bool          IsKeyReleased(ImGuiKey key);                                        </code>
		/// was key released (went from Down to !Down)? </summary>
	public static bool IsKeyReleased(ImGuiKey Key)
	{
		return ImGui_IsKeyReleased911(Key);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern int ImGui_GetKeyPressedAmount912(ImGuiKey Key, float RepeatDelay, float Rate);

	/// <summary><code>IMGUI_API int           GetKeyPressedAmount(ImGuiKey key, float repeat_delay, float rate);  </code>
		/// uses provided repeat rate/delay. return a count, most often 0 or 1 but might be >1 if RepeatRate is small enough that DeltaTime > RepeatRate </summary>
	public static int GetKeyPressedAmount(ImGuiKey Key, float RepeatDelay, float Rate)
	{
		return ImGui_GetKeyPressedAmount912(Key, RepeatDelay, Rate);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.LPStr)]
	private static extern string ImGui_GetKeyName913(ImGuiKey Key);

	/// <summary><code>IMGUI_API const char*   GetKeyName(ImGuiKey key);                                           </code>
		/// [DEBUG] returns English name of the key. Those names a provided for debugging purpose and are not meant to be saved persistently not compared. </summary>
	public static string GetKeyName(ImGuiKey Key)
	{
		return ImGui_GetKeyName913(Key);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_SetNextFrameWantCaptureKeyboard914([MarshalAs(UnmanagedType.I1)]bool WantCaptureKeyboard);

	/// <summary><code>IMGUI_API void          SetNextFrameWantCaptureKeyboard(bool want_capture_keyboard);        </code>
		/// Override io.WantCaptureKeyboard flag next frame (said flag is left for your application to handle, typically when true it instructs your app to ignore inputs). e.g. force capture keyboard when your widget is being hovered. This is equivalent to setting "io.WantCaptureKeyboard = want_capture_keyboard"; after the next NewFrame() call. </summary>
	public static void SetNextFrameWantCaptureKeyboard(bool WantCaptureKeyboard)
	{
		ImGui_SetNextFrameWantCaptureKeyboard914(WantCaptureKeyboard);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_IsMouseDown920(ImGuiMouseButton Button);

	/// <summary><code>IMGUI_API bool          IsMouseDown(ImGuiMouseButton button);                               </code>
		/// is mouse button held? </summary>
	public static bool IsMouseDown(ImGuiMouseButton Button)
	{
		return ImGui_IsMouseDown920(Button);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_IsMouseClicked921(ImGuiMouseButton Button, [MarshalAs(UnmanagedType.I1)]bool Repeat);

	/// <summary><code>IMGUI_API bool          IsMouseClicked(ImGuiMouseButton button, bool repeat = false);       </code>
		/// did mouse button clicked? (went from !Down to Down). Same as GetMouseClickedCount() == 1. </summary>
	public static bool IsMouseClicked(ImGuiMouseButton Button)
	{
		return ImGui_IsMouseClicked921(Button, false);
	}

	/// <summary><code>IMGUI_API bool          IsMouseClicked(ImGuiMouseButton button, bool repeat = false);       </code>
		/// did mouse button clicked? (went from !Down to Down). Same as GetMouseClickedCount() == 1. </summary>
	public static bool IsMouseClicked(ImGuiMouseButton Button, bool Repeat)
	{
		return ImGui_IsMouseClicked921(Button, Repeat);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_IsMouseReleased922(ImGuiMouseButton Button);

	/// <summary><code>IMGUI_API bool          IsMouseReleased(ImGuiMouseButton button);                           </code>
		/// did mouse button released? (went from Down to !Down) </summary>
	public static bool IsMouseReleased(ImGuiMouseButton Button)
	{
		return ImGui_IsMouseReleased922(Button);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_IsMouseDoubleClicked923(ImGuiMouseButton Button);

	/// <summary><code>IMGUI_API bool          IsMouseDoubleClicked(ImGuiMouseButton button);                      </code>
		/// did mouse button double-clicked? Same as GetMouseClickedCount() == 2. (note that a double-click will also report IsMouseClicked() == true) </summary>
	public static bool IsMouseDoubleClicked(ImGuiMouseButton Button)
	{
		return ImGui_IsMouseDoubleClicked923(Button);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern int ImGui_GetMouseClickedCount924(ImGuiMouseButton Button);

	/// <summary><code>IMGUI_API int           GetMouseClickedCount(ImGuiMouseButton button);                      </code>
		/// return the number of successive mouse-clicks at the time where a click happen (otherwise 0). </summary>
	public static int GetMouseClickedCount(ImGuiMouseButton Button)
	{
		return ImGui_GetMouseClickedCount924(Button);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_IsMouseHoveringRect925(out  Vector2 RMin, out  Vector2 RMax, [MarshalAs(UnmanagedType.I1)]bool Clip);

	/// <summary><code>IMGUI_API bool          IsMouseHoveringRect(const ImVec2& r_min, const ImVec2& r_max, bool clip = true);</code>
		/// is mouse hovering given bounding rect (in screen space). clipped by current clipping settings, but disregarding of other consideration of focus/window ordering/popup-block. </summary>
	public static bool IsMouseHoveringRect(out  Vector2 RMin, out  Vector2 RMax)
	{
		return ImGui_IsMouseHoveringRect925(out RMin, out RMax, true);
	}

	/// <summary><code>IMGUI_API bool          IsMouseHoveringRect(const ImVec2& r_min, const ImVec2& r_max, bool clip = true);</code>
		/// is mouse hovering given bounding rect (in screen space). clipped by current clipping settings, but disregarding of other consideration of focus/window ordering/popup-block. </summary>
	public static bool IsMouseHoveringRect(out  Vector2 RMin, out  Vector2 RMax, bool Clip)
	{
		return ImGui_IsMouseHoveringRect925(out RMin, out RMax, Clip);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_IsMousePosValid926(out  Vector2 MousePos);

	/// <summary><code>IMGUI_API bool          IsMousePosValid(const ImVec2* mouse_pos = NULL);                    </code>
		/// by convention we use (-FLT_MAX,-FLT_MAX) to denote that there is no mouse available </summary>
	public static bool IsMousePosValid()
	{
		return ImGui_IsMousePosValid926(out _);
	}

	/// <summary><code>IMGUI_API bool          IsMousePosValid(const ImVec2* mouse_pos = NULL);                    </code>
		/// by convention we use (-FLT_MAX,-FLT_MAX) to denote that there is no mouse available </summary>
	public static bool IsMousePosValid(out  Vector2 MousePos)
	{
		return ImGui_IsMousePosValid926(out MousePos);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_IsAnyMouseDown927();

	/// <summary><code>IMGUI_API bool          IsAnyMouseDown();                                                   </code>
		/// [WILL OBSOLETE] is any mouse button held? This was designed for backends, but prefer having backend maintain a mask of held mouse buttons, because upcoming input queue system will make this invalid. </summary>
	public static bool IsAnyMouseDown()
	{
		return ImGui_IsAnyMouseDown927();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern Vector2 ImGui_GetMousePos928();

	/// <summary><code>IMGUI_API ImVec2        GetMousePos();                                                      </code>
		/// shortcut to ImGui::GetIO().MousePos provided by user, to be consistent with other calls </summary>
	public static Vector2 GetMousePos()
	{
		return ImGui_GetMousePos928();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern Vector2 ImGui_GetMousePosOnOpeningCurrentPopup929();

	/// <summary><code>IMGUI_API ImVec2        GetMousePosOnOpeningCurrentPopup();                                 </code>
		/// retrieve mouse position at the time of opening popup we have BeginPopup() into (helper to avoid user backing that value themselves) </summary>
	public static Vector2 GetMousePosOnOpeningCurrentPopup()
	{
		return ImGui_GetMousePosOnOpeningCurrentPopup929();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_IsMouseDragging930(ImGuiMouseButton Button, float LockThreshold);

	/// <summary><code>IMGUI_API bool          IsMouseDragging(ImGuiMouseButton button, float lock_threshold = -1.0f);         </code>
		/// is mouse dragging? (if lock_threshold < -1.0f, uses io.MouseDraggingThreshold) </summary>
	public static bool IsMouseDragging(ImGuiMouseButton Button)
	{
		return ImGui_IsMouseDragging930(Button, -1.0f);
	}

	/// <summary><code>IMGUI_API bool          IsMouseDragging(ImGuiMouseButton button, float lock_threshold = -1.0f);         </code>
		/// is mouse dragging? (if lock_threshold < -1.0f, uses io.MouseDraggingThreshold) </summary>
	public static bool IsMouseDragging(ImGuiMouseButton Button, float LockThreshold)
	{
		return ImGui_IsMouseDragging930(Button, LockThreshold);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern Vector2 ImGui_GetMouseDragDelta931(ImGuiMouseButton Button, float LockThreshold);

	/// <summary><code>IMGUI_API ImVec2        GetMouseDragDelta(ImGuiMouseButton button = 0, float lock_threshold = -1.0f);   </code>
		/// return the delta from the initial clicking position while the mouse button is pressed or was just released. This is locked and return 0.0f until the mouse moves past a distance threshold at least once (if lock_threshold < -1.0f, uses io.MouseDraggingThreshold) </summary>
	public static Vector2 GetMouseDragDelta()
	{
		return ImGui_GetMouseDragDelta931((ImGuiMouseButton)0, -1.0f);
	}

	/// <summary><code>IMGUI_API ImVec2        GetMouseDragDelta(ImGuiMouseButton button = 0, float lock_threshold = -1.0f);   </code>
		/// return the delta from the initial clicking position while the mouse button is pressed or was just released. This is locked and return 0.0f until the mouse moves past a distance threshold at least once (if lock_threshold < -1.0f, uses io.MouseDraggingThreshold) </summary>
	public static Vector2 GetMouseDragDelta(ImGuiMouseButton Button)
	{
		return ImGui_GetMouseDragDelta931(Button, -1.0f);
	}

	/// <summary><code>IMGUI_API ImVec2        GetMouseDragDelta(ImGuiMouseButton button = 0, float lock_threshold = -1.0f);   </code>
		/// return the delta from the initial clicking position while the mouse button is pressed or was just released. This is locked and return 0.0f until the mouse moves past a distance threshold at least once (if lock_threshold < -1.0f, uses io.MouseDraggingThreshold) </summary>
	public static Vector2 GetMouseDragDelta(ImGuiMouseButton Button, float LockThreshold)
	{
		return ImGui_GetMouseDragDelta931(Button, LockThreshold);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_ResetMouseDragDelta932(ImGuiMouseButton Button);

	/// <summary><code>IMGUI_API void          ResetMouseDragDelta(ImGuiMouseButton button = 0);                   </code>
		/// </summary>
	public static void ResetMouseDragDelta()
	{
		ImGui_ResetMouseDragDelta932((ImGuiMouseButton)0);
	}

	/// <summary><code>IMGUI_API void          ResetMouseDragDelta(ImGuiMouseButton button = 0);                   </code>
		/// </summary>
	public static void ResetMouseDragDelta(ImGuiMouseButton Button)
	{
		ImGui_ResetMouseDragDelta932(Button);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern ImGuiMouseCursor ImGui_GetMouseCursor933();

	/// <summary><code>IMGUI_API ImGuiMouseCursor GetMouseCursor();                                                </code>
		/// get desired mouse cursor shape. Important: reset in ImGui::NewFrame(), this is updated during the frame. valid before Render(). If you use software rendering by setting io.MouseDrawCursor ImGui will render those for you </summary>
	public static ImGuiMouseCursor GetMouseCursor()
	{
		return ImGui_GetMouseCursor933();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_SetMouseCursor934(ImGuiMouseCursor CursorType);

	/// <summary><code>IMGUI_API void          SetMouseCursor(ImGuiMouseCursor cursor_type);                       </code>
		/// set desired mouse cursor shape </summary>
	public static void SetMouseCursor(ImGuiMouseCursor CursorType)
	{
		ImGui_SetMouseCursor934(CursorType);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_SetNextFrameWantCaptureMouse935([MarshalAs(UnmanagedType.I1)]bool WantCaptureMouse);

	/// <summary><code>IMGUI_API void          SetNextFrameWantCaptureMouse(bool want_capture_mouse);              </code>
		/// Override io.WantCaptureMouse flag next frame (said flag is left for your application to handle, typical when true it instucts your app to ignore inputs). This is equivalent to setting "io.WantCaptureMouse = want_capture_mouse;" after the next NewFrame() call. </summary>
	public static void SetNextFrameWantCaptureMouse(bool WantCaptureMouse)
	{
		ImGui_SetNextFrameWantCaptureMouse935(WantCaptureMouse);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.LPStr)]
	private static extern string ImGui_GetClipboardText939();

	/// <summary><code>IMGUI_API const char*   GetClipboardText();</code>
		///    IMGUI_API const char*   GetClipboardText(); </summary>
	public static string GetClipboardText()
	{
		return ImGui_GetClipboardText939();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_SetClipboardText940([MarshalAs(UnmanagedType.LPStr)]string Text);

	/// <summary><code>IMGUI_API void          SetClipboardText(const char* text);</code>
		///    IMGUI_API void          SetClipboardText(const char* text); </summary>
	public static void SetClipboardText(string Text)
	{
		ImGui_SetClipboardText940(Text);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_LoadIniSettingsFromDisk946([MarshalAs(UnmanagedType.LPStr)]string IniFilename);

	/// <summary><code>IMGUI_API void          LoadIniSettingsFromDisk(const char* ini_filename);                  </code>
		/// call after CreateContext() and before the first call to NewFrame(). NewFrame() automatically calls LoadIniSettingsFromDisk(io.IniFilename). </summary>
	public static void LoadIniSettingsFromDisk(string IniFilename)
	{
		ImGui_LoadIniSettingsFromDisk946(IniFilename);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_LoadIniSettingsFromMemory947([MarshalAs(UnmanagedType.LPStr)]string IniData, long IniSize);

	/// <summary><code>IMGUI_API void          LoadIniSettingsFromMemory(const char* ini_data, size_t ini_size=0); </code>
		/// call after CreateContext() and before the first call to NewFrame() to provide .ini data from your own data source. </summary>
	public static void LoadIniSettingsFromMemory(string IniData)
	{
		ImGui_LoadIniSettingsFromMemory947(IniData, (long)0);
	}

	/// <summary><code>IMGUI_API void          LoadIniSettingsFromMemory(const char* ini_data, size_t ini_size=0); </code>
		/// call after CreateContext() and before the first call to NewFrame() to provide .ini data from your own data source. </summary>
	public static void LoadIniSettingsFromMemory(string IniData, long IniSize)
	{
		ImGui_LoadIniSettingsFromMemory947(IniData, IniSize);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_SaveIniSettingsToDisk948([MarshalAs(UnmanagedType.LPStr)]string IniFilename);

	/// <summary><code>IMGUI_API void          SaveIniSettingsToDisk(const char* ini_filename);                    </code>
		/// this is automatically called (if io.IniFilename is not empty) a few seconds after any modification that should be reflected in the .ini file (and also by DestroyContext). </summary>
	public static void SaveIniSettingsToDisk(string IniFilename)
	{
		ImGui_SaveIniSettingsToDisk948(IniFilename);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.LPStr)]
	private static extern string ImGui_SaveIniSettingsToMemory949(out long OutIniSize);

	/// <summary><code>IMGUI_API const char*   SaveIniSettingsToMemory(size_t* out_ini_size = NULL);               </code>
		/// return a zero-terminated string with the .ini data which you can save by your own mean. call when io.WantSaveIniSettings is set, then save data by your own mean and clear io.WantSaveIniSettings. </summary>
	public static string SaveIniSettingsToMemory()
	{
		return ImGui_SaveIniSettingsToMemory949(out _);
	}

	/// <summary><code>IMGUI_API const char*   SaveIniSettingsToMemory(size_t* out_ini_size = NULL);               </code>
		/// return a zero-terminated string with the .ini data which you can save by your own mean. call when io.WantSaveIniSettings is set, then save data by your own mean and clear io.WantSaveIniSettings. </summary>
	public static string SaveIniSettingsToMemory(out long OutIniSize)
	{
		return ImGui_SaveIniSettingsToMemory949(out OutIniSize);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_DebugTextEncoding952([MarshalAs(UnmanagedType.LPStr)]string Text);

	/// <summary><code>IMGUI_API void          DebugTextEncoding(const char* text);</code>
		///    IMGUI_API void          DebugTextEncoding(const char* text); </summary>
	public static void DebugTextEncoding(string Text)
	{
		ImGui_DebugTextEncoding952(Text);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_DebugCheckVersionAndDataLayout953([MarshalAs(UnmanagedType.LPStr)]string VersionStr, long SzIo, long SzStyle, long SzVec2, long SzVec4, long SzDrawvert, long SzDrawidx);

	/// <summary><code>IMGUI_API bool          DebugCheckVersionAndDataLayout(const char* version_str, size_t sz_io, size_t sz_style, size_t sz_vec2, size_t sz_vec4, size_t sz_drawvert, size_t sz_drawidx); </code>
		/// This is called by IMGUI_CHECKVERSION() macro. </summary>
	public static bool DebugCheckVersionAndDataLayout(string VersionStr, long SzIo, long SzStyle, long SzVec2, long SzVec4, long SzDrawvert, long SzDrawidx)
	{
		return ImGui_DebugCheckVersionAndDataLayout953(VersionStr, SzIo, SzStyle, SzVec2, SzVec4, SzDrawvert, SzDrawidx);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_SetAllocatorFunctions959(ImGuiMemAllocFunc AllocFunc, ImGuiMemFreeFunc FreeFunc, IntPtr UserData);

	/// <summary><code>IMGUI_API void          SetAllocatorFunctions(ImGuiMemAllocFunc alloc_func, ImGuiMemFreeFunc free_func, void* user_data = NULL);</code>
		///    IMGUI_API void          SetAllocatorFunctions(ImGuiMemAllocFunc alloc_func, ImGuiMemFreeFunc free_func, void* user_data = NULL); </summary>
	public static void SetAllocatorFunctions(ImGuiMemAllocFunc AllocFunc, ImGuiMemFreeFunc FreeFunc)
	{
		ImGui_SetAllocatorFunctions959(AllocFunc, FreeFunc, default);
	}

	/// <summary><code>IMGUI_API void          SetAllocatorFunctions(ImGuiMemAllocFunc alloc_func, ImGuiMemFreeFunc free_func, void* user_data = NULL);</code>
		///    IMGUI_API void          SetAllocatorFunctions(ImGuiMemAllocFunc alloc_func, ImGuiMemFreeFunc free_func, void* user_data = NULL); </summary>
	public static void SetAllocatorFunctions(ImGuiMemAllocFunc AllocFunc, ImGuiMemFreeFunc FreeFunc, IntPtr UserData)
	{
		ImGui_SetAllocatorFunctions959(AllocFunc, FreeFunc, UserData);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_GetAllocatorFunctions960(out ImGuiMemAllocFunc PAllocFunc, out ImGuiMemFreeFunc PFreeFunc, IntPtr PUserData);

	/// <summary><code>IMGUI_API void          GetAllocatorFunctions(ImGuiMemAllocFunc* p_alloc_func, ImGuiMemFreeFunc* p_free_func, void** p_user_data);</code>
		///    IMGUI_API void          GetAllocatorFunctions(ImGuiMemAllocFunc* p_alloc_func, ImGuiMemFreeFunc* p_free_func, void** p_user_data); </summary>
	public static void GetAllocatorFunctions(out ImGuiMemAllocFunc PAllocFunc, out ImGuiMemFreeFunc PFreeFunc, IntPtr PUserData)
	{
		ImGui_GetAllocatorFunctions960(out PAllocFunc, out PFreeFunc, PUserData);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern IntPtr ImGui_MemAlloc961(long Size);

	/// <summary><code>IMGUI_API void*         MemAlloc(size_t size);</code>
		///    IMGUI_API void*         MemAlloc(size_t size); </summary>
	public static IntPtr MemAlloc(long Size)
	{
		return ImGui_MemAlloc961(Size);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_MemFree962(IntPtr Ptr);

	/// <summary><code>IMGUI_API void          MemFree(void* ptr);</code>
		///    IMGUI_API void          MemFree(void* ptr); </summary>
	public static void MemFree(IntPtr Ptr)
	{
		ImGui_MemFree962(Ptr);
	}
	}
	public class ImGuiStyle
	{
		private IntPtr _objectPtr;
		public IntPtr AsPtr { get => _objectPtr; }
		public ImGuiStyle(IntPtr Ptr){ _objectPtr = Ptr; }

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGuiStyle_Get_Alpha1886(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStyle_Set_Alpha1886(IntPtr objectPtr, float  Value);

	/// <summary><code>float       Alpha;                      </code>
		/// Global alpha applies to everything in Dear ImGui. </summary>
	public float Alpha
	{
		get { return ImGuiStyle_Get_Alpha1886(_objectPtr);}
		set {ImGuiStyle_Set_Alpha1886(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGuiStyle_Get_DisabledAlpha1887(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStyle_Set_DisabledAlpha1887(IntPtr objectPtr, float  Value);

	/// <summary><code>float       DisabledAlpha;              </code>
		/// Additional alpha multiplier applied by BeginDisabled(). Multiply over current value of Alpha. </summary>
	public float DisabledAlpha
	{
		get { return ImGuiStyle_Get_DisabledAlpha1887(_objectPtr);}
		set {ImGuiStyle_Set_DisabledAlpha1887(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern Vector2 ImGuiStyle_Get_WindowPadding1888(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStyle_Set_WindowPadding1888(IntPtr objectPtr, Vector2  Value);

	/// <summary><code>ImVec2      WindowPadding;              </code>
		/// Padding within a window. </summary>
	public Vector2 WindowPadding
	{
		get { return ImGuiStyle_Get_WindowPadding1888(_objectPtr);}
		set {ImGuiStyle_Set_WindowPadding1888(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGuiStyle_Get_WindowRounding1889(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStyle_Set_WindowRounding1889(IntPtr objectPtr, float  Value);

	/// <summary><code>float       WindowRounding;             </code>
		/// Radius of window corners rounding. Set to 0.0f to have rectangular windows. Large values tend to lead to variety of artifacts and are not recommended. </summary>
	public float WindowRounding
	{
		get { return ImGuiStyle_Get_WindowRounding1889(_objectPtr);}
		set {ImGuiStyle_Set_WindowRounding1889(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGuiStyle_Get_WindowBorderSize1890(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStyle_Set_WindowBorderSize1890(IntPtr objectPtr, float  Value);

	/// <summary><code>float       WindowBorderSize;           </code>
		/// Thickness of border around windows. Generally set to 0.0f or 1.0f. (Other values are not well tested and more CPU/GPU costly). </summary>
	public float WindowBorderSize
	{
		get { return ImGuiStyle_Get_WindowBorderSize1890(_objectPtr);}
		set {ImGuiStyle_Set_WindowBorderSize1890(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern Vector2 ImGuiStyle_Get_WindowMinSize1891(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStyle_Set_WindowMinSize1891(IntPtr objectPtr, Vector2  Value);

	/// <summary><code>ImVec2      WindowMinSize;              </code>
		/// Minimum window size. This is a global setting. If you want to constrain individual windows, use SetNextWindowSizeConstraints(). </summary>
	public Vector2 WindowMinSize
	{
		get { return ImGuiStyle_Get_WindowMinSize1891(_objectPtr);}
		set {ImGuiStyle_Set_WindowMinSize1891(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern Vector2 ImGuiStyle_Get_WindowTitleAlign1892(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStyle_Set_WindowTitleAlign1892(IntPtr objectPtr, Vector2  Value);

	/// <summary><code>ImVec2      WindowTitleAlign;           </code>
		/// Alignment for title bar text. Defaults to (0.0f,0.5f) for left-aligned,vertically centered. </summary>
	public Vector2 WindowTitleAlign
	{
		get { return ImGuiStyle_Get_WindowTitleAlign1892(_objectPtr);}
		set {ImGuiStyle_Set_WindowTitleAlign1892(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern ImGuiDir ImGuiStyle_Get_WindowMenuButtonPosition1893(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStyle_Set_WindowMenuButtonPosition1893(IntPtr objectPtr, ImGuiDir  Value);

	/// <summary><code>ImGuiDir    WindowMenuButtonPosition;   </code>
		/// Side of the collapsing/docking button in the title bar (None/Left/Right). Defaults to ImGuiDir_Left. </summary>
	public ImGuiDir WindowMenuButtonPosition
	{
		get { return ImGuiStyle_Get_WindowMenuButtonPosition1893(_objectPtr);}
		set {ImGuiStyle_Set_WindowMenuButtonPosition1893(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGuiStyle_Get_ChildRounding1894(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStyle_Set_ChildRounding1894(IntPtr objectPtr, float  Value);

	/// <summary><code>float       ChildRounding;              </code>
		/// Radius of child window corners rounding. Set to 0.0f to have rectangular windows. </summary>
	public float ChildRounding
	{
		get { return ImGuiStyle_Get_ChildRounding1894(_objectPtr);}
		set {ImGuiStyle_Set_ChildRounding1894(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGuiStyle_Get_ChildBorderSize1895(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStyle_Set_ChildBorderSize1895(IntPtr objectPtr, float  Value);

	/// <summary><code>float       ChildBorderSize;            </code>
		/// Thickness of border around child windows. Generally set to 0.0f or 1.0f. (Other values are not well tested and more CPU/GPU costly). </summary>
	public float ChildBorderSize
	{
		get { return ImGuiStyle_Get_ChildBorderSize1895(_objectPtr);}
		set {ImGuiStyle_Set_ChildBorderSize1895(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGuiStyle_Get_PopupRounding1896(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStyle_Set_PopupRounding1896(IntPtr objectPtr, float  Value);

	/// <summary><code>float       PopupRounding;              </code>
		/// Radius of popup window corners rounding. (Note that tooltip windows use WindowRounding) </summary>
	public float PopupRounding
	{
		get { return ImGuiStyle_Get_PopupRounding1896(_objectPtr);}
		set {ImGuiStyle_Set_PopupRounding1896(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGuiStyle_Get_PopupBorderSize1897(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStyle_Set_PopupBorderSize1897(IntPtr objectPtr, float  Value);

	/// <summary><code>float       PopupBorderSize;            </code>
		/// Thickness of border around popup/tooltip windows. Generally set to 0.0f or 1.0f. (Other values are not well tested and more CPU/GPU costly). </summary>
	public float PopupBorderSize
	{
		get { return ImGuiStyle_Get_PopupBorderSize1897(_objectPtr);}
		set {ImGuiStyle_Set_PopupBorderSize1897(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern Vector2 ImGuiStyle_Get_FramePadding1898(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStyle_Set_FramePadding1898(IntPtr objectPtr, Vector2  Value);

	/// <summary><code>ImVec2      FramePadding;               </code>
		/// Padding within a framed rectangle (used by most widgets). </summary>
	public Vector2 FramePadding
	{
		get { return ImGuiStyle_Get_FramePadding1898(_objectPtr);}
		set {ImGuiStyle_Set_FramePadding1898(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGuiStyle_Get_FrameRounding1899(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStyle_Set_FrameRounding1899(IntPtr objectPtr, float  Value);

	/// <summary><code>float       FrameRounding;              </code>
		/// Radius of frame corners rounding. Set to 0.0f to have rectangular frame (used by most widgets). </summary>
	public float FrameRounding
	{
		get { return ImGuiStyle_Get_FrameRounding1899(_objectPtr);}
		set {ImGuiStyle_Set_FrameRounding1899(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGuiStyle_Get_FrameBorderSize1900(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStyle_Set_FrameBorderSize1900(IntPtr objectPtr, float  Value);

	/// <summary><code>float       FrameBorderSize;            </code>
		/// Thickness of border around frames. Generally set to 0.0f or 1.0f. (Other values are not well tested and more CPU/GPU costly). </summary>
	public float FrameBorderSize
	{
		get { return ImGuiStyle_Get_FrameBorderSize1900(_objectPtr);}
		set {ImGuiStyle_Set_FrameBorderSize1900(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern Vector2 ImGuiStyle_Get_ItemSpacing1901(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStyle_Set_ItemSpacing1901(IntPtr objectPtr, Vector2  Value);

	/// <summary><code>ImVec2      ItemSpacing;                </code>
		/// Horizontal and vertical spacing between widgets/lines. </summary>
	public Vector2 ItemSpacing
	{
		get { return ImGuiStyle_Get_ItemSpacing1901(_objectPtr);}
		set {ImGuiStyle_Set_ItemSpacing1901(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern Vector2 ImGuiStyle_Get_ItemInnerSpacing1902(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStyle_Set_ItemInnerSpacing1902(IntPtr objectPtr, Vector2  Value);

	/// <summary><code>ImVec2      ItemInnerSpacing;           </code>
		/// Horizontal and vertical spacing between within elements of a composed widget (e.g. a slider and its label). </summary>
	public Vector2 ItemInnerSpacing
	{
		get { return ImGuiStyle_Get_ItemInnerSpacing1902(_objectPtr);}
		set {ImGuiStyle_Set_ItemInnerSpacing1902(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern Vector2 ImGuiStyle_Get_CellPadding1903(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStyle_Set_CellPadding1903(IntPtr objectPtr, Vector2  Value);

	/// <summary><code>ImVec2      CellPadding;                </code>
		/// Padding within a table cell. CellPadding.y may be altered between different rows. </summary>
	public Vector2 CellPadding
	{
		get { return ImGuiStyle_Get_CellPadding1903(_objectPtr);}
		set {ImGuiStyle_Set_CellPadding1903(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern Vector2 ImGuiStyle_Get_TouchExtraPadding1904(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStyle_Set_TouchExtraPadding1904(IntPtr objectPtr, Vector2  Value);

	/// <summary><code>ImVec2      TouchExtraPadding;          </code>
		/// Expand reactive bounding box for touch-based system where touch position is not accurate enough. Unfortunately we don't sort widgets so priority on overlap will always be given to the first widget. So don't grow this too much! </summary>
	public Vector2 TouchExtraPadding
	{
		get { return ImGuiStyle_Get_TouchExtraPadding1904(_objectPtr);}
		set {ImGuiStyle_Set_TouchExtraPadding1904(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGuiStyle_Get_IndentSpacing1905(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStyle_Set_IndentSpacing1905(IntPtr objectPtr, float  Value);

	/// <summary><code>float       IndentSpacing;              </code>
		/// Horizontal indentation when e.g. entering a tree node. Generally == (FontSize + FramePadding.x*2). </summary>
	public float IndentSpacing
	{
		get { return ImGuiStyle_Get_IndentSpacing1905(_objectPtr);}
		set {ImGuiStyle_Set_IndentSpacing1905(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGuiStyle_Get_ColumnsMinSpacing1906(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStyle_Set_ColumnsMinSpacing1906(IntPtr objectPtr, float  Value);

	/// <summary><code>float       ColumnsMinSpacing;          </code>
		/// Minimum horizontal spacing between two columns. Preferably > (FramePadding.x + 1). </summary>
	public float ColumnsMinSpacing
	{
		get { return ImGuiStyle_Get_ColumnsMinSpacing1906(_objectPtr);}
		set {ImGuiStyle_Set_ColumnsMinSpacing1906(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGuiStyle_Get_ScrollbarSize1907(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStyle_Set_ScrollbarSize1907(IntPtr objectPtr, float  Value);

	/// <summary><code>float       ScrollbarSize;              </code>
		/// Width of the vertical scrollbar, Height of the horizontal scrollbar. </summary>
	public float ScrollbarSize
	{
		get { return ImGuiStyle_Get_ScrollbarSize1907(_objectPtr);}
		set {ImGuiStyle_Set_ScrollbarSize1907(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGuiStyle_Get_ScrollbarRounding1908(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStyle_Set_ScrollbarRounding1908(IntPtr objectPtr, float  Value);

	/// <summary><code>float       ScrollbarRounding;          </code>
		/// Radius of grab corners for scrollbar. </summary>
	public float ScrollbarRounding
	{
		get { return ImGuiStyle_Get_ScrollbarRounding1908(_objectPtr);}
		set {ImGuiStyle_Set_ScrollbarRounding1908(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGuiStyle_Get_GrabMinSize1909(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStyle_Set_GrabMinSize1909(IntPtr objectPtr, float  Value);

	/// <summary><code>float       GrabMinSize;                </code>
		/// Minimum width/height of a grab box for slider/scrollbar. </summary>
	public float GrabMinSize
	{
		get { return ImGuiStyle_Get_GrabMinSize1909(_objectPtr);}
		set {ImGuiStyle_Set_GrabMinSize1909(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGuiStyle_Get_GrabRounding1910(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStyle_Set_GrabRounding1910(IntPtr objectPtr, float  Value);

	/// <summary><code>float       GrabRounding;               </code>
		/// Radius of grabs corners rounding. Set to 0.0f to have rectangular slider grabs. </summary>
	public float GrabRounding
	{
		get { return ImGuiStyle_Get_GrabRounding1910(_objectPtr);}
		set {ImGuiStyle_Set_GrabRounding1910(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGuiStyle_Get_LogSliderDeadzone1911(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStyle_Set_LogSliderDeadzone1911(IntPtr objectPtr, float  Value);

	/// <summary><code>float       LogSliderDeadzone;          </code>
		/// The size in pixels of the dead-zone around zero on logarithmic sliders that cross zero. </summary>
	public float LogSliderDeadzone
	{
		get { return ImGuiStyle_Get_LogSliderDeadzone1911(_objectPtr);}
		set {ImGuiStyle_Set_LogSliderDeadzone1911(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGuiStyle_Get_TabRounding1912(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStyle_Set_TabRounding1912(IntPtr objectPtr, float  Value);

	/// <summary><code>float       TabRounding;                </code>
		/// Radius of upper corners of a tab. Set to 0.0f to have rectangular tabs. </summary>
	public float TabRounding
	{
		get { return ImGuiStyle_Get_TabRounding1912(_objectPtr);}
		set {ImGuiStyle_Set_TabRounding1912(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGuiStyle_Get_TabBorderSize1913(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStyle_Set_TabBorderSize1913(IntPtr objectPtr, float  Value);

	/// <summary><code>float       TabBorderSize;              </code>
		/// Thickness of border around tabs. </summary>
	public float TabBorderSize
	{
		get { return ImGuiStyle_Get_TabBorderSize1913(_objectPtr);}
		set {ImGuiStyle_Set_TabBorderSize1913(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGuiStyle_Get_TabMinWidthForCloseButton1914(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStyle_Set_TabMinWidthForCloseButton1914(IntPtr objectPtr, float  Value);

	/// <summary><code>float       TabMinWidthForCloseButton;  </code>
		/// Minimum width for close button to appear on an unselected tab when hovered. Set to 0.0f to always show when hovering, set to FLT_MAX to never show close button unless selected. </summary>
	public float TabMinWidthForCloseButton
	{
		get { return ImGuiStyle_Get_TabMinWidthForCloseButton1914(_objectPtr);}
		set {ImGuiStyle_Set_TabMinWidthForCloseButton1914(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern ImGuiDir ImGuiStyle_Get_ColorButtonPosition1915(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStyle_Set_ColorButtonPosition1915(IntPtr objectPtr, ImGuiDir  Value);

	/// <summary><code>ImGuiDir    ColorButtonPosition;        </code>
		/// Side of the color button in the ColorEdit4 widget (left/right). Defaults to ImGuiDir_Right. </summary>
	public ImGuiDir ColorButtonPosition
	{
		get { return ImGuiStyle_Get_ColorButtonPosition1915(_objectPtr);}
		set {ImGuiStyle_Set_ColorButtonPosition1915(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern Vector2 ImGuiStyle_Get_ButtonTextAlign1916(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStyle_Set_ButtonTextAlign1916(IntPtr objectPtr, Vector2  Value);

	/// <summary><code>ImVec2      ButtonTextAlign;            </code>
		/// Alignment of button text when button is larger than text. Defaults to (0.5f, 0.5f) (centered). </summary>
	public Vector2 ButtonTextAlign
	{
		get { return ImGuiStyle_Get_ButtonTextAlign1916(_objectPtr);}
		set {ImGuiStyle_Set_ButtonTextAlign1916(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern Vector2 ImGuiStyle_Get_SelectableTextAlign1917(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStyle_Set_SelectableTextAlign1917(IntPtr objectPtr, Vector2  Value);

	/// <summary><code>ImVec2      SelectableTextAlign;        </code>
		/// Alignment of selectable text. Defaults to (0.0f, 0.0f) (top-left aligned). It's generally important to keep this left-aligned if you want to lay multiple items on a same line. </summary>
	public Vector2 SelectableTextAlign
	{
		get { return ImGuiStyle_Get_SelectableTextAlign1917(_objectPtr);}
		set {ImGuiStyle_Set_SelectableTextAlign1917(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGuiStyle_Get_SeparatorTextBorderSize1918(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStyle_Set_SeparatorTextBorderSize1918(IntPtr objectPtr, float  Value);

	/// <summary><code>float       SeparatorTextBorderSize;    </code>
		/// Thickkness of border in SeparatorText() </summary>
	public float SeparatorTextBorderSize
	{
		get { return ImGuiStyle_Get_SeparatorTextBorderSize1918(_objectPtr);}
		set {ImGuiStyle_Set_SeparatorTextBorderSize1918(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern Vector2 ImGuiStyle_Get_SeparatorTextAlign1919(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStyle_Set_SeparatorTextAlign1919(IntPtr objectPtr, Vector2  Value);

	/// <summary><code>ImVec2      SeparatorTextAlign;         </code>
		/// Alignment of text within the separator. Defaults to (0.0f, 0.5f) (left aligned, center). </summary>
	public Vector2 SeparatorTextAlign
	{
		get { return ImGuiStyle_Get_SeparatorTextAlign1919(_objectPtr);}
		set {ImGuiStyle_Set_SeparatorTextAlign1919(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern Vector2 ImGuiStyle_Get_SeparatorTextPadding1920(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStyle_Set_SeparatorTextPadding1920(IntPtr objectPtr, Vector2  Value);

	/// <summary><code>ImVec2      SeparatorTextPadding;       </code>
		/// Horizontal offset of text from each edge of the separator + spacing on other axis. Generally small values. .y is recommended to be == FramePadding.y. </summary>
	public Vector2 SeparatorTextPadding
	{
		get { return ImGuiStyle_Get_SeparatorTextPadding1920(_objectPtr);}
		set {ImGuiStyle_Set_SeparatorTextPadding1920(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern Vector2 ImGuiStyle_Get_DisplayWindowPadding1921(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStyle_Set_DisplayWindowPadding1921(IntPtr objectPtr, Vector2  Value);

	/// <summary><code>ImVec2      DisplayWindowPadding;       </code>
		/// Window position are clamped to be visible within the display area or monitors by at least this amount. Only applies to regular windows. </summary>
	public Vector2 DisplayWindowPadding
	{
		get { return ImGuiStyle_Get_DisplayWindowPadding1921(_objectPtr);}
		set {ImGuiStyle_Set_DisplayWindowPadding1921(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern Vector2 ImGuiStyle_Get_DisplaySafeAreaPadding1922(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStyle_Set_DisplaySafeAreaPadding1922(IntPtr objectPtr, Vector2  Value);

	/// <summary><code>ImVec2      DisplaySafeAreaPadding;     </code>
		/// If you cannot see the edges of your screen (e.g. on a TV) increase the safe area padding. Apply to popups/tooltips as well regular windows. NB: Prefer configuring your TV sets correctly! </summary>
	public Vector2 DisplaySafeAreaPadding
	{
		get { return ImGuiStyle_Get_DisplaySafeAreaPadding1922(_objectPtr);}
		set {ImGuiStyle_Set_DisplaySafeAreaPadding1922(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGuiStyle_Get_MouseCursorScale1923(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStyle_Set_MouseCursorScale1923(IntPtr objectPtr, float  Value);

	/// <summary><code>float       MouseCursorScale;           </code>
		/// Scale software rendered mouse cursor (when io.MouseDrawCursor is enabled). May be removed later. </summary>
	public float MouseCursorScale
	{
		get { return ImGuiStyle_Get_MouseCursorScale1923(_objectPtr);}
		set {ImGuiStyle_Set_MouseCursorScale1923(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGuiStyle_Get_AntiAliasedLines1924(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStyle_Set_AntiAliasedLines1924(IntPtr objectPtr, [MarshalAs(UnmanagedType.I1)]bool  Value);

	/// <summary><code>bool        AntiAliasedLines;           </code>
		/// Enable anti-aliased lines/borders. Disable if you are really tight on CPU/GPU. Latched at the beginning of the frame (copied to ImDrawList). </summary>
	public bool AntiAliasedLines
	{
		get { return ImGuiStyle_Get_AntiAliasedLines1924(_objectPtr);}
		set {ImGuiStyle_Set_AntiAliasedLines1924(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGuiStyle_Get_AntiAliasedLinesUseTex1925(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStyle_Set_AntiAliasedLinesUseTex1925(IntPtr objectPtr, [MarshalAs(UnmanagedType.I1)]bool  Value);

	/// <summary><code>bool        AntiAliasedLinesUseTex;     </code>
		/// Enable anti-aliased lines/borders using textures where possible. Require backend to render with bilinear filtering (NOT point/nearest filtering). Latched at the beginning of the frame (copied to ImDrawList). </summary>
	public bool AntiAliasedLinesUseTex
	{
		get { return ImGuiStyle_Get_AntiAliasedLinesUseTex1925(_objectPtr);}
		set {ImGuiStyle_Set_AntiAliasedLinesUseTex1925(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGuiStyle_Get_AntiAliasedFill1926(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStyle_Set_AntiAliasedFill1926(IntPtr objectPtr, [MarshalAs(UnmanagedType.I1)]bool  Value);

	/// <summary><code>bool        AntiAliasedFill;            </code>
		/// Enable anti-aliased edges around filled shapes (rounded rectangles, circles, etc.). Disable if you are really tight on CPU/GPU. Latched at the beginning of the frame (copied to ImDrawList). </summary>
	public bool AntiAliasedFill
	{
		get { return ImGuiStyle_Get_AntiAliasedFill1926(_objectPtr);}
		set {ImGuiStyle_Set_AntiAliasedFill1926(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGuiStyle_Get_CurveTessellationTol1927(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStyle_Set_CurveTessellationTol1927(IntPtr objectPtr, float  Value);

	/// <summary><code>float       CurveTessellationTol;       </code>
		/// Tessellation tolerance when using PathBezierCurveTo() without a specific number of segments. Decrease for highly tessellated curves (higher quality, more polygons), increase to reduce quality. </summary>
	public float CurveTessellationTol
	{
		get { return ImGuiStyle_Get_CurveTessellationTol1927(_objectPtr);}
		set {ImGuiStyle_Set_CurveTessellationTol1927(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGuiStyle_Get_CircleTessellationMaxError1928(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStyle_Set_CircleTessellationMaxError1928(IntPtr objectPtr, float  Value);

	/// <summary><code>float       CircleTessellationMaxError; </code>
		/// Maximum error (in pixels) allowed when using AddCircle()/AddCircleFilled() or drawing rounded corner rectangles with no explicit segment count specified. Decrease for higher quality but more geometry. </summary>
	public float CircleTessellationMaxError
	{
		get { return ImGuiStyle_Get_CircleTessellationMaxError1928(_objectPtr);}
		set {ImGuiStyle_Set_CircleTessellationMaxError1928(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.LPArray, SizeConst = (int)ImGuiCol.COUNT)]
	private static extern Vector4[] ImGuiStyle_Get_Colors1929(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStyle_Set_Colors1929(IntPtr objectPtr, [MarshalAs(UnmanagedType.LPArray, SizeConst = (int)ImGuiCol.COUNT)]Vector4[]  Value);

	/// <summary><code>ImVec4      Colors[ImGuiCol_COUNT];</code>
		///    ImVec4      Colors[ImGuiCol_COUNT]; </summary>
	public Vector4[] Colors
	{
		get { return ImGuiStyle_Get_Colors1929(_objectPtr);}
		set {ImGuiStyle_Set_Colors1929(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGuiStyle_Get_HoverStationaryDelay1933(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStyle_Set_HoverStationaryDelay1933(IntPtr objectPtr, float  Value);

	/// <summary><code>float             HoverStationaryDelay;     </code>
		/// Delay for IsItemHovered(ImGuiHoveredFlags_Stationary). Time required to consider mouse stationary. </summary>
	public float HoverStationaryDelay
	{
		get { return ImGuiStyle_Get_HoverStationaryDelay1933(_objectPtr);}
		set {ImGuiStyle_Set_HoverStationaryDelay1933(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGuiStyle_Get_HoverDelayShort1934(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStyle_Set_HoverDelayShort1934(IntPtr objectPtr, float  Value);

	/// <summary><code>float             HoverDelayShort;          </code>
		/// Delay for IsItemHovered(ImGuiHoveredFlags_DelayShort). Usually used along with HoverStationaryDelay. </summary>
	public float HoverDelayShort
	{
		get { return ImGuiStyle_Get_HoverDelayShort1934(_objectPtr);}
		set {ImGuiStyle_Set_HoverDelayShort1934(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGuiStyle_Get_HoverDelayNormal1935(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStyle_Set_HoverDelayNormal1935(IntPtr objectPtr, float  Value);

	/// <summary><code>float             HoverDelayNormal;         </code>
		/// Delay for IsItemHovered(ImGuiHoveredFlags_DelayNormal). " </summary>
	public float HoverDelayNormal
	{
		get { return ImGuiStyle_Get_HoverDelayNormal1935(_objectPtr);}
		set {ImGuiStyle_Set_HoverDelayNormal1935(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern ImGuiHoveredFlags ImGuiStyle_Get_HoverFlagsForTooltipMouse1936(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStyle_Set_HoverFlagsForTooltipMouse1936(IntPtr objectPtr, ImGuiHoveredFlags  Value);

	/// <summary><code>ImGuiHoveredFlags HoverFlagsForTooltipMouse;</code>
		/// Default flags when using IsItemHovered(ImGuiHoveredFlags_ForTooltip) or BeginItemTooltip()/SetItemTooltip() while using mouse. </summary>
	public ImGuiHoveredFlags HoverFlagsForTooltipMouse
	{
		get { return ImGuiStyle_Get_HoverFlagsForTooltipMouse1936(_objectPtr);}
		set {ImGuiStyle_Set_HoverFlagsForTooltipMouse1936(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern ImGuiHoveredFlags ImGuiStyle_Get_HoverFlagsForTooltipNav1937(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStyle_Set_HoverFlagsForTooltipNav1937(IntPtr objectPtr, ImGuiHoveredFlags  Value);

	/// <summary><code>ImGuiHoveredFlags HoverFlagsForTooltipNav;  </code>
		/// Default flags when using IsItemHovered(ImGuiHoveredFlags_ForTooltip) or BeginItemTooltip()/SetItemTooltip() while using keyboard/gamepad. </summary>
	public ImGuiHoveredFlags HoverFlagsForTooltipNav
	{
		get { return ImGuiStyle_Get_HoverFlagsForTooltipNav1937(_objectPtr);}
		set {ImGuiStyle_Set_HoverFlagsForTooltipNav1937(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern ImGuiStyle ImGuiStyle_ImGuiStyle1939();

	/// <summary><code>IMGUI_API ImGuiStyle();</code>
		///    IMGUI_API ImGuiStyle(); </summary>
	public  ImGuiStyle()
	{
		_objectPtr = ImGuiStyle_ImGuiStyle1939()._objectPtr;
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStyle_ScaleAllSizes1940(IntPtr objectPtr, float ScaleFactor);

	/// <summary><code>IMGUI_API void ScaleAllSizes(float scale_factor);</code>
		///    IMGUI_API void ScaleAllSizes(float scale_factor); </summary>
	public void ScaleAllSizes(float ScaleFactor)
	{
		ImGuiStyle_ScaleAllSizes1940(_objectPtr, ScaleFactor);
	}
	}
	public class ImGuiKeyData
	{
		private IntPtr _objectPtr;
		public IntPtr AsPtr { get => _objectPtr; }
		public ImGuiKeyData(IntPtr Ptr){ _objectPtr = Ptr; }

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGuiKeyData_Get_Down1954(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiKeyData_Set_Down1954(IntPtr objectPtr, [MarshalAs(UnmanagedType.I1)]bool  Value);

	/// <summary><code>bool        Down;               </code>
		/// True for if key is down </summary>
	public bool Down
	{
		get { return ImGuiKeyData_Get_Down1954(_objectPtr);}
		set {ImGuiKeyData_Set_Down1954(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGuiKeyData_Get_DownDuration1955(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiKeyData_Set_DownDuration1955(IntPtr objectPtr, float  Value);

	/// <summary><code>float       DownDuration;       </code>
		/// Duration the key has been down (<0.0f: not pressed, 0.0f: just pressed, >0.0f: time held) </summary>
	public float DownDuration
	{
		get { return ImGuiKeyData_Get_DownDuration1955(_objectPtr);}
		set {ImGuiKeyData_Set_DownDuration1955(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGuiKeyData_Get_DownDurationPrev1956(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiKeyData_Set_DownDurationPrev1956(IntPtr objectPtr, float  Value);

	/// <summary><code>float       DownDurationPrev;   </code>
		/// Last frame duration the key has been down </summary>
	public float DownDurationPrev
	{
		get { return ImGuiKeyData_Get_DownDurationPrev1956(_objectPtr);}
		set {ImGuiKeyData_Set_DownDurationPrev1956(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGuiKeyData_Get_AnalogValue1957(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiKeyData_Set_AnalogValue1957(IntPtr objectPtr, float  Value);

	/// <summary><code>float       AnalogValue;        </code>
		/// 0.0f..1.0f for gamepad values </summary>
	public float AnalogValue
	{
		get { return ImGuiKeyData_Get_AnalogValue1957(_objectPtr);}
		set {ImGuiKeyData_Set_AnalogValue1957(_objectPtr, value);}
	}	}
	public class ImGuiIO
	{
		private IntPtr _objectPtr;
		public IntPtr AsPtr { get => _objectPtr; }
		public ImGuiIO(IntPtr Ptr){ _objectPtr = Ptr; }

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern ImGuiConfigFlags ImGuiIO_Get_ConfigFlags1966(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_ConfigFlags1966(IntPtr objectPtr, ImGuiConfigFlags  Value);

	/// <summary><code>ImGuiConfigFlags   ConfigFlags;             </code>
		/// = 0              // See ImGuiConfigFlags_ enum. Set by user/application. Gamepad/keyboard navigation options, etc. </summary>
	public ImGuiConfigFlags ConfigFlags
	{
		get { return ImGuiIO_Get_ConfigFlags1966(_objectPtr);}
		set {ImGuiIO_Set_ConfigFlags1966(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern ImGuiBackendFlags ImGuiIO_Get_BackendFlags1967(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_BackendFlags1967(IntPtr objectPtr, ImGuiBackendFlags  Value);

	/// <summary><code>ImGuiBackendFlags  BackendFlags;            </code>
		/// = 0              // See ImGuiBackendFlags_ enum. Set by backend (imgui_impl_xxx files or custom backend) to communicate features supported by the backend. </summary>
	public ImGuiBackendFlags BackendFlags
	{
		get { return ImGuiIO_Get_BackendFlags1967(_objectPtr);}
		set {ImGuiIO_Set_BackendFlags1967(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern Vector2 ImGuiIO_Get_DisplaySize1968(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_DisplaySize1968(IntPtr objectPtr, Vector2  Value);

	/// <summary><code>ImVec2      DisplaySize;                    </code>
		/// <unset>          // Main display size, in pixels (generally == GetMainViewport()->Size). May change every frame. </summary>
	public Vector2 DisplaySize
	{
		get { return ImGuiIO_Get_DisplaySize1968(_objectPtr);}
		set {ImGuiIO_Set_DisplaySize1968(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGuiIO_Get_DeltaTime1969(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_DeltaTime1969(IntPtr objectPtr, float  Value);

	/// <summary><code>float       DeltaTime;                      </code>
		/// = 1.0f/60.0f     // Time elapsed since last frame, in seconds. May change every frame. </summary>
	public float DeltaTime
	{
		get { return ImGuiIO_Get_DeltaTime1969(_objectPtr);}
		set {ImGuiIO_Set_DeltaTime1969(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGuiIO_Get_IniSavingRate1970(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_IniSavingRate1970(IntPtr objectPtr, float  Value);

	/// <summary><code>float       IniSavingRate;                  </code>
		/// = 5.0f           // Minimum time between saving positions/sizes to .ini file, in seconds. </summary>
	public float IniSavingRate
	{
		get { return ImGuiIO_Get_IniSavingRate1970(_objectPtr);}
		set {ImGuiIO_Set_IniSavingRate1970(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.LPStr)]
	private static extern string ImGuiIO_Get_IniFilename1971(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_IniFilename1971(IntPtr objectPtr, [MarshalAs(UnmanagedType.LPStr)]string  Value);

	/// <summary><code>const char* IniFilename;                    </code>
		/// = "imgui.ini"    // Path to .ini file (important: default "imgui.ini" is relative to current working dir!). Set NULL to disable automatic .ini loading/saving or if you want to manually call LoadIniSettingsXXX() / SaveIniSettingsXXX() functions. </summary>
	public string IniFilename
	{
		get { return ImGuiIO_Get_IniFilename1971(_objectPtr);}
		set {ImGuiIO_Set_IniFilename1971(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.LPStr)]
	private static extern string ImGuiIO_Get_LogFilename1972(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_LogFilename1972(IntPtr objectPtr, [MarshalAs(UnmanagedType.LPStr)]string  Value);

	/// <summary><code>const char* LogFilename;                    </code>
		/// = "imgui_log.txt"// Path to .log file (default parameter to ImGui::LogToFile when no file is specified). </summary>
	public string LogFilename
	{
		get { return ImGuiIO_Get_LogFilename1972(_objectPtr);}
		set {ImGuiIO_Set_LogFilename1972(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern IntPtr ImGuiIO_Get_UserData1973(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_UserData1973(IntPtr objectPtr, IntPtr  Value);

	/// <summary><code>void*       UserData;                       </code>
		/// = NULL           // Store your own data. </summary>
	public IntPtr UserData
	{
		get { return ImGuiIO_Get_UserData1973(_objectPtr);}
		set {ImGuiIO_Set_UserData1973(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern Vector2 ImGuiIO_Get_DisplayFramebufferScale1979(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_DisplayFramebufferScale1979(IntPtr objectPtr, Vector2  Value);

	/// <summary><code>ImVec2      DisplayFramebufferScale;        </code>
		/// = (1, 1)         // For retina display or other situations where window coordinates are different from framebuffer coordinates. This generally ends up in ImDrawData::FramebufferScale. </summary>
	public Vector2 DisplayFramebufferScale
	{
		get { return ImGuiIO_Get_DisplayFramebufferScale1979(_objectPtr);}
		set {ImGuiIO_Set_DisplayFramebufferScale1979(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGuiIO_Get_MouseDrawCursor1982(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_MouseDrawCursor1982(IntPtr objectPtr, [MarshalAs(UnmanagedType.I1)]bool  Value);

	/// <summary><code>bool        MouseDrawCursor;                </code>
		/// = false          // Request ImGui to draw a mouse cursor for you (if you are on a platform without a mouse cursor). Cannot be easily renamed to 'io.ConfigXXX' because this is frequently used by backend implementations. </summary>
	public bool MouseDrawCursor
	{
		get { return ImGuiIO_Get_MouseDrawCursor1982(_objectPtr);}
		set {ImGuiIO_Set_MouseDrawCursor1982(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGuiIO_Get_ConfigMacOSXBehaviors1983(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_ConfigMacOSXBehaviors1983(IntPtr objectPtr, [MarshalAs(UnmanagedType.I1)]bool  Value);

	/// <summary><code>bool        ConfigMacOSXBehaviors;          </code>
		/// = defined(__APPLE__) // OS X style: Text editing cursor movement using Alt instead of Ctrl, Shortcuts using Cmd/Super instead of Ctrl, Line/Text Start and End using Cmd+Arrows instead of Home/End, Double click selects by word instead of selecting whole text, Multi-selection in lists uses Cmd/Super instead of Ctrl. </summary>
	public bool ConfigMacOSXBehaviors
	{
		get { return ImGuiIO_Get_ConfigMacOSXBehaviors1983(_objectPtr);}
		set {ImGuiIO_Set_ConfigMacOSXBehaviors1983(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGuiIO_Get_ConfigInputTrickleEventQueue1984(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_ConfigInputTrickleEventQueue1984(IntPtr objectPtr, [MarshalAs(UnmanagedType.I1)]bool  Value);

	/// <summary><code>bool        ConfigInputTrickleEventQueue;   </code>
		/// = true           // Enable input queue trickling: some types of events submitted during the same frame (e.g. button down + up) will be spread over multiple frames, improving interactions with low framerates. </summary>
	public bool ConfigInputTrickleEventQueue
	{
		get { return ImGuiIO_Get_ConfigInputTrickleEventQueue1984(_objectPtr);}
		set {ImGuiIO_Set_ConfigInputTrickleEventQueue1984(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGuiIO_Get_ConfigInputTextCursorBlink1985(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_ConfigInputTextCursorBlink1985(IntPtr objectPtr, [MarshalAs(UnmanagedType.I1)]bool  Value);

	/// <summary><code>bool        ConfigInputTextCursorBlink;     </code>
		/// = true           // Enable blinking cursor (optional as some users consider it to be distracting). </summary>
	public bool ConfigInputTextCursorBlink
	{
		get { return ImGuiIO_Get_ConfigInputTextCursorBlink1985(_objectPtr);}
		set {ImGuiIO_Set_ConfigInputTextCursorBlink1985(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGuiIO_Get_ConfigInputTextEnterKeepActive1986(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_ConfigInputTextEnterKeepActive1986(IntPtr objectPtr, [MarshalAs(UnmanagedType.I1)]bool  Value);

	/// <summary><code>bool        ConfigInputTextEnterKeepActive; </code>
		/// = false          // [BETA] Pressing Enter will keep item active and select contents (single-line only). </summary>
	public bool ConfigInputTextEnterKeepActive
	{
		get { return ImGuiIO_Get_ConfigInputTextEnterKeepActive1986(_objectPtr);}
		set {ImGuiIO_Set_ConfigInputTextEnterKeepActive1986(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGuiIO_Get_ConfigDragClickToInputText1987(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_ConfigDragClickToInputText1987(IntPtr objectPtr, [MarshalAs(UnmanagedType.I1)]bool  Value);

	/// <summary><code>bool        ConfigDragClickToInputText;     </code>
		/// = false          // [BETA] Enable turning DragXXX widgets into text input with a simple mouse click-release (without moving). Not desirable on devices without a keyboard. </summary>
	public bool ConfigDragClickToInputText
	{
		get { return ImGuiIO_Get_ConfigDragClickToInputText1987(_objectPtr);}
		set {ImGuiIO_Set_ConfigDragClickToInputText1987(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGuiIO_Get_ConfigWindowsResizeFromEdges1988(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_ConfigWindowsResizeFromEdges1988(IntPtr objectPtr, [MarshalAs(UnmanagedType.I1)]bool  Value);

	/// <summary><code>bool        ConfigWindowsResizeFromEdges;   </code>
		/// = true           // Enable resizing of windows from their edges and from the lower-left corner. This requires (io.BackendFlags & ImGuiBackendFlags_HasMouseCursors) because it needs mouse cursor feedback. (This used to be a per-window ImGuiWindowFlags_ResizeFromAnySide flag) </summary>
	public bool ConfigWindowsResizeFromEdges
	{
		get { return ImGuiIO_Get_ConfigWindowsResizeFromEdges1988(_objectPtr);}
		set {ImGuiIO_Set_ConfigWindowsResizeFromEdges1988(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGuiIO_Get_ConfigWindowsMoveFromTitleBarOnly1989(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_ConfigWindowsMoveFromTitleBarOnly1989(IntPtr objectPtr, [MarshalAs(UnmanagedType.I1)]bool  Value);

	/// <summary><code>bool        ConfigWindowsMoveFromTitleBarOnly; </code>
		/// = false       // Enable allowing to move windows only when clicking on their title bar. Does not apply to windows without a title bar. </summary>
	public bool ConfigWindowsMoveFromTitleBarOnly
	{
		get { return ImGuiIO_Get_ConfigWindowsMoveFromTitleBarOnly1989(_objectPtr);}
		set {ImGuiIO_Set_ConfigWindowsMoveFromTitleBarOnly1989(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGuiIO_Get_ConfigMemoryCompactTimer1990(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_ConfigMemoryCompactTimer1990(IntPtr objectPtr, float  Value);

	/// <summary><code>float       ConfigMemoryCompactTimer;       </code>
		/// = 60.0f          // Timer (in seconds) to free transient windows/tables memory buffers when unused. Set to -1.0f to disable. </summary>
	public float ConfigMemoryCompactTimer
	{
		get { return ImGuiIO_Get_ConfigMemoryCompactTimer1990(_objectPtr);}
		set {ImGuiIO_Set_ConfigMemoryCompactTimer1990(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGuiIO_Get_MouseDoubleClickTime1994(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_MouseDoubleClickTime1994(IntPtr objectPtr, float  Value);

	/// <summary><code>float       MouseDoubleClickTime;           </code>
		/// = 0.30f          // Time for a double-click, in seconds. </summary>
	public float MouseDoubleClickTime
	{
		get { return ImGuiIO_Get_MouseDoubleClickTime1994(_objectPtr);}
		set {ImGuiIO_Set_MouseDoubleClickTime1994(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGuiIO_Get_MouseDoubleClickMaxDist1995(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_MouseDoubleClickMaxDist1995(IntPtr objectPtr, float  Value);

	/// <summary><code>float       MouseDoubleClickMaxDist;        </code>
		/// = 6.0f           // Distance threshold to stay in to validate a double-click, in pixels. </summary>
	public float MouseDoubleClickMaxDist
	{
		get { return ImGuiIO_Get_MouseDoubleClickMaxDist1995(_objectPtr);}
		set {ImGuiIO_Set_MouseDoubleClickMaxDist1995(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGuiIO_Get_MouseDragThreshold1996(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_MouseDragThreshold1996(IntPtr objectPtr, float  Value);

	/// <summary><code>float       MouseDragThreshold;             </code>
		/// = 6.0f           // Distance threshold before considering we are dragging. </summary>
	public float MouseDragThreshold
	{
		get { return ImGuiIO_Get_MouseDragThreshold1996(_objectPtr);}
		set {ImGuiIO_Set_MouseDragThreshold1996(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGuiIO_Get_KeyRepeatDelay1997(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_KeyRepeatDelay1997(IntPtr objectPtr, float  Value);

	/// <summary><code>float       KeyRepeatDelay;                 </code>
		/// = 0.275f         // When holding a key/button, time before it starts repeating, in seconds (for buttons in Repeat mode, etc.). </summary>
	public float KeyRepeatDelay
	{
		get { return ImGuiIO_Get_KeyRepeatDelay1997(_objectPtr);}
		set {ImGuiIO_Set_KeyRepeatDelay1997(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGuiIO_Get_KeyRepeatRate1998(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_KeyRepeatRate1998(IntPtr objectPtr, float  Value);

	/// <summary><code>float       KeyRepeatRate;                  </code>
		/// = 0.050f         // When holding a key/button, rate at which it repeats, in seconds. </summary>
	public float KeyRepeatRate
	{
		get { return ImGuiIO_Get_KeyRepeatRate1998(_objectPtr);}
		set {ImGuiIO_Set_KeyRepeatRate1998(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGuiIO_Get_ConfigDebugBeginReturnValueOnce2008(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_ConfigDebugBeginReturnValueOnce2008(IntPtr objectPtr, [MarshalAs(UnmanagedType.I1)]bool  Value);

	/// <summary><code>bool        ConfigDebugBeginReturnValueOnce;</code>
		/// = false          // First-time calls to Begin()/BeginChild() will return false. NEEDS TO BE SET AT APPLICATION BOOT TIME if you don't want to miss windows. </summary>
	public bool ConfigDebugBeginReturnValueOnce
	{
		get { return ImGuiIO_Get_ConfigDebugBeginReturnValueOnce2008(_objectPtr);}
		set {ImGuiIO_Set_ConfigDebugBeginReturnValueOnce2008(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGuiIO_Get_ConfigDebugBeginReturnValueLoop2009(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_ConfigDebugBeginReturnValueLoop2009(IntPtr objectPtr, [MarshalAs(UnmanagedType.I1)]bool  Value);

	/// <summary><code>bool        ConfigDebugBeginReturnValueLoop;</code>
		/// = false          // Some calls to Begin()/BeginChild() will return false. Will cycle through window depths then repeat. Suggested use: add "io.ConfigDebugBeginReturnValue = io.KeyShift" in your main loop then occasionally press SHIFT. Windows should be flickering while running. </summary>
	public bool ConfigDebugBeginReturnValueLoop
	{
		get { return ImGuiIO_Get_ConfigDebugBeginReturnValueLoop2009(_objectPtr);}
		set {ImGuiIO_Set_ConfigDebugBeginReturnValueLoop2009(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGuiIO_Get_ConfigDebugIgnoreFocusLoss2014(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_ConfigDebugIgnoreFocusLoss2014(IntPtr objectPtr, [MarshalAs(UnmanagedType.I1)]bool  Value);

	/// <summary><code>bool        ConfigDebugIgnoreFocusLoss;     </code>
		/// = false          // Ignore io.AddFocusEvent(false), consequently not calling io.ClearInputKeys() in input processing. </summary>
	public bool ConfigDebugIgnoreFocusLoss
	{
		get { return ImGuiIO_Get_ConfigDebugIgnoreFocusLoss2014(_objectPtr);}
		set {ImGuiIO_Set_ConfigDebugIgnoreFocusLoss2014(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGuiIO_Get_ConfigDebugIniSettings2017(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_ConfigDebugIniSettings2017(IntPtr objectPtr, [MarshalAs(UnmanagedType.I1)]bool  Value);

	/// <summary><code>bool        ConfigDebugIniSettings;         </code>
		/// = false          // Save .ini data with extra comments (particularly helpful for Docking, but makes saving slower) </summary>
	public bool ConfigDebugIniSettings
	{
		get { return ImGuiIO_Get_ConfigDebugIniSettings2017(_objectPtr);}
		set {ImGuiIO_Set_ConfigDebugIniSettings2017(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.LPStr)]
	private static extern string ImGuiIO_Get_BackendPlatformName2025(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_BackendPlatformName2025(IntPtr objectPtr, [MarshalAs(UnmanagedType.LPStr)]string  Value);

	/// <summary><code>const char* BackendPlatformName;            </code>
		/// = NULL </summary>
	public string BackendPlatformName
	{
		get { return ImGuiIO_Get_BackendPlatformName2025(_objectPtr);}
		set {ImGuiIO_Set_BackendPlatformName2025(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.LPStr)]
	private static extern string ImGuiIO_Get_BackendRendererName2026(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_BackendRendererName2026(IntPtr objectPtr, [MarshalAs(UnmanagedType.LPStr)]string  Value);

	/// <summary><code>const char* BackendRendererName;            </code>
		/// = NULL </summary>
	public string BackendRendererName
	{
		get { return ImGuiIO_Get_BackendRendererName2026(_objectPtr);}
		set {ImGuiIO_Set_BackendRendererName2026(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern IntPtr ImGuiIO_Get_BackendPlatformUserData2027(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_BackendPlatformUserData2027(IntPtr objectPtr, IntPtr  Value);

	/// <summary><code>void*       BackendPlatformUserData;        </code>
		/// = NULL           // User data for platform backend </summary>
	public IntPtr BackendPlatformUserData
	{
		get { return ImGuiIO_Get_BackendPlatformUserData2027(_objectPtr);}
		set {ImGuiIO_Set_BackendPlatformUserData2027(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern IntPtr ImGuiIO_Get_BackendRendererUserData2028(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_BackendRendererUserData2028(IntPtr objectPtr, IntPtr  Value);

	/// <summary><code>void*       BackendRendererUserData;        </code>
		/// = NULL           // User data for renderer backend </summary>
	public IntPtr BackendRendererUserData
	{
		get { return ImGuiIO_Get_BackendRendererUserData2028(_objectPtr);}
		set {ImGuiIO_Set_BackendRendererUserData2028(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern IntPtr ImGuiIO_Get_BackendLanguageUserData2029(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_BackendLanguageUserData2029(IntPtr objectPtr, IntPtr  Value);

	/// <summary><code>void*       BackendLanguageUserData;        </code>
		/// = NULL           // User data for non C++ programming language backend </summary>
	public IntPtr BackendLanguageUserData
	{
		get { return ImGuiIO_Get_BackendLanguageUserData2029(_objectPtr);}
		set {ImGuiIO_Set_BackendLanguageUserData2029(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern IntPtr ImGuiIO_Get_ClipboardUserData2035(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_ClipboardUserData2035(IntPtr objectPtr, IntPtr  Value);

	/// <summary><code>void*       ClipboardUserData;</code>
		///    void*       ClipboardUserData; </summary>
	public IntPtr ClipboardUserData
	{
		get { return ImGuiIO_Get_ClipboardUserData2035(_objectPtr);}
		set {ImGuiIO_Set_ClipboardUserData2035(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern IntPtr ImGuiIO_Get_ImeWindowHandle2041(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_ImeWindowHandle2041(IntPtr objectPtr, IntPtr  Value);

	/// <summary><code>void*       ImeWindowHandle;                </code>
		/// = NULL           // [Obsolete] Set ImGuiViewport::PlatformHandleRaw instead. Set this to your HWND to get automatic IME cursor positioning. </summary>
	public IntPtr ImeWindowHandle
	{
		get { return ImGuiIO_Get_ImeWindowHandle2041(_objectPtr);}
		set {ImGuiIO_Set_ImeWindowHandle2041(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern char ImGuiIO_Get_PlatformLocaleDecimalPoint2047(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_PlatformLocaleDecimalPoint2047(IntPtr objectPtr, char  Value);

	/// <summary><code>ImWchar     PlatformLocaleDecimalPoint;     </code>
		/// '.'              // [Experimental] Configure decimal point e.g. '.' or ',' useful for some languages (e.g. German), generally pulled from *localeconv()->decimal_point </summary>
	public char PlatformLocaleDecimalPoint
	{
		get { return ImGuiIO_Get_PlatformLocaleDecimalPoint2047(_objectPtr);}
		set {ImGuiIO_Set_PlatformLocaleDecimalPoint2047(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_AddKeyEvent2054(IntPtr objectPtr, ImGuiKey Key, [MarshalAs(UnmanagedType.I1)]bool Down);

	/// <summary><code>IMGUI_API void  AddKeyEvent(ImGuiKey key, bool down);                   </code>
		/// Queue a new key down/up event. Key should be "translated" (as in, generally ImGuiKey_A matches the key end-user would use to emit an 'A' character) </summary>
	public void AddKeyEvent(ImGuiKey Key, bool Down)
	{
		ImGuiIO_AddKeyEvent2054(_objectPtr, Key, Down);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_AddKeyAnalogEvent2055(IntPtr objectPtr, ImGuiKey Key, [MarshalAs(UnmanagedType.I1)]bool Down, float V);

	/// <summary><code>IMGUI_API void  AddKeyAnalogEvent(ImGuiKey key, bool down, float v);    </code>
		/// Queue a new key down/up event for analog values (e.g. ImGuiKey_Gamepad_ values). Dead-zones should be handled by the backend. </summary>
	public void AddKeyAnalogEvent(ImGuiKey Key, bool Down, float V)
	{
		ImGuiIO_AddKeyAnalogEvent2055(_objectPtr, Key, Down, V);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_AddMousePosEvent2056(IntPtr objectPtr, float X, float Y);

	/// <summary><code>IMGUI_API void  AddMousePosEvent(float x, float y);                     </code>
		/// Queue a mouse position update. Use -FLT_MAX,-FLT_MAX to signify no mouse (e.g. app not focused and not hovered) </summary>
	public void AddMousePosEvent(float X, float Y)
	{
		ImGuiIO_AddMousePosEvent2056(_objectPtr, X, Y);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_AddMouseButtonEvent2057(IntPtr objectPtr, int Button, [MarshalAs(UnmanagedType.I1)]bool Down);

	/// <summary><code>IMGUI_API void  AddMouseButtonEvent(int button, bool down);             </code>
		/// Queue a mouse button change </summary>
	public void AddMouseButtonEvent(int Button, bool Down)
	{
		ImGuiIO_AddMouseButtonEvent2057(_objectPtr, Button, Down);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_AddMouseWheelEvent2058(IntPtr objectPtr, float WheelX, float WheelY);

	/// <summary><code>IMGUI_API void  AddMouseWheelEvent(float wheel_x, float wheel_y);       </code>
		/// Queue a mouse wheel update. wheel_y<0: scroll down, wheel_y>0: scroll up, wheel_x<0: scroll right, wheel_x>0: scroll left. </summary>
	public void AddMouseWheelEvent(float WheelX, float WheelY)
	{
		ImGuiIO_AddMouseWheelEvent2058(_objectPtr, WheelX, WheelY);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_AddMouseSourceEvent2059(IntPtr objectPtr, ImGuiMouseSource Source);

	/// <summary><code>IMGUI_API void  AddMouseSourceEvent(ImGuiMouseSource source);           </code>
		/// Queue a mouse source change (Mouse/TouchScreen/Pen) </summary>
	public void AddMouseSourceEvent(ImGuiMouseSource Source)
	{
		ImGuiIO_AddMouseSourceEvent2059(_objectPtr, Source);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_AddFocusEvent2060(IntPtr objectPtr, [MarshalAs(UnmanagedType.I1)]bool Focused);

	/// <summary><code>IMGUI_API void  AddFocusEvent(bool focused);                            </code>
		/// Queue a gain/loss of focus for the application (generally based on OS/platform focus of your window) </summary>
	public void AddFocusEvent(bool Focused)
	{
		ImGuiIO_AddFocusEvent2060(_objectPtr, Focused);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_AddInputCharacter2061(IntPtr objectPtr, uint C);

	/// <summary><code>IMGUI_API void  AddInputCharacter(unsigned int c);                      </code>
		/// Queue a new character input </summary>
	public void AddInputCharacter(uint C)
	{
		ImGuiIO_AddInputCharacter2061(_objectPtr, C);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_AddInputCharacterUTF162062(IntPtr objectPtr, char C);

	/// <summary><code>IMGUI_API void  AddInputCharacterUTF16(ImWchar16 c);                    </code>
		/// Queue a new character input from a UTF-16 character, it can be a surrogate </summary>
	public void AddInputCharacterUTF16(char C)
	{
		ImGuiIO_AddInputCharacterUTF162062(_objectPtr, C);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_AddInputCharactersUTF82063(IntPtr objectPtr, [MarshalAs(UnmanagedType.LPStr)]string Str);

	/// <summary><code>IMGUI_API void  AddInputCharactersUTF8(const char* str);                </code>
		/// Queue a new characters input from a UTF-8 string </summary>
	public void AddInputCharactersUTF8(string Str)
	{
		ImGuiIO_AddInputCharactersUTF82063(_objectPtr, Str);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_SetKeyEventNativeData2065(IntPtr objectPtr, ImGuiKey Key, int NativeKeycode, int NativeScancode, int NativeLegacyIndex);

	/// <summary><code>IMGUI_API void  SetKeyEventNativeData(ImGuiKey key, int native_keycode, int native_scancode, int native_legacy_index = -1); </code>
		/// [Optional] Specify index for legacy <1.87 IsKeyXXX() functions with native indices + specify native keycode, scancode. </summary>
	public void SetKeyEventNativeData(ImGuiKey Key, int NativeKeycode, int NativeScancode)
	{
		ImGuiIO_SetKeyEventNativeData2065(_objectPtr, Key, NativeKeycode, NativeScancode, -1);
	}

	/// <summary><code>IMGUI_API void  SetKeyEventNativeData(ImGuiKey key, int native_keycode, int native_scancode, int native_legacy_index = -1); </code>
		/// [Optional] Specify index for legacy <1.87 IsKeyXXX() functions with native indices + specify native keycode, scancode. </summary>
	public void SetKeyEventNativeData(ImGuiKey Key, int NativeKeycode, int NativeScancode, int NativeLegacyIndex)
	{
		ImGuiIO_SetKeyEventNativeData2065(_objectPtr, Key, NativeKeycode, NativeScancode, NativeLegacyIndex);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_SetAppAcceptingEvents2066(IntPtr objectPtr, [MarshalAs(UnmanagedType.I1)]bool AcceptingEvents);

	/// <summary><code>IMGUI_API void  SetAppAcceptingEvents(bool accepting_events);           </code>
		/// Set master flag for accepting key/mouse/text events (default to true). Useful if you have native dialog boxes that are interrupting your application loop/refresh, and you want to disable events being queued while your app is frozen. </summary>
	public void SetAppAcceptingEvents(bool AcceptingEvents)
	{
		ImGuiIO_SetAppAcceptingEvents2066(_objectPtr, AcceptingEvents);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_ClearEventsQueue2067(IntPtr objectPtr);

	/// <summary><code>IMGUI_API void  ClearEventsQueue();                                     </code>
		/// Clear all incoming events. </summary>
	public void ClearEventsQueue()
	{
		ImGuiIO_ClearEventsQueue2067(_objectPtr);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_ClearInputKeys2068(IntPtr objectPtr);

	/// <summary><code>IMGUI_API void  ClearInputKeys();                                       </code>
		/// Clear current keyboard/mouse/gamepad state + current frame text input buffer. Equivalent to releasing all keys/buttons. </summary>
	public void ClearInputKeys()
	{
		ImGuiIO_ClearInputKeys2068(_objectPtr);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_ClearInputCharacters2070(IntPtr objectPtr);

	/// <summary><code>IMGUI_API void  ClearInputCharacters();                                 </code>
		/// [Obsolete] Clear the current frame text input buffer. Now included within ClearInputKeys(). </summary>
	public void ClearInputCharacters()
	{
		ImGuiIO_ClearInputCharacters2070(_objectPtr);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGuiIO_Get_WantCaptureMouse2079(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_WantCaptureMouse2079(IntPtr objectPtr, [MarshalAs(UnmanagedType.I1)]bool  Value);

	/// <summary><code>bool        WantCaptureMouse;                   </code>
		/// Set when Dear ImGui will use mouse inputs, in this case do not dispatch them to your main game/application (either way, always pass on mouse inputs to imgui). (e.g. unclicked mouse is hovering over an imgui window, widget is active, mouse was clicked over an imgui window, etc.). </summary>
	public bool WantCaptureMouse
	{
		get { return ImGuiIO_Get_WantCaptureMouse2079(_objectPtr);}
		set {ImGuiIO_Set_WantCaptureMouse2079(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGuiIO_Get_WantCaptureKeyboard2080(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_WantCaptureKeyboard2080(IntPtr objectPtr, [MarshalAs(UnmanagedType.I1)]bool  Value);

	/// <summary><code>bool        WantCaptureKeyboard;                </code>
		/// Set when Dear ImGui will use keyboard inputs, in this case do not dispatch them to your main game/application (either way, always pass keyboard inputs to imgui). (e.g. InputText active, or an imgui window is focused and navigation is enabled, etc.). </summary>
	public bool WantCaptureKeyboard
	{
		get { return ImGuiIO_Get_WantCaptureKeyboard2080(_objectPtr);}
		set {ImGuiIO_Set_WantCaptureKeyboard2080(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGuiIO_Get_WantTextInput2081(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_WantTextInput2081(IntPtr objectPtr, [MarshalAs(UnmanagedType.I1)]bool  Value);

	/// <summary><code>bool        WantTextInput;                      </code>
		/// Mobile/console: when set, you may display an on-screen keyboard. This is set by Dear ImGui when it wants textual keyboard input to happen (e.g. when a InputText widget is active). </summary>
	public bool WantTextInput
	{
		get { return ImGuiIO_Get_WantTextInput2081(_objectPtr);}
		set {ImGuiIO_Set_WantTextInput2081(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGuiIO_Get_WantSetMousePos2082(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_WantSetMousePos2082(IntPtr objectPtr, [MarshalAs(UnmanagedType.I1)]bool  Value);

	/// <summary><code>bool        WantSetMousePos;                    </code>
		/// MousePos has been altered, backend should reposition mouse on next frame. Rarely used! Set only when ImGuiConfigFlags_NavEnableSetMousePos flag is enabled. </summary>
	public bool WantSetMousePos
	{
		get { return ImGuiIO_Get_WantSetMousePos2082(_objectPtr);}
		set {ImGuiIO_Set_WantSetMousePos2082(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGuiIO_Get_WantSaveIniSettings2083(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_WantSaveIniSettings2083(IntPtr objectPtr, [MarshalAs(UnmanagedType.I1)]bool  Value);

	/// <summary><code>bool        WantSaveIniSettings;                </code>
		/// When manual .ini load/save is active (io.IniFilename == NULL), this will be set to notify your application that you can call SaveIniSettingsToMemory() and save yourself. Important: clear io.WantSaveIniSettings yourself after saving! </summary>
	public bool WantSaveIniSettings
	{
		get { return ImGuiIO_Get_WantSaveIniSettings2083(_objectPtr);}
		set {ImGuiIO_Set_WantSaveIniSettings2083(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGuiIO_Get_NavActive2084(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_NavActive2084(IntPtr objectPtr, [MarshalAs(UnmanagedType.I1)]bool  Value);

	/// <summary><code>bool        NavActive;                          </code>
		/// Keyboard/Gamepad navigation is currently allowed (will handle ImGuiKey_NavXXX events) = a window is focused and it doesn't use the ImGuiWindowFlags_NoNavInputs flag. </summary>
	public bool NavActive
	{
		get { return ImGuiIO_Get_NavActive2084(_objectPtr);}
		set {ImGuiIO_Set_NavActive2084(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGuiIO_Get_NavVisible2085(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_NavVisible2085(IntPtr objectPtr, [MarshalAs(UnmanagedType.I1)]bool  Value);

	/// <summary><code>bool        NavVisible;                         </code>
		/// Keyboard/Gamepad navigation is visible and allowed (will handle ImGuiKey_NavXXX events). </summary>
	public bool NavVisible
	{
		get { return ImGuiIO_Get_NavVisible2085(_objectPtr);}
		set {ImGuiIO_Set_NavVisible2085(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGuiIO_Get_Framerate2086(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_Framerate2086(IntPtr objectPtr, float  Value);

	/// <summary><code>float       Framerate;                          </code>
		/// Estimate of application framerate (rolling average over 60 frames, based on io.DeltaTime), in frame per second. Solely for convenience. Slow applications may not want to use a moving average or may want to reset underlying buffers occasionally. </summary>
	public float Framerate
	{
		get { return ImGuiIO_Get_Framerate2086(_objectPtr);}
		set {ImGuiIO_Set_Framerate2086(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern int ImGuiIO_Get_MetricsRenderVertices2087(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_MetricsRenderVertices2087(IntPtr objectPtr, int  Value);

	/// <summary><code>int         MetricsRenderVertices;              </code>
		/// Vertices output during last call to Render() </summary>
	public int MetricsRenderVertices
	{
		get { return ImGuiIO_Get_MetricsRenderVertices2087(_objectPtr);}
		set {ImGuiIO_Set_MetricsRenderVertices2087(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern int ImGuiIO_Get_MetricsRenderIndices2088(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_MetricsRenderIndices2088(IntPtr objectPtr, int  Value);

	/// <summary><code>int         MetricsRenderIndices;               </code>
		/// Indices output during last call to Render() = number of triangles * 3 </summary>
	public int MetricsRenderIndices
	{
		get { return ImGuiIO_Get_MetricsRenderIndices2088(_objectPtr);}
		set {ImGuiIO_Set_MetricsRenderIndices2088(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern int ImGuiIO_Get_MetricsRenderWindows2089(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_MetricsRenderWindows2089(IntPtr objectPtr, int  Value);

	/// <summary><code>int         MetricsRenderWindows;               </code>
		/// Number of visible windows </summary>
	public int MetricsRenderWindows
	{
		get { return ImGuiIO_Get_MetricsRenderWindows2089(_objectPtr);}
		set {ImGuiIO_Set_MetricsRenderWindows2089(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern int ImGuiIO_Get_MetricsActiveWindows2090(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_MetricsActiveWindows2090(IntPtr objectPtr, int  Value);

	/// <summary><code>int         MetricsActiveWindows;               </code>
		/// Number of active windows </summary>
	public int MetricsActiveWindows
	{
		get { return ImGuiIO_Get_MetricsActiveWindows2090(_objectPtr);}
		set {ImGuiIO_Set_MetricsActiveWindows2090(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern int ImGuiIO_Get_MetricsActiveAllocations2091(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_MetricsActiveAllocations2091(IntPtr objectPtr, int  Value);

	/// <summary><code>int         MetricsActiveAllocations;           </code>
		/// Number of active allocations, updated by MemAlloc/MemFree based on current context. May be off if you have multiple imgui contexts. </summary>
	public int MetricsActiveAllocations
	{
		get { return ImGuiIO_Get_MetricsActiveAllocations2091(_objectPtr);}
		set {ImGuiIO_Set_MetricsActiveAllocations2091(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern Vector2 ImGuiIO_Get_MouseDelta2092(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_MouseDelta2092(IntPtr objectPtr, Vector2  Value);

	/// <summary><code>ImVec2      MouseDelta;                         </code>
		/// Mouse delta. Note that this is zero if either current or previous position are invalid (-FLT_MAX,-FLT_MAX), so a disappearing/reappearing mouse won't have a huge delta. </summary>
	public Vector2 MouseDelta
	{
		get { return ImGuiIO_Get_MouseDelta2092(_objectPtr);}
		set {ImGuiIO_Set_MouseDelta2092(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.LPArray, SizeConst = (int)ImGuiKey._COUNT)]
	private static extern int[] ImGuiIO_Get_KeyMap2098(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_KeyMap2098(IntPtr objectPtr, [MarshalAs(UnmanagedType.LPArray, SizeConst = (int)ImGuiKey._COUNT)]int[]  Value);

	/// <summary><code>int         KeyMap[ImGuiKey_COUNT];             </code>
		/// [LEGACY] Input: map of indices into the KeysDown[512] entries array which represent your "native" keyboard state. The first 512 are now unused and should be kept zero. Legacy backend will write into KeyMap[] using ImGuiKey_ indices which are always >512. </summary>
	public int[] KeyMap
	{
		get { return ImGuiIO_Get_KeyMap2098(_objectPtr);}
		set {ImGuiIO_Set_KeyMap2098(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1, SizeConst = (int)ImGuiKey._COUNT)]
	private static extern bool[] ImGuiIO_Get_KeysDown2099(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_KeysDown2099(IntPtr objectPtr, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1, SizeConst = (int)ImGuiKey._COUNT)]bool[]  Value);

	/// <summary><code>bool        KeysDown[ImGuiKey_COUNT];           </code>
		/// [LEGACY] Input: Keyboard keys that are pressed (ideally left in the "native" order your engine has access to keyboard keys, so you can use your own defines/enums for keys). This used to be [512] sized. It is now ImGuiKey_COUNT to allow legacy io.KeysDown[GetKeyIndex(...)] to work without an overflow. </summary>
	public bool[] KeysDown
	{
		get { return ImGuiIO_Get_KeysDown2099(_objectPtr);}
		set {ImGuiIO_Set_KeysDown2099(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.LPArray, SizeConst = (int)ImGuiNavInput._COUNT)]
	private static extern float[] ImGuiIO_Get_NavInputs2100(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_NavInputs2100(IntPtr objectPtr, [MarshalAs(UnmanagedType.LPArray, SizeConst = (int)ImGuiNavInput._COUNT)]float[]  Value);

	/// <summary><code>float       NavInputs[ImGuiNavInput_COUNT];     </code>
		/// [LEGACY] Since 1.88, NavInputs[] was removed. Backends from 1.60 to 1.86 won't build. Feed gamepad inputs via io.AddKeyEvent() and ImGuiKey_GamepadXXX enums. </summary>
	public float[] NavInputs
	{
		get { return ImGuiIO_Get_NavInputs2100(_objectPtr);}
		set {ImGuiIO_Set_NavInputs2100(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern Vector2 ImGuiIO_Get_MousePos2112(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_MousePos2112(IntPtr objectPtr, Vector2  Value);

	/// <summary><code>ImVec2      MousePos;                           </code>
		/// Mouse position, in pixels. Set to ImVec2(-FLT_MAX, -FLT_MAX) if mouse is unavailable (on another screen, etc.) </summary>
	public Vector2 MousePos
	{
		get { return ImGuiIO_Get_MousePos2112(_objectPtr);}
		set {ImGuiIO_Set_MousePos2112(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1, SizeConst = 5)]
	private static extern bool[] ImGuiIO_Get_MouseDown2113(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_MouseDown2113(IntPtr objectPtr, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1, SizeConst = 5)]bool[]  Value);

	/// <summary><code>bool        MouseDown[5];                       </code>
		/// Mouse buttons: 0=left, 1=right, 2=middle + extras (ImGuiMouseButton_COUNT == 5). Dear ImGui mostly uses left and right buttons. Other buttons allow us to track if the mouse is being used by your application + available to user as a convenience via IsMouse** API. </summary>
	public bool[] MouseDown
	{
		get { return ImGuiIO_Get_MouseDown2113(_objectPtr);}
		set {ImGuiIO_Set_MouseDown2113(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGuiIO_Get_MouseWheel2114(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_MouseWheel2114(IntPtr objectPtr, float  Value);

	/// <summary><code>float       MouseWheel;                         </code>
		/// Mouse wheel Vertical: 1 unit scrolls about 5 lines text. >0 scrolls Up, <0 scrolls Down. Hold SHIFT to turn vertical scroll into horizontal scroll. </summary>
	public float MouseWheel
	{
		get { return ImGuiIO_Get_MouseWheel2114(_objectPtr);}
		set {ImGuiIO_Set_MouseWheel2114(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGuiIO_Get_MouseWheelH2115(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_MouseWheelH2115(IntPtr objectPtr, float  Value);

	/// <summary><code>float       MouseWheelH;                        </code>
		/// Mouse wheel Horizontal. >0 scrolls Left, <0 scrolls Right. Most users don't have a mouse with a horizontal wheel, may not be filled by all backends. </summary>
	public float MouseWheelH
	{
		get { return ImGuiIO_Get_MouseWheelH2115(_objectPtr);}
		set {ImGuiIO_Set_MouseWheelH2115(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern ImGuiMouseSource ImGuiIO_Get_MouseSource2116(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_MouseSource2116(IntPtr objectPtr, ImGuiMouseSource  Value);

	/// <summary><code>ImGuiMouseSource MouseSource;                   </code>
		/// Mouse actual input peripheral (Mouse/TouchScreen/Pen). </summary>
	public ImGuiMouseSource MouseSource
	{
		get { return ImGuiIO_Get_MouseSource2116(_objectPtr);}
		set {ImGuiIO_Set_MouseSource2116(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGuiIO_Get_KeyCtrl2117(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_KeyCtrl2117(IntPtr objectPtr, [MarshalAs(UnmanagedType.I1)]bool  Value);

	/// <summary><code>bool        KeyCtrl;                            </code>
		/// Keyboard modifier down: Control </summary>
	public bool KeyCtrl
	{
		get { return ImGuiIO_Get_KeyCtrl2117(_objectPtr);}
		set {ImGuiIO_Set_KeyCtrl2117(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGuiIO_Get_KeyShift2118(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_KeyShift2118(IntPtr objectPtr, [MarshalAs(UnmanagedType.I1)]bool  Value);

	/// <summary><code>bool        KeyShift;                           </code>
		/// Keyboard modifier down: Shift </summary>
	public bool KeyShift
	{
		get { return ImGuiIO_Get_KeyShift2118(_objectPtr);}
		set {ImGuiIO_Set_KeyShift2118(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGuiIO_Get_KeyAlt2119(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_KeyAlt2119(IntPtr objectPtr, [MarshalAs(UnmanagedType.I1)]bool  Value);

	/// <summary><code>bool        KeyAlt;                             </code>
		/// Keyboard modifier down: Alt </summary>
	public bool KeyAlt
	{
		get { return ImGuiIO_Get_KeyAlt2119(_objectPtr);}
		set {ImGuiIO_Set_KeyAlt2119(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGuiIO_Get_KeySuper2120(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_KeySuper2120(IntPtr objectPtr, [MarshalAs(UnmanagedType.I1)]bool  Value);

	/// <summary><code>bool        KeySuper;                           </code>
		/// Keyboard modifier down: Cmd/Super/Windows </summary>
	public bool KeySuper
	{
		get { return ImGuiIO_Get_KeySuper2120(_objectPtr);}
		set {ImGuiIO_Set_KeySuper2120(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern ImGuiKey ImGuiIO_Get_KeyMods2123(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_KeyMods2123(IntPtr objectPtr, ImGuiKey  Value);

	/// <summary><code>ImGuiKeyChord KeyMods;                          </code>
		/// Key mods flags (any of ImGuiMod_Ctrl/ImGuiMod_Shift/ImGuiMod_Alt/ImGuiMod_Super flags, same as io.KeyCtrl/KeyShift/KeyAlt/KeySuper but merged into flags. DOES NOT CONTAINS ImGuiMod_Shortcut which is pretranslated). Read-only, updated by NewFrame() </summary>
	public ImGuiKey KeyMods
	{
		get { return ImGuiIO_Get_KeyMods2123(_objectPtr);}
		set {ImGuiIO_Set_KeyMods2123(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.LPArray, SizeConst = (int)ImGuiKey._KeysData_SIZE)]
	private static extern ImGuiKeyData[] ImGuiIO_Get_KeysData2124(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_KeysData2124(IntPtr objectPtr, [MarshalAs(UnmanagedType.LPArray, SizeConst = (int)ImGuiKey._KeysData_SIZE)]ImGuiKeyData[]  Value);

	/// <summary><code>ImGuiKeyData  KeysData[ImGuiKey_KeysData_SIZE]; </code>
		/// Key state for all known keys. Use IsKeyXXX() functions to access this. </summary>
	public ImGuiKeyData[] KeysData
	{
		get { return ImGuiIO_Get_KeysData2124(_objectPtr);}
		set {ImGuiIO_Set_KeysData2124(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGuiIO_Get_WantCaptureMouseUnlessPopupClose2125(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_WantCaptureMouseUnlessPopupClose2125(IntPtr objectPtr, [MarshalAs(UnmanagedType.I1)]bool  Value);

	/// <summary><code>bool        WantCaptureMouseUnlessPopupClose;   </code>
		/// Alternative to WantCaptureMouse: (WantCaptureMouse == true && WantCaptureMouseUnlessPopupClose == false) when a click over void is expected to close a popup. </summary>
	public bool WantCaptureMouseUnlessPopupClose
	{
		get { return ImGuiIO_Get_WantCaptureMouseUnlessPopupClose2125(_objectPtr);}
		set {ImGuiIO_Set_WantCaptureMouseUnlessPopupClose2125(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern Vector2 ImGuiIO_Get_MousePosPrev2126(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_MousePosPrev2126(IntPtr objectPtr, Vector2  Value);

	/// <summary><code>ImVec2      MousePosPrev;                       </code>
		/// Previous mouse position (note that MouseDelta is not necessary == MousePos-MousePosPrev, in case either position is invalid) </summary>
	public Vector2 MousePosPrev
	{
		get { return ImGuiIO_Get_MousePosPrev2126(_objectPtr);}
		set {ImGuiIO_Set_MousePosPrev2126(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.LPArray, SizeConst = 5)]
	private static extern Vector2[] ImGuiIO_Get_MouseClickedPos2127(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_MouseClickedPos2127(IntPtr objectPtr, [MarshalAs(UnmanagedType.LPArray, SizeConst = 5)]Vector2[]  Value);

	/// <summary><code>ImVec2      MouseClickedPos[5];                 </code>
		/// Position at time of clicking </summary>
	public Vector2[] MouseClickedPos
	{
		get { return ImGuiIO_Get_MouseClickedPos2127(_objectPtr);}
		set {ImGuiIO_Set_MouseClickedPos2127(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.LPArray, SizeConst = 5)]
	private static extern double[] ImGuiIO_Get_MouseClickedTime2128(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_MouseClickedTime2128(IntPtr objectPtr, [MarshalAs(UnmanagedType.LPArray, SizeConst = 5)]double[]  Value);

	/// <summary><code>double      MouseClickedTime[5];                </code>
		/// Time of last click (used to figure out double-click) </summary>
	public double[] MouseClickedTime
	{
		get { return ImGuiIO_Get_MouseClickedTime2128(_objectPtr);}
		set {ImGuiIO_Set_MouseClickedTime2128(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1, SizeConst = 5)]
	private static extern bool[] ImGuiIO_Get_MouseClicked2129(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_MouseClicked2129(IntPtr objectPtr, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1, SizeConst = 5)]bool[]  Value);

	/// <summary><code>bool        MouseClicked[5];                    </code>
		/// Mouse button went from !Down to Down (same as MouseClickedCount[x] != 0) </summary>
	public bool[] MouseClicked
	{
		get { return ImGuiIO_Get_MouseClicked2129(_objectPtr);}
		set {ImGuiIO_Set_MouseClicked2129(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1, SizeConst = 5)]
	private static extern bool[] ImGuiIO_Get_MouseDoubleClicked2130(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_MouseDoubleClicked2130(IntPtr objectPtr, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1, SizeConst = 5)]bool[]  Value);

	/// <summary><code>bool        MouseDoubleClicked[5];              </code>
		/// Has mouse button been double-clicked? (same as MouseClickedCount[x] == 2) </summary>
	public bool[] MouseDoubleClicked
	{
		get { return ImGuiIO_Get_MouseDoubleClicked2130(_objectPtr);}
		set {ImGuiIO_Set_MouseDoubleClicked2130(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.LPArray, SizeConst = 5)]
	private static extern ushort[] ImGuiIO_Get_MouseClickedCount2131(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_MouseClickedCount2131(IntPtr objectPtr, [MarshalAs(UnmanagedType.LPArray, SizeConst = 5)]ushort[]  Value);

	/// <summary><code>ImU16       MouseClickedCount[5];               </code>
		/// == 0 (not clicked), == 1 (same as MouseClicked[]), == 2 (double-clicked), == 3 (triple-clicked) etc. when going from !Down to Down </summary>
	public ushort[] MouseClickedCount
	{
		get { return ImGuiIO_Get_MouseClickedCount2131(_objectPtr);}
		set {ImGuiIO_Set_MouseClickedCount2131(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.LPArray, SizeConst = 5)]
	private static extern ushort[] ImGuiIO_Get_MouseClickedLastCount2132(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_MouseClickedLastCount2132(IntPtr objectPtr, [MarshalAs(UnmanagedType.LPArray, SizeConst = 5)]ushort[]  Value);

	/// <summary><code>ImU16       MouseClickedLastCount[5];           </code>
		/// Count successive number of clicks. Stays valid after mouse release. Reset after another click is done. </summary>
	public ushort[] MouseClickedLastCount
	{
		get { return ImGuiIO_Get_MouseClickedLastCount2132(_objectPtr);}
		set {ImGuiIO_Set_MouseClickedLastCount2132(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1, SizeConst = 5)]
	private static extern bool[] ImGuiIO_Get_MouseReleased2133(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_MouseReleased2133(IntPtr objectPtr, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1, SizeConst = 5)]bool[]  Value);

	/// <summary><code>bool        MouseReleased[5];                   </code>
		/// Mouse button went from Down to !Down </summary>
	public bool[] MouseReleased
	{
		get { return ImGuiIO_Get_MouseReleased2133(_objectPtr);}
		set {ImGuiIO_Set_MouseReleased2133(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1, SizeConst = 5)]
	private static extern bool[] ImGuiIO_Get_MouseDownOwned2134(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_MouseDownOwned2134(IntPtr objectPtr, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1, SizeConst = 5)]bool[]  Value);

	/// <summary><code>bool        MouseDownOwned[5];                  </code>
		/// Track if button was clicked inside a dear imgui window or over void blocked by a popup. We don't request mouse capture from the application if click started outside ImGui bounds. </summary>
	public bool[] MouseDownOwned
	{
		get { return ImGuiIO_Get_MouseDownOwned2134(_objectPtr);}
		set {ImGuiIO_Set_MouseDownOwned2134(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1, SizeConst = 5)]
	private static extern bool[] ImGuiIO_Get_MouseDownOwnedUnlessPopupClose2135(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_MouseDownOwnedUnlessPopupClose2135(IntPtr objectPtr, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1, SizeConst = 5)]bool[]  Value);

	/// <summary><code>bool        MouseDownOwnedUnlessPopupClose[5];  </code>
		/// Track if button was clicked inside a dear imgui window. </summary>
	public bool[] MouseDownOwnedUnlessPopupClose
	{
		get { return ImGuiIO_Get_MouseDownOwnedUnlessPopupClose2135(_objectPtr);}
		set {ImGuiIO_Set_MouseDownOwnedUnlessPopupClose2135(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGuiIO_Get_MouseWheelRequestAxisSwap2136(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_MouseWheelRequestAxisSwap2136(IntPtr objectPtr, [MarshalAs(UnmanagedType.I1)]bool  Value);

	/// <summary><code>bool        MouseWheelRequestAxisSwap;          </code>
		/// On a non-Mac system, holding SHIFT requests WheelY to perform the equivalent of a WheelX event. On a Mac system this is already enforced by the system. </summary>
	public bool MouseWheelRequestAxisSwap
	{
		get { return ImGuiIO_Get_MouseWheelRequestAxisSwap2136(_objectPtr);}
		set {ImGuiIO_Set_MouseWheelRequestAxisSwap2136(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.LPArray, SizeConst = 5)]
	private static extern float[] ImGuiIO_Get_MouseDownDuration2137(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_MouseDownDuration2137(IntPtr objectPtr, [MarshalAs(UnmanagedType.LPArray, SizeConst = 5)]float[]  Value);

	/// <summary><code>float       MouseDownDuration[5];               </code>
		/// Duration the mouse button has been down (0.0f == just clicked) </summary>
	public float[] MouseDownDuration
	{
		get { return ImGuiIO_Get_MouseDownDuration2137(_objectPtr);}
		set {ImGuiIO_Set_MouseDownDuration2137(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.LPArray, SizeConst = 5)]
	private static extern float[] ImGuiIO_Get_MouseDownDurationPrev2138(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_MouseDownDurationPrev2138(IntPtr objectPtr, [MarshalAs(UnmanagedType.LPArray, SizeConst = 5)]float[]  Value);

	/// <summary><code>float       MouseDownDurationPrev[5];           </code>
		/// Previous time the mouse button has been down </summary>
	public float[] MouseDownDurationPrev
	{
		get { return ImGuiIO_Get_MouseDownDurationPrev2138(_objectPtr);}
		set {ImGuiIO_Set_MouseDownDurationPrev2138(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.LPArray, SizeConst = 5)]
	private static extern float[] ImGuiIO_Get_MouseDragMaxDistanceSqr2139(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_MouseDragMaxDistanceSqr2139(IntPtr objectPtr, [MarshalAs(UnmanagedType.LPArray, SizeConst = 5)]float[]  Value);

	/// <summary><code>float       MouseDragMaxDistanceSqr[5];         </code>
		/// Squared maximum distance of how much mouse has traveled from the clicking point (used for moving thresholds) </summary>
	public float[] MouseDragMaxDistanceSqr
	{
		get { return ImGuiIO_Get_MouseDragMaxDistanceSqr2139(_objectPtr);}
		set {ImGuiIO_Set_MouseDragMaxDistanceSqr2139(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGuiIO_Get_PenPressure2140(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_PenPressure2140(IntPtr objectPtr, float  Value);

	/// <summary><code>float       PenPressure;                        </code>
		/// Touch/Pen pressure (0.0f to 1.0f, should be >0.0f only when MouseDown[0] == true). Helper storage currently unused by Dear ImGui. </summary>
	public float PenPressure
	{
		get { return ImGuiIO_Get_PenPressure2140(_objectPtr);}
		set {ImGuiIO_Set_PenPressure2140(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGuiIO_Get_AppFocusLost2141(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_AppFocusLost2141(IntPtr objectPtr, [MarshalAs(UnmanagedType.I1)]bool  Value);

	/// <summary><code>bool        AppFocusLost;                       </code>
		/// Only modify via AddFocusEvent() </summary>
	public bool AppFocusLost
	{
		get { return ImGuiIO_Get_AppFocusLost2141(_objectPtr);}
		set {ImGuiIO_Set_AppFocusLost2141(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGuiIO_Get_AppAcceptingEvents2142(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_AppAcceptingEvents2142(IntPtr objectPtr, [MarshalAs(UnmanagedType.I1)]bool  Value);

	/// <summary><code>bool        AppAcceptingEvents;                 </code>
		/// Only modify via SetAppAcceptingEvents() </summary>
	public bool AppAcceptingEvents
	{
		get { return ImGuiIO_Get_AppAcceptingEvents2142(_objectPtr);}
		set {ImGuiIO_Set_AppAcceptingEvents2142(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern byte ImGuiIO_Get_BackendUsingLegacyKeyArrays2143(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_BackendUsingLegacyKeyArrays2143(IntPtr objectPtr, byte  Value);

	/// <summary><code>ImS8        BackendUsingLegacyKeyArrays;        </code>
		/// -1: unknown, 0: using AddKeyEvent(), 1: using legacy io.KeysDown[] </summary>
	public byte BackendUsingLegacyKeyArrays
	{
		get { return ImGuiIO_Get_BackendUsingLegacyKeyArrays2143(_objectPtr);}
		set {ImGuiIO_Set_BackendUsingLegacyKeyArrays2143(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGuiIO_Get_BackendUsingLegacyNavInputArray2144(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_BackendUsingLegacyNavInputArray2144(IntPtr objectPtr, [MarshalAs(UnmanagedType.I1)]bool  Value);

	/// <summary><code>bool        BackendUsingLegacyNavInputArray;    </code>
		/// 0: using AddKeyAnalogEvent(), 1: writing to legacy io.NavInputs[] directly </summary>
	public bool BackendUsingLegacyNavInputArray
	{
		get { return ImGuiIO_Get_BackendUsingLegacyNavInputArray2144(_objectPtr);}
		set {ImGuiIO_Set_BackendUsingLegacyNavInputArray2144(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern char ImGuiIO_Get_InputQueueSurrogate2145(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_InputQueueSurrogate2145(IntPtr objectPtr, char  Value);

	/// <summary><code>ImWchar16   InputQueueSurrogate;                </code>
		/// For AddInputCharacterUTF16() </summary>
	public char InputQueueSurrogate
	{
		get { return ImGuiIO_Get_InputQueueSurrogate2145(_objectPtr);}
		set {ImGuiIO_Set_InputQueueSurrogate2145(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.LPArray)]
	private static extern List<char> ImGuiIO_Get_InputQueueCharacters2146(out int ReturnListSize, IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiIO_Set_InputQueueCharacters2146(IntPtr objectPtr, [MarshalAs(UnmanagedType.LPArray)]List<char>  Value);

	/// <summary><code>ImVector<ImWchar> InputQueueCharacters;         </code>
		/// Queue of _characters_ input (obtained by platform backend). Fill using AddInputCharacter() helper. </summary>
	public List<char> InputQueueCharacters
	{
		get { return ImGuiIO_Get_InputQueueCharacters2146(out _, _objectPtr);}
		set {ImGuiIO_Set_InputQueueCharacters2146(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern ImGuiIO ImGuiIO_ImGuiIO2148();

	/// <summary><code>IMGUI_API   ImGuiIO();</code>
		///    IMGUI_API   ImGuiIO(); </summary>
	public  ImGuiIO()
	{
		_objectPtr = ImGuiIO_ImGuiIO2148()._objectPtr;
	}
	}
	public class ImGuiInputTextCallbackData
	{
		private IntPtr _objectPtr;
		public IntPtr AsPtr { get => _objectPtr; }
		public ImGuiInputTextCallbackData(IntPtr Ptr){ _objectPtr = Ptr; }

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern ImGuiInputTextFlags ImGuiInputTextCallbackData_Get_EventFlag2167(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiInputTextCallbackData_Set_EventFlag2167(IntPtr objectPtr, ImGuiInputTextFlags  Value);

	/// <summary><code>ImGuiInputTextFlags EventFlag;      </code>
		/// One ImGuiInputTextFlags_Callback*    // Read-only </summary>
	public ImGuiInputTextFlags EventFlag
	{
		get { return ImGuiInputTextCallbackData_Get_EventFlag2167(_objectPtr);}
		set {ImGuiInputTextCallbackData_Set_EventFlag2167(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern ImGuiInputTextFlags ImGuiInputTextCallbackData_Get_Flags2168(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiInputTextCallbackData_Set_Flags2168(IntPtr objectPtr, ImGuiInputTextFlags  Value);

	/// <summary><code>ImGuiInputTextFlags Flags;          </code>
		/// What user passed to InputText()      // Read-only </summary>
	public ImGuiInputTextFlags Flags
	{
		get { return ImGuiInputTextCallbackData_Get_Flags2168(_objectPtr);}
		set {ImGuiInputTextCallbackData_Set_Flags2168(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern IntPtr ImGuiInputTextCallbackData_Get_UserData2169(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiInputTextCallbackData_Set_UserData2169(IntPtr objectPtr, IntPtr  Value);

	/// <summary><code>void*               UserData;       </code>
		/// What user passed to InputText()      // Read-only </summary>
	public IntPtr UserData
	{
		get { return ImGuiInputTextCallbackData_Get_UserData2169(_objectPtr);}
		set {ImGuiInputTextCallbackData_Set_UserData2169(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern char ImGuiInputTextCallbackData_Get_EventChar2174(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiInputTextCallbackData_Set_EventChar2174(IntPtr objectPtr, char  Value);

	/// <summary><code>ImWchar             EventChar;      </code>
		/// Character input                      // Read-write   // [CharFilter] Replace character with another one, or set to zero to drop. return 1 is equivalent to setting EventChar=0; </summary>
	public char EventChar
	{
		get { return ImGuiInputTextCallbackData_Get_EventChar2174(_objectPtr);}
		set {ImGuiInputTextCallbackData_Set_EventChar2174(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern ImGuiKey ImGuiInputTextCallbackData_Get_EventKey2175(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiInputTextCallbackData_Set_EventKey2175(IntPtr objectPtr, ImGuiKey  Value);

	/// <summary><code>ImGuiKey            EventKey;       </code>
		/// Key pressed (Up/Down/TAB)            // Read-only    // [Completion,History] </summary>
	public ImGuiKey EventKey
	{
		get { return ImGuiInputTextCallbackData_Get_EventKey2175(_objectPtr);}
		set {ImGuiInputTextCallbackData_Set_EventKey2175(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.LPStr)]
	private static extern string ImGuiInputTextCallbackData_Get_Buf2176(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiInputTextCallbackData_Set_Buf2176(IntPtr objectPtr, [MarshalAs(UnmanagedType.LPStr)]string  Value);

	/// <summary><code>char*               Buf;            </code>
		/// Text buffer                          // Read-write   // [Resize] Can replace pointer / [Completion,History,Always] Only write to pointed data, don't replace the actual pointer! </summary>
	public string Buf
	{
		get { return ImGuiInputTextCallbackData_Get_Buf2176(_objectPtr);}
		set {ImGuiInputTextCallbackData_Set_Buf2176(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern int ImGuiInputTextCallbackData_Get_BufTextLen2177(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiInputTextCallbackData_Set_BufTextLen2177(IntPtr objectPtr, int  Value);

	/// <summary><code>int                 BufTextLen;     </code>
		/// Text length (in bytes)               // Read-write   // [Resize,Completion,History,Always] Exclude zero-terminator storage. In C land: == strlen(some_text), in C++ land: string.length() </summary>
	public int BufTextLen
	{
		get { return ImGuiInputTextCallbackData_Get_BufTextLen2177(_objectPtr);}
		set {ImGuiInputTextCallbackData_Set_BufTextLen2177(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern int ImGuiInputTextCallbackData_Get_BufSize2178(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiInputTextCallbackData_Set_BufSize2178(IntPtr objectPtr, int  Value);

	/// <summary><code>int                 BufSize;        </code>
		/// Buffer size (in bytes) = capacity+1  // Read-only    // [Resize,Completion,History,Always] Include zero-terminator storage. In C land == ARRAYSIZE(my_char_array), in C++ land: string.capacity()+1 </summary>
	public int BufSize
	{
		get { return ImGuiInputTextCallbackData_Get_BufSize2178(_objectPtr);}
		set {ImGuiInputTextCallbackData_Set_BufSize2178(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGuiInputTextCallbackData_Get_BufDirty2179(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiInputTextCallbackData_Set_BufDirty2179(IntPtr objectPtr, [MarshalAs(UnmanagedType.I1)]bool  Value);

	/// <summary><code>bool                BufDirty;       </code>
		/// Set if you modify Buf/BufTextLen!    // Write        // [Completion,History,Always] </summary>
	public bool BufDirty
	{
		get { return ImGuiInputTextCallbackData_Get_BufDirty2179(_objectPtr);}
		set {ImGuiInputTextCallbackData_Set_BufDirty2179(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern int ImGuiInputTextCallbackData_Get_CursorPos2180(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiInputTextCallbackData_Set_CursorPos2180(IntPtr objectPtr, int  Value);

	/// <summary><code>int                 CursorPos;      </code>
		///                                      // Read-write   // [Completion,History,Always] </summary>
	public int CursorPos
	{
		get { return ImGuiInputTextCallbackData_Get_CursorPos2180(_objectPtr);}
		set {ImGuiInputTextCallbackData_Set_CursorPos2180(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern int ImGuiInputTextCallbackData_Get_SelectionStart2181(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiInputTextCallbackData_Set_SelectionStart2181(IntPtr objectPtr, int  Value);

	/// <summary><code>int                 SelectionStart; </code>
		///                                      // Read-write   // [Completion,History,Always] == to SelectionEnd when no selection) </summary>
	public int SelectionStart
	{
		get { return ImGuiInputTextCallbackData_Get_SelectionStart2181(_objectPtr);}
		set {ImGuiInputTextCallbackData_Set_SelectionStart2181(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern int ImGuiInputTextCallbackData_Get_SelectionEnd2182(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiInputTextCallbackData_Set_SelectionEnd2182(IntPtr objectPtr, int  Value);

	/// <summary><code>int                 SelectionEnd;   </code>
		///                                      // Read-write   // [Completion,History,Always] </summary>
	public int SelectionEnd
	{
		get { return ImGuiInputTextCallbackData_Get_SelectionEnd2182(_objectPtr);}
		set {ImGuiInputTextCallbackData_Set_SelectionEnd2182(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern ImGuiInputTextCallbackData ImGuiInputTextCallbackData_ImGuiInputTextCallbackData2186();

	/// <summary><code>IMGUI_API ImGuiInputTextCallbackData();</code>
		///    IMGUI_API ImGuiInputTextCallbackData(); </summary>
	public  ImGuiInputTextCallbackData()
	{
		_objectPtr = ImGuiInputTextCallbackData_ImGuiInputTextCallbackData2186()._objectPtr;
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiInputTextCallbackData_DeleteChars2187(IntPtr objectPtr, int Pos, int BytesCount);

	/// <summary><code>IMGUI_API void      DeleteChars(int pos, int bytes_count);</code>
		///    IMGUI_API void      DeleteChars(int pos, int bytes_count); </summary>
	public void DeleteChars(int Pos, int BytesCount)
	{
		ImGuiInputTextCallbackData_DeleteChars2187(_objectPtr, Pos, BytesCount);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiInputTextCallbackData_InsertChars2188(IntPtr objectPtr, int Pos, [MarshalAs(UnmanagedType.LPStr)]string Text, [MarshalAs(UnmanagedType.LPStr)]string TextEnd);

	/// <summary><code>IMGUI_API void      InsertChars(int pos, const char* text, const char* text_end = NULL);</code>
		///    IMGUI_API void      InsertChars(int pos, const char* text, const char* text_end = NULL); </summary>
	public void InsertChars(int Pos, string Text)
	{
		ImGuiInputTextCallbackData_InsertChars2188(_objectPtr, Pos, Text, default);
	}

	/// <summary><code>IMGUI_API void      InsertChars(int pos, const char* text, const char* text_end = NULL);</code>
		///    IMGUI_API void      InsertChars(int pos, const char* text, const char* text_end = NULL); </summary>
	public void InsertChars(int Pos, string Text, string TextEnd)
	{
		ImGuiInputTextCallbackData_InsertChars2188(_objectPtr, Pos, Text, TextEnd);
	}
	}
	public class ImGuiSizeCallbackData
	{
		private IntPtr _objectPtr;
		public IntPtr AsPtr { get => _objectPtr; }
		public ImGuiSizeCallbackData(IntPtr Ptr){ _objectPtr = Ptr; }

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern IntPtr ImGuiSizeCallbackData_Get_UserData2198(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiSizeCallbackData_Set_UserData2198(IntPtr objectPtr, IntPtr  Value);

	/// <summary><code>void*   UserData;       </code>
		/// Read-only.   What user passed to SetNextWindowSizeConstraints(). Generally store an integer or float in here (need reinterpret_cast<>). </summary>
	public IntPtr UserData
	{
		get { return ImGuiSizeCallbackData_Get_UserData2198(_objectPtr);}
		set {ImGuiSizeCallbackData_Set_UserData2198(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern Vector2 ImGuiSizeCallbackData_Get_Pos2199(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiSizeCallbackData_Set_Pos2199(IntPtr objectPtr, Vector2  Value);

	/// <summary><code>ImVec2  Pos;            </code>
		/// Read-only.   Window position, for reference. </summary>
	public Vector2 Pos
	{
		get { return ImGuiSizeCallbackData_Get_Pos2199(_objectPtr);}
		set {ImGuiSizeCallbackData_Set_Pos2199(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern Vector2 ImGuiSizeCallbackData_Get_CurrentSize2200(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiSizeCallbackData_Set_CurrentSize2200(IntPtr objectPtr, Vector2  Value);

	/// <summary><code>ImVec2  CurrentSize;    </code>
		/// Read-only.   Current window size. </summary>
	public Vector2 CurrentSize
	{
		get { return ImGuiSizeCallbackData_Get_CurrentSize2200(_objectPtr);}
		set {ImGuiSizeCallbackData_Set_CurrentSize2200(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern Vector2 ImGuiSizeCallbackData_Get_DesiredSize2201(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiSizeCallbackData_Set_DesiredSize2201(IntPtr objectPtr, Vector2  Value);

	/// <summary><code>ImVec2  DesiredSize;    </code>
		/// Read-write.  Desired size, based on user's mouse position. Write to this field to restrain resizing. </summary>
	public Vector2 DesiredSize
	{
		get { return ImGuiSizeCallbackData_Get_DesiredSize2201(_objectPtr);}
		set {ImGuiSizeCallbackData_Set_DesiredSize2201(_objectPtr, value);}
	}	}
	public class ImGuiPayload
	{
		private IntPtr _objectPtr;
		public IntPtr AsPtr { get => _objectPtr; }
		public ImGuiPayload(IntPtr Ptr){ _objectPtr = Ptr; }

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern IntPtr ImGuiPayload_Get_Data2208(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiPayload_Set_Data2208(IntPtr objectPtr, IntPtr  Value);

	/// <summary><code>void*           Data;               </code>
		/// Data (copied and owned by dear imgui) </summary>
	public IntPtr Data
	{
		get { return ImGuiPayload_Get_Data2208(_objectPtr);}
		set {ImGuiPayload_Set_Data2208(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern int ImGuiPayload_Get_DataSize2209(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiPayload_Set_DataSize2209(IntPtr objectPtr, int  Value);

	/// <summary><code>int             DataSize;           </code>
		/// Data size </summary>
	public int DataSize
	{
		get { return ImGuiPayload_Get_DataSize2209(_objectPtr);}
		set {ImGuiPayload_Set_DataSize2209(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern ImGuiID ImGuiPayload_Get_SourceId2212(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiPayload_Set_SourceId2212(IntPtr objectPtr, ImGuiID  Value);

	/// <summary><code>ImGuiID         SourceId;           </code>
		/// Source item id </summary>
	public ImGuiID SourceId
	{
		get { return ImGuiPayload_Get_SourceId2212(_objectPtr);}
		set {ImGuiPayload_Set_SourceId2212(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern ImGuiID ImGuiPayload_Get_SourceParentId2213(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiPayload_Set_SourceParentId2213(IntPtr objectPtr, ImGuiID  Value);

	/// <summary><code>ImGuiID         SourceParentId;     </code>
		/// Source parent id (if available) </summary>
	public ImGuiID SourceParentId
	{
		get { return ImGuiPayload_Get_SourceParentId2213(_objectPtr);}
		set {ImGuiPayload_Set_SourceParentId2213(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern int ImGuiPayload_Get_DataFrameCount2214(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiPayload_Set_DataFrameCount2214(IntPtr objectPtr, int  Value);

	/// <summary><code>int             DataFrameCount;     </code>
		/// Data timestamp </summary>
	public int DataFrameCount
	{
		get { return ImGuiPayload_Get_DataFrameCount2214(_objectPtr);}
		set {ImGuiPayload_Set_DataFrameCount2214(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.LPArray, SizeConst = 32 + 1)]
	private static extern char[] ImGuiPayload_Get_DataType2215(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiPayload_Set_DataType2215(IntPtr objectPtr, [MarshalAs(UnmanagedType.LPArray, SizeConst = 32 + 1)]char[]  Value);

	/// <summary><code>char            DataType[32 + 1];   </code>
		/// Data type tag (short user-supplied string, 32 characters max) </summary>
	public char[] DataType
	{
		get { return ImGuiPayload_Get_DataType2215(_objectPtr);}
		set {ImGuiPayload_Set_DataType2215(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGuiPayload_Get_Preview2216(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiPayload_Set_Preview2216(IntPtr objectPtr, [MarshalAs(UnmanagedType.I1)]bool  Value);

	/// <summary><code>bool            Preview;            </code>
		/// Set when AcceptDragDropPayload() was called and mouse has been hovering the target item (nb: handle overlapping drag targets) </summary>
	public bool Preview
	{
		get { return ImGuiPayload_Get_Preview2216(_objectPtr);}
		set {ImGuiPayload_Set_Preview2216(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGuiPayload_Get_Delivery2217(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiPayload_Set_Delivery2217(IntPtr objectPtr, [MarshalAs(UnmanagedType.I1)]bool  Value);

	/// <summary><code>bool            Delivery;           </code>
		/// Set when AcceptDragDropPayload() was called and mouse button is released over the target item. </summary>
	public bool Delivery
	{
		get { return ImGuiPayload_Get_Delivery2217(_objectPtr);}
		set {ImGuiPayload_Set_Delivery2217(_objectPtr, value);}
	}	}
	public class ImGuiTableColumnSortSpecs
	{
		private IntPtr _objectPtr;
		public IntPtr AsPtr { get => _objectPtr; }
		public ImGuiTableColumnSortSpecs(IntPtr Ptr){ _objectPtr = Ptr; }

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern ImGuiID ImGuiTableColumnSortSpecs_Get_ColumnUserID2229(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiTableColumnSortSpecs_Set_ColumnUserID2229(IntPtr objectPtr, ImGuiID  Value);

	/// <summary><code>ImGuiID                     ColumnUserID;       </code>
		/// User id of the column (if specified by a TableSetupColumn() call) </summary>
	public ImGuiID ColumnUserID
	{
		get { return ImGuiTableColumnSortSpecs_Get_ColumnUserID2229(_objectPtr);}
		set {ImGuiTableColumnSortSpecs_Set_ColumnUserID2229(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern short ImGuiTableColumnSortSpecs_Get_ColumnIndex2230(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiTableColumnSortSpecs_Set_ColumnIndex2230(IntPtr objectPtr, short  Value);

	/// <summary><code>ImS16                       ColumnIndex;        </code>
		/// Index of the column </summary>
	public short ColumnIndex
	{
		get { return ImGuiTableColumnSortSpecs_Get_ColumnIndex2230(_objectPtr);}
		set {ImGuiTableColumnSortSpecs_Set_ColumnIndex2230(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern short ImGuiTableColumnSortSpecs_Get_SortOrder2231(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiTableColumnSortSpecs_Set_SortOrder2231(IntPtr objectPtr, short  Value);

	/// <summary><code>ImS16                       SortOrder;          </code>
		/// Index within parent ImGuiTableSortSpecs (always stored in order starting from 0, tables sorted on a single criteria will always have a 0 here) </summary>
	public short SortOrder
	{
		get { return ImGuiTableColumnSortSpecs_Get_SortOrder2231(_objectPtr);}
		set {ImGuiTableColumnSortSpecs_Set_SortOrder2231(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern ImGuiSortDirection ImGuiTableColumnSortSpecs_Get_SortDirection2232(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiTableColumnSortSpecs_Set_SortDirection2232(IntPtr objectPtr, ImGuiSortDirection  Value);

	/// <summary><code>ImGuiSortDirection          SortDirection : 8;  </code>
		/// ImGuiSortDirection_Ascending or ImGuiSortDirection_Descending (you can use this or SortSign, whichever is more convenient for your sort function) </summary>
	public ImGuiSortDirection SortDirection
	{
		get { return ImGuiTableColumnSortSpecs_Get_SortDirection2232(_objectPtr);}
		set {ImGuiTableColumnSortSpecs_Set_SortDirection2232(_objectPtr, value);}
	}	}
	public class ImGuiTableSortSpecs
	{
		private IntPtr _objectPtr;
		public IntPtr AsPtr { get => _objectPtr; }
		public ImGuiTableSortSpecs(IntPtr Ptr){ _objectPtr = Ptr; }

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern IntPtr ImGuiTableSortSpecs_Get_Specs2243(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiTableSortSpecs_Set_Specs2243(IntPtr objectPtr, IntPtr  Value);

	/// <summary><code>const ImGuiTableColumnSortSpecs* Specs;     </code>
		/// Pointer to sort spec array. </summary>
	public  ImGuiTableColumnSortSpecs Specs
	{
		get { return new  ImGuiTableColumnSortSpecs(ImGuiTableSortSpecs_Get_Specs2243(_objectPtr));}
		set {ImGuiTableSortSpecs_Set_Specs2243(_objectPtr, value.AsPtr);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern int ImGuiTableSortSpecs_Get_SpecsCount2244(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiTableSortSpecs_Set_SpecsCount2244(IntPtr objectPtr, int  Value);

	/// <summary><code>int                         SpecsCount;     </code>
		/// Sort spec count. Most often 1. May be > 1 when ImGuiTableFlags_SortMulti is enabled. May be == 0 when ImGuiTableFlags_SortTristate is enabled. </summary>
	public int SpecsCount
	{
		get { return ImGuiTableSortSpecs_Get_SpecsCount2244(_objectPtr);}
		set {ImGuiTableSortSpecs_Set_SpecsCount2244(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGuiTableSortSpecs_Get_SpecsDirty2245(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiTableSortSpecs_Set_SpecsDirty2245(IntPtr objectPtr, [MarshalAs(UnmanagedType.I1)]bool  Value);

	/// <summary><code>bool                        SpecsDirty;     </code>
		/// Set to true when specs have changed since last time! Use this to sort again, then clear the flag. </summary>
	public bool SpecsDirty
	{
		get { return ImGuiTableSortSpecs_Get_SpecsDirty2245(_objectPtr);}
		set {ImGuiTableSortSpecs_Set_SpecsDirty2245(_objectPtr, value);}
	}	}
	public class ImGuiOnceUponAFrame
	{
		private IntPtr _objectPtr;
		public IntPtr AsPtr { get => _objectPtr; }
		public ImGuiOnceUponAFrame(IntPtr Ptr){ _objectPtr = Ptr; }

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern  int ImGuiOnceUponAFrame_Get_RefFrame2267(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiOnceUponAFrame_Set_RefFrame2267(IntPtr objectPtr,  int  Value);

	/// <summary><code>mutable int RefFrame;</code>
		///    mutable int RefFrame; </summary>
	public  int RefFrame
	{
		get { return ImGuiOnceUponAFrame_Get_RefFrame2267(_objectPtr);}
		set {ImGuiOnceUponAFrame_Set_RefFrame2267(_objectPtr, value);}
	}	}
	public class ImGuiTextFilter
	{
		private IntPtr _objectPtr;
		public IntPtr AsPtr { get => _objectPtr; }
		public ImGuiTextFilter(IntPtr Ptr){ _objectPtr = Ptr; }

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern ImGuiTextFilter ImGuiTextFilter_ImGuiTextFilter2274([MarshalAs(UnmanagedType.LPStr)]string DefaultFilter);

	/// <summary><code>IMGUI_API           ImGuiTextFilter(const char* default_filter = "");</code>
		///    IMGUI_API           ImGuiTextFilter(const char* default_filter = ""); </summary>
	public  ImGuiTextFilter()
	{
		_objectPtr = ImGuiTextFilter_ImGuiTextFilter2274("")._objectPtr;
	}

	/// <summary><code>IMGUI_API           ImGuiTextFilter(const char* default_filter = "");</code>
		///    IMGUI_API           ImGuiTextFilter(const char* default_filter = ""); </summary>
	public  ImGuiTextFilter(string DefaultFilter)
	{
		_objectPtr = ImGuiTextFilter_ImGuiTextFilter2274(DefaultFilter)._objectPtr;
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGuiTextFilter_Draw2275(IntPtr objectPtr, [MarshalAs(UnmanagedType.LPStr)]string Label, float Width);

	/// <summary><code>IMGUI_API bool      Draw(const char* label = "Filter (inc,-exc)", float width = 0.0f);  </code>
		/// Helper calling InputText+Build </summary>
	public bool Draw()
	{
		return ImGuiTextFilter_Draw2275(_objectPtr, "Filter (inc, -exc)", (float)0.0f);
	}

	/// <summary><code>IMGUI_API bool      Draw(const char* label = "Filter (inc,-exc)", float width = 0.0f);  </code>
		/// Helper calling InputText+Build </summary>
	public bool Draw(string Label)
	{
		return ImGuiTextFilter_Draw2275(_objectPtr, Label, (float)0.0f);
	}

	/// <summary><code>IMGUI_API bool      Draw(const char* label = "Filter (inc,-exc)", float width = 0.0f);  </code>
		/// Helper calling InputText+Build </summary>
	public bool Draw(string Label, float Width)
	{
		return ImGuiTextFilter_Draw2275(_objectPtr, Label, Width);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGuiTextFilter_PassFilter2276(IntPtr objectPtr, [MarshalAs(UnmanagedType.LPStr)]string Text, [MarshalAs(UnmanagedType.LPStr)]string TextEnd);

	/// <summary><code>IMGUI_API bool      PassFilter(const char* text, const char* text_end = NULL) const;</code>
		///    IMGUI_API bool      PassFilter(const char* text, const char* text_end = NULL) const; </summary>
	public bool PassFilter(string Text)
	{
		return ImGuiTextFilter_PassFilter2276(_objectPtr, Text, default);
	}

	/// <summary><code>IMGUI_API bool      PassFilter(const char* text, const char* text_end = NULL) const;</code>
		///    IMGUI_API bool      PassFilter(const char* text, const char* text_end = NULL) const; </summary>
	public bool PassFilter(string Text, string TextEnd)
	{
		return ImGuiTextFilter_PassFilter2276(_objectPtr, Text, TextEnd);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiTextFilter_Build2277(IntPtr objectPtr);

	/// <summary><code>IMGUI_API void      Build();</code>
		///    IMGUI_API void      Build(); </summary>
	public void Build()
	{
		ImGuiTextFilter_Build2277(_objectPtr);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.LPArray, SizeConst = 256)]
	private static extern char[] ImGuiTextFilter_Get_InputBuf2292(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiTextFilter_Set_InputBuf2292(IntPtr objectPtr, [MarshalAs(UnmanagedType.LPArray, SizeConst = 256)]char[]  Value);

	/// <summary><code>char                    InputBuf[256];</code>
		///    char                    InputBuf[256]; </summary>
	public char[] InputBuf
	{
		get { return ImGuiTextFilter_Get_InputBuf2292(_objectPtr);}
		set {ImGuiTextFilter_Set_InputBuf2292(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern int ImGuiTextFilter_Get_CountGrep2294(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiTextFilter_Set_CountGrep2294(IntPtr objectPtr, int  Value);

	/// <summary><code>int                     CountGrep;</code>
		///    int                     CountGrep; </summary>
	public int CountGrep
	{
		get { return ImGuiTextFilter_Get_CountGrep2294(_objectPtr);}
		set {ImGuiTextFilter_Set_CountGrep2294(_objectPtr, value);}
	}	}
	public class ImGuiTextBuffer
	{
		private IntPtr _objectPtr;
		public IntPtr AsPtr { get => _objectPtr; }
		public ImGuiTextBuffer(IntPtr Ptr){ _objectPtr = Ptr; }

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.LPArray)]
	private static extern List<char> ImGuiTextBuffer_Get_Buf2301(out int ReturnListSize, IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiTextBuffer_Set_Buf2301(IntPtr objectPtr, [MarshalAs(UnmanagedType.LPArray)]List<char>  Value);

	/// <summary><code>ImVector<char>      Buf;</code>
		///    ImVector<char>      Buf; </summary>
	public List<char> Buf
	{
		get { return ImGuiTextBuffer_Get_Buf2301(out _, _objectPtr);}
		set {ImGuiTextBuffer_Set_Buf2301(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.LPArray, SizeConst = 1)]
	private static extern char[] ImGuiTextBuffer_Get_EmptyString2302();

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiTextBuffer_Set_EmptyString2302([MarshalAs(UnmanagedType.LPArray, SizeConst = 1)]char[]  Value);

	/// <summary><code>IMGUI_API static char EmptyString[1];</code>
		///    IMGUI_API static char EmptyString[1]; </summary>
	public static char[] EmptyString
	{
		get { return ImGuiTextBuffer_Get_EmptyString2302();}
		set {ImGuiTextBuffer_Set_EmptyString2302(value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiTextBuffer_append2313(IntPtr objectPtr, [MarshalAs(UnmanagedType.LPStr)]string Str, [MarshalAs(UnmanagedType.LPStr)]string StrEnd);

	/// <summary><code>IMGUI_API void      append(const char* str, const char* str_end = NULL);</code>
		///    IMGUI_API void      append(const char* str, const char* str_end = NULL); </summary>
	public void append(string Str)
	{
		ImGuiTextBuffer_append2313(_objectPtr, Str, default);
	}

	/// <summary><code>IMGUI_API void      append(const char* str, const char* str_end = NULL);</code>
		///    IMGUI_API void      append(const char* str, const char* str_end = NULL); </summary>
	public void append(string Str, string StrEnd)
	{
		ImGuiTextBuffer_append2313(_objectPtr, Str, StrEnd);
	}
	}
	public class ImGuiStorage
	{
		private IntPtr _objectPtr;
		public IntPtr AsPtr { get => _objectPtr; }
		public ImGuiStorage(IntPtr Ptr){ _objectPtr = Ptr; }

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern int ImGuiStorage_GetInt2344(IntPtr objectPtr, ImGuiID Key, int DefaultVal);

	/// <summary><code>IMGUI_API int       GetInt(ImGuiID key, int default_val = 0) const;</code>
		///    IMGUI_API int       GetInt(ImGuiID key, int default_val = 0) const; </summary>
	public int GetInt(ImGuiID Key)
	{
		return ImGuiStorage_GetInt2344(_objectPtr, Key, (int)0);
	}

	/// <summary><code>IMGUI_API int       GetInt(ImGuiID key, int default_val = 0) const;</code>
		///    IMGUI_API int       GetInt(ImGuiID key, int default_val = 0) const; </summary>
	public int GetInt(ImGuiID Key, int DefaultVal)
	{
		return ImGuiStorage_GetInt2344(_objectPtr, Key, DefaultVal);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStorage_SetInt2345(IntPtr objectPtr, ImGuiID Key, int Val);

	/// <summary><code>IMGUI_API void      SetInt(ImGuiID key, int val);</code>
		///    IMGUI_API void      SetInt(ImGuiID key, int val); </summary>
	public void SetInt(ImGuiID Key, int Val)
	{
		ImGuiStorage_SetInt2345(_objectPtr, Key, Val);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGuiStorage_GetBool2346(IntPtr objectPtr, ImGuiID Key, [MarshalAs(UnmanagedType.I1)]bool DefaultVal);

	/// <summary><code>IMGUI_API bool      GetBool(ImGuiID key, bool default_val = false) const;</code>
		///    IMGUI_API bool      GetBool(ImGuiID key, bool default_val = false) const; </summary>
	public bool GetBool(ImGuiID Key)
	{
		return ImGuiStorage_GetBool2346(_objectPtr, Key, false);
	}

	/// <summary><code>IMGUI_API bool      GetBool(ImGuiID key, bool default_val = false) const;</code>
		///    IMGUI_API bool      GetBool(ImGuiID key, bool default_val = false) const; </summary>
	public bool GetBool(ImGuiID Key, bool DefaultVal)
	{
		return ImGuiStorage_GetBool2346(_objectPtr, Key, DefaultVal);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStorage_SetBool2347(IntPtr objectPtr, ImGuiID Key, [MarshalAs(UnmanagedType.I1)]bool Val);

	/// <summary><code>IMGUI_API void      SetBool(ImGuiID key, bool val);</code>
		///    IMGUI_API void      SetBool(ImGuiID key, bool val); </summary>
	public void SetBool(ImGuiID Key, bool Val)
	{
		ImGuiStorage_SetBool2347(_objectPtr, Key, Val);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGuiStorage_GetFloat2348(IntPtr objectPtr, ImGuiID Key, float DefaultVal);

	/// <summary><code>IMGUI_API float     GetFloat(ImGuiID key, float default_val = 0.0f) const;</code>
		///    IMGUI_API float     GetFloat(ImGuiID key, float default_val = 0.0f) const; </summary>
	public float GetFloat(ImGuiID Key)
	{
		return ImGuiStorage_GetFloat2348(_objectPtr, Key, (float)0.0f);
	}

	/// <summary><code>IMGUI_API float     GetFloat(ImGuiID key, float default_val = 0.0f) const;</code>
		///    IMGUI_API float     GetFloat(ImGuiID key, float default_val = 0.0f) const; </summary>
	public float GetFloat(ImGuiID Key, float DefaultVal)
	{
		return ImGuiStorage_GetFloat2348(_objectPtr, Key, DefaultVal);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStorage_SetFloat2349(IntPtr objectPtr, ImGuiID Key, float Val);

	/// <summary><code>IMGUI_API void      SetFloat(ImGuiID key, float val);</code>
		///    IMGUI_API void      SetFloat(ImGuiID key, float val); </summary>
	public void SetFloat(ImGuiID Key, float Val)
	{
		ImGuiStorage_SetFloat2349(_objectPtr, Key, Val);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern IntPtr ImGuiStorage_GetVoidPtr2350(IntPtr objectPtr, ImGuiID Key);

	/// <summary><code>IMGUI_API void*     GetVoidPtr(ImGuiID key) const; </code>
		/// default_val is NULL </summary>
	public IntPtr GetVoidPtr(ImGuiID Key)
	{
		return ImGuiStorage_GetVoidPtr2350(_objectPtr, Key);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStorage_SetVoidPtr2351(IntPtr objectPtr, ImGuiID Key, IntPtr Val);

	/// <summary><code>IMGUI_API void      SetVoidPtr(ImGuiID key, void* val);</code>
		///    IMGUI_API void      SetVoidPtr(ImGuiID key, void* val); </summary>
	public void SetVoidPtr(ImGuiID Key, IntPtr Val)
	{
		ImGuiStorage_SetVoidPtr2351(_objectPtr, Key, Val);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern ref int ImGuiStorage_GetIntRef2357(IntPtr objectPtr, ImGuiID Key, int DefaultVal);

	/// <summary><code>IMGUI_API int*      GetIntRef(ImGuiID key, int default_val = 0);</code>
		///    IMGUI_API int*      GetIntRef(ImGuiID key, int default_val = 0); </summary>
	public ref int GetIntRef(ImGuiID Key)
	{
		return ref ImGuiStorage_GetIntRef2357(_objectPtr, Key, (int)0);
	}

	/// <summary><code>IMGUI_API int*      GetIntRef(ImGuiID key, int default_val = 0);</code>
		///    IMGUI_API int*      GetIntRef(ImGuiID key, int default_val = 0); </summary>
	public ref int GetIntRef(ImGuiID Key, int DefaultVal)
	{
		return ref ImGuiStorage_GetIntRef2357(_objectPtr, Key, DefaultVal);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern ref bool ImGuiStorage_GetBoolRef2358(IntPtr objectPtr, ImGuiID Key, [MarshalAs(UnmanagedType.I1)]bool DefaultVal);

	/// <summary><code>IMGUI_API bool*     GetBoolRef(ImGuiID key, bool default_val = false);</code>
		///    IMGUI_API bool*     GetBoolRef(ImGuiID key, bool default_val = false); </summary>
	public ref bool GetBoolRef(ImGuiID Key)
	{
		return ref ImGuiStorage_GetBoolRef2358(_objectPtr, Key, false);
	}

	/// <summary><code>IMGUI_API bool*     GetBoolRef(ImGuiID key, bool default_val = false);</code>
		///    IMGUI_API bool*     GetBoolRef(ImGuiID key, bool default_val = false); </summary>
	public ref bool GetBoolRef(ImGuiID Key, bool DefaultVal)
	{
		return ref ImGuiStorage_GetBoolRef2358(_objectPtr, Key, DefaultVal);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern ref float ImGuiStorage_GetFloatRef2359(IntPtr objectPtr, ImGuiID Key, float DefaultVal);

	/// <summary><code>IMGUI_API float*    GetFloatRef(ImGuiID key, float default_val = 0.0f);</code>
		///    IMGUI_API float*    GetFloatRef(ImGuiID key, float default_val = 0.0f); </summary>
	public ref float GetFloatRef(ImGuiID Key)
	{
		return ref ImGuiStorage_GetFloatRef2359(_objectPtr, Key, (float)0.0f);
	}

	/// <summary><code>IMGUI_API float*    GetFloatRef(ImGuiID key, float default_val = 0.0f);</code>
		///    IMGUI_API float*    GetFloatRef(ImGuiID key, float default_val = 0.0f); </summary>
	public ref float GetFloatRef(ImGuiID Key, float DefaultVal)
	{
		return ref ImGuiStorage_GetFloatRef2359(_objectPtr, Key, DefaultVal);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern IntPtr ImGuiStorage_GetVoidPtrRef2360(IntPtr objectPtr, ImGuiID Key, IntPtr DefaultVal);

	/// <summary><code>IMGUI_API void**    GetVoidPtrRef(ImGuiID key, void* default_val = NULL);</code>
		///    IMGUI_API void**    GetVoidPtrRef(ImGuiID key, void* default_val = NULL); </summary>
	public IntPtr GetVoidPtrRef(ImGuiID Key)
	{
		return ImGuiStorage_GetVoidPtrRef2360(_objectPtr, Key, default);
	}

	/// <summary><code>IMGUI_API void**    GetVoidPtrRef(ImGuiID key, void* default_val = NULL);</code>
		///    IMGUI_API void**    GetVoidPtrRef(ImGuiID key, void* default_val = NULL); </summary>
	public IntPtr GetVoidPtrRef(ImGuiID Key, IntPtr DefaultVal)
	{
		return ImGuiStorage_GetVoidPtrRef2360(_objectPtr, Key, DefaultVal);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStorage_SetAllInt2363(IntPtr objectPtr, int Val);

	/// <summary><code>IMGUI_API void      SetAllInt(int val);</code>
		///    IMGUI_API void      SetAllInt(int val); </summary>
	public void SetAllInt(int Val)
	{
		ImGuiStorage_SetAllInt2363(_objectPtr, Val);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiStorage_BuildSortByKey2366(IntPtr objectPtr);

	/// <summary><code>IMGUI_API void      BuildSortByKey();</code>
		///    IMGUI_API void      BuildSortByKey(); </summary>
	public void BuildSortByKey()
	{
		ImGuiStorage_BuildSortByKey2366(_objectPtr);
	}
	}
	public class ImGuiListClipper
	{
		private IntPtr _objectPtr;
		public IntPtr AsPtr { get => _objectPtr; }
		public ImGuiListClipper(IntPtr Ptr){ _objectPtr = Ptr; }

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern int ImGuiListClipper_Get_DisplayStart2392(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiListClipper_Set_DisplayStart2392(IntPtr objectPtr, int  Value);

	/// <summary><code>int             DisplayStart;       </code>
		/// First item to display, updated by each call to Step() </summary>
	public int DisplayStart
	{
		get { return ImGuiListClipper_Get_DisplayStart2392(_objectPtr);}
		set {ImGuiListClipper_Set_DisplayStart2392(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern int ImGuiListClipper_Get_DisplayEnd2393(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiListClipper_Set_DisplayEnd2393(IntPtr objectPtr, int  Value);

	/// <summary><code>int             DisplayEnd;         </code>
		/// End of items to display (exclusive) </summary>
	public int DisplayEnd
	{
		get { return ImGuiListClipper_Get_DisplayEnd2393(_objectPtr);}
		set {ImGuiListClipper_Set_DisplayEnd2393(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.LPArray)]
	private static extern ImGuiListClipper ImGuiListClipper_ImGuiListClipper2401();

	/// <summary><code>IMGUI_API ImGuiListClipper();</code>
		///    IMGUI_API ImGuiListClipper(); </summary>
	public  ImGuiListClipper()
	{
		_objectPtr = ImGuiListClipper_ImGuiListClipper2401()._objectPtr;
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiListClipper_DeleteImGuiListClipper2402(IntPtr objectPtr);

	/// <summary><code>IMGUI_API ~ImGuiListClipper();</code>
		///    IMGUI_API ~ImGuiListClipper(); </summary>
	 ~ImGuiListClipper()
	{
		ImGuiListClipper_DeleteImGuiListClipper2402(_objectPtr);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiListClipper_Begin2403(IntPtr objectPtr, int ItemsCount, float ItemsHeight);

	/// <summary><code>IMGUI_API void  Begin(int items_count, float items_height = -1.0f);</code>
		///    IMGUI_API void  Begin(int items_count, float items_height = -1.0f); </summary>
	public void Begin(int ItemsCount)
	{
		ImGuiListClipper_Begin2403(_objectPtr, ItemsCount, -1.0f);
	}

	/// <summary><code>IMGUI_API void  Begin(int items_count, float items_height = -1.0f);</code>
		///    IMGUI_API void  Begin(int items_count, float items_height = -1.0f); </summary>
	public void Begin(int ItemsCount, float ItemsHeight)
	{
		ImGuiListClipper_Begin2403(_objectPtr, ItemsCount, ItemsHeight);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiListClipper_End2404(IntPtr objectPtr);

	/// <summary><code>IMGUI_API void  End();             </code>
		/// Automatically called on the last call of Step() that returns false. </summary>
	public void End()
	{
		ImGuiListClipper_End2404(_objectPtr);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGuiListClipper_Step2405(IntPtr objectPtr);

	/// <summary><code>IMGUI_API bool  Step();            </code>
		/// Call until it returns false. The DisplayStart/DisplayEnd fields will be set and you can process/draw those items. </summary>
	public bool Step()
	{
		return ImGuiListClipper_Step2405(_objectPtr);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiListClipper_IncludeItemsByIndex2410(IntPtr objectPtr, int ItemBegin, int ItemEnd);

	/// <summary><code>IMGUI_API void  IncludeItemsByIndex(int item_begin, int item_end);  </code>
		/// item_end is exclusive e.g. use (42, 42+1) to make item 42 never clipped. </summary>
	public void IncludeItemsByIndex(int ItemBegin, int ItemEnd)
	{
		ImGuiListClipper_IncludeItemsByIndex2410(_objectPtr, ItemBegin, ItemEnd);
	}
	}
	public class ImColor
	{
		private IntPtr _objectPtr;
		public IntPtr AsPtr { get => _objectPtr; }
		public ImColor(IntPtr Ptr){ _objectPtr = Ptr; }

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern Vector4 ImColor_Get_Value2473(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImColor_Set_Value2473(IntPtr objectPtr, Vector4  Value);

	/// <summary><code>ImVec4          Value;</code>
		///    ImVec4          Value; </summary>
	public Vector4 Value
	{
		get { return ImColor_Get_Value2473(_objectPtr);}
		set {ImColor_Set_Value2473(_objectPtr, value);}
	}	}
	public class ImDrawCmd
	{
		private IntPtr _objectPtr;
		public IntPtr AsPtr { get => _objectPtr; }
		public ImDrawCmd(IntPtr Ptr){ _objectPtr = Ptr; }

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern Vector4 ImDrawCmd_Get_ClipRect2522(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawCmd_Set_ClipRect2522(IntPtr objectPtr, Vector4  Value);

	/// <summary><code>ImVec4          ClipRect;           </code>
		/// 4*4  // Clipping rectangle (x1, y1, x2, y2). Subtract ImDrawData->DisplayPos to get clipping rectangle in "viewport" coordinates </summary>
	public Vector4 ClipRect
	{
		get { return ImDrawCmd_Get_ClipRect2522(_objectPtr);}
		set {ImDrawCmd_Set_ClipRect2522(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern ImTextureID ImDrawCmd_Get_TextureId2523(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawCmd_Set_TextureId2523(IntPtr objectPtr, ImTextureID  Value);

	/// <summary><code>ImTextureID     TextureId;          </code>
		/// 4-8  // User-provided texture ID. Set by user in ImfontAtlas::SetTexID() for fonts or passed to Image*() functions. Ignore if never using images or multiple fonts atlas. </summary>
	public ImTextureID TextureId
	{
		get { return ImDrawCmd_Get_TextureId2523(_objectPtr);}
		set {ImDrawCmd_Set_TextureId2523(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern uint ImDrawCmd_Get_VtxOffset2524(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawCmd_Set_VtxOffset2524(IntPtr objectPtr, uint  Value);

	/// <summary><code>unsigned int    VtxOffset;          </code>
		/// 4    // Start offset in vertex buffer. ImGuiBackendFlags_RendererHasVtxOffset: always 0, otherwise may be >0 to support meshes larger than 64K vertices with 16-bit indices. </summary>
	public uint VtxOffset
	{
		get { return ImDrawCmd_Get_VtxOffset2524(_objectPtr);}
		set {ImDrawCmd_Set_VtxOffset2524(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern uint ImDrawCmd_Get_IdxOffset2525(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawCmd_Set_IdxOffset2525(IntPtr objectPtr, uint  Value);

	/// <summary><code>unsigned int    IdxOffset;          </code>
		/// 4    // Start offset in index buffer. </summary>
	public uint IdxOffset
	{
		get { return ImDrawCmd_Get_IdxOffset2525(_objectPtr);}
		set {ImDrawCmd_Set_IdxOffset2525(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern uint ImDrawCmd_Get_ElemCount2526(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawCmd_Set_ElemCount2526(IntPtr objectPtr, uint  Value);

	/// <summary><code>unsigned int    ElemCount;          </code>
		/// 4    // Number of indices (multiple of 3) to be rendered as triangles. Vertices are stored in the callee ImDrawList's vtx_buffer[] array, indices in idx_buffer[]. </summary>
	public uint ElemCount
	{
		get { return ImDrawCmd_Get_ElemCount2526(_objectPtr);}
		set {ImDrawCmd_Set_ElemCount2526(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern ImDrawCallback ImDrawCmd_Get_UserCallback2527(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawCmd_Set_UserCallback2527(IntPtr objectPtr, ImDrawCallback  Value);

	/// <summary><code>ImDrawCallback  UserCallback;       </code>
		/// 4-8  // If != NULL, call the function instead of rendering the vertices. clip_rect and texture_id will be set normally. </summary>
	public ImDrawCallback UserCallback
	{
		get { return ImDrawCmd_Get_UserCallback2527(_objectPtr);}
		set {ImDrawCmd_Set_UserCallback2527(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern IntPtr ImDrawCmd_Get_UserCallbackData2528(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawCmd_Set_UserCallbackData2528(IntPtr objectPtr, IntPtr  Value);

	/// <summary><code>void*           UserCallbackData;   </code>
		/// 4-8  // The draw callback code can access this. </summary>
	public IntPtr UserCallbackData
	{
		get { return ImDrawCmd_Get_UserCallbackData2528(_objectPtr);}
		set {ImDrawCmd_Set_UserCallbackData2528(_objectPtr, value);}
	}	}
	public class ImDrawVert
	{
		private IntPtr _objectPtr;
		public IntPtr AsPtr { get => _objectPtr; }
		public ImDrawVert(IntPtr Ptr){ _objectPtr = Ptr; }

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern Vector2 ImDrawVert_Get_pos2540(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawVert_Set_pos2540(IntPtr objectPtr, Vector2  Value);

	/// <summary><code>ImVec2  pos;</code>
		///    ImVec2  pos; </summary>
	public Vector2 pos
	{
		get { return ImDrawVert_Get_pos2540(_objectPtr);}
		set {ImDrawVert_Set_pos2540(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern Vector2 ImDrawVert_Get_uv2541(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawVert_Set_uv2541(IntPtr objectPtr, Vector2  Value);

	/// <summary><code>ImVec2  uv;</code>
		///    ImVec2  uv; </summary>
	public Vector2 uv
	{
		get { return ImDrawVert_Get_uv2541(_objectPtr);}
		set {ImDrawVert_Set_uv2541(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern uint ImDrawVert_Get_col2542(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawVert_Set_col2542(IntPtr objectPtr, uint  Value);

	/// <summary><code>ImU32   col;</code>
		///    ImU32   col; </summary>
	public uint col
	{
		get { return ImDrawVert_Get_col2542(_objectPtr);}
		set {ImDrawVert_Set_col2542(_objectPtr, value);}
	}	}
	public class ImDrawCmdHeader
	{
		private IntPtr _objectPtr;
		public IntPtr AsPtr { get => _objectPtr; }
		public ImDrawCmdHeader(IntPtr Ptr){ _objectPtr = Ptr; }

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern Vector4 ImDrawCmdHeader_Get_ClipRect2555(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawCmdHeader_Set_ClipRect2555(IntPtr objectPtr, Vector4  Value);

	/// <summary><code>ImVec4          ClipRect;</code>
		///    ImVec4          ClipRect; </summary>
	public Vector4 ClipRect
	{
		get { return ImDrawCmdHeader_Get_ClipRect2555(_objectPtr);}
		set {ImDrawCmdHeader_Set_ClipRect2555(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern ImTextureID ImDrawCmdHeader_Get_TextureId2556(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawCmdHeader_Set_TextureId2556(IntPtr objectPtr, ImTextureID  Value);

	/// <summary><code>ImTextureID     TextureId;</code>
		///    ImTextureID     TextureId; </summary>
	public ImTextureID TextureId
	{
		get { return ImDrawCmdHeader_Get_TextureId2556(_objectPtr);}
		set {ImDrawCmdHeader_Set_TextureId2556(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern uint ImDrawCmdHeader_Get_VtxOffset2557(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawCmdHeader_Set_VtxOffset2557(IntPtr objectPtr, uint  Value);

	/// <summary><code>unsigned int    VtxOffset;</code>
		///    unsigned int    VtxOffset; </summary>
	public uint VtxOffset
	{
		get { return ImDrawCmdHeader_Get_VtxOffset2557(_objectPtr);}
		set {ImDrawCmdHeader_Set_VtxOffset2557(_objectPtr, value);}
	}	}
	public class ImDrawChannel
	{
		private IntPtr _objectPtr;
		public IntPtr AsPtr { get => _objectPtr; }
		public ImDrawChannel(IntPtr Ptr){ _objectPtr = Ptr; }

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.LPArray)]
	private static extern List<ImDrawCmd> ImDrawChannel_Get__CmdBuffer2563(out int ReturnListSize, IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawChannel_Set__CmdBuffer2563(IntPtr objectPtr, [MarshalAs(UnmanagedType.LPArray)]List<ImDrawCmd>  Value);

	/// <summary><code>ImVector<ImDrawCmd>         _CmdBuffer;</code>
		///    ImVector<ImDrawCmd>         _CmdBuffer; </summary>
	public List<ImDrawCmd> _CmdBuffer
	{
		get { return ImDrawChannel_Get__CmdBuffer2563(out _, _objectPtr);}
		set {ImDrawChannel_Set__CmdBuffer2563(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.LPArray)]
	private static extern List<ImDrawIdx> ImDrawChannel_Get__IdxBuffer2564(out int ReturnListSize, IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawChannel_Set__IdxBuffer2564(IntPtr objectPtr, [MarshalAs(UnmanagedType.LPArray)]List<ImDrawIdx>  Value);

	/// <summary><code>ImVector<ImDrawIdx>         _IdxBuffer;</code>
		///    ImVector<ImDrawIdx>         _IdxBuffer; </summary>
	public List<ImDrawIdx> _IdxBuffer
	{
		get { return ImDrawChannel_Get__IdxBuffer2564(out _, _objectPtr);}
		set {ImDrawChannel_Set__IdxBuffer2564(_objectPtr, value);}
	}	}
	public class ImDrawListSplitter
	{
		private IntPtr _objectPtr;
		public IntPtr AsPtr { get => _objectPtr; }
		public ImDrawListSplitter(IntPtr Ptr){ _objectPtr = Ptr; }

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern int ImDrawListSplitter_Get__Current2572(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawListSplitter_Set__Current2572(IntPtr objectPtr, int  Value);

	/// <summary><code>int                         _Current;    </code>
		/// Current channel number (0) </summary>
	public int _Current
	{
		get { return ImDrawListSplitter_Get__Current2572(_objectPtr);}
		set {ImDrawListSplitter_Set__Current2572(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern int ImDrawListSplitter_Get__Count2573(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawListSplitter_Set__Count2573(IntPtr objectPtr, int  Value);

	/// <summary><code>int                         _Count;      </code>
		/// Number of active channels (1+) </summary>
	public int _Count
	{
		get { return ImDrawListSplitter_Get__Count2573(_objectPtr);}
		set {ImDrawListSplitter_Set__Count2573(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.LPArray)]
	private static extern List<ImDrawChannel> ImDrawListSplitter_Get__Channels2574(out int ReturnListSize, IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawListSplitter_Set__Channels2574(IntPtr objectPtr, [MarshalAs(UnmanagedType.LPArray)]List<ImDrawChannel>  Value);

	/// <summary><code>ImVector<ImDrawChannel>     _Channels;   </code>
		/// Draw channels (not resized down so _Count might be < Channels.Size) </summary>
	public List<ImDrawChannel> _Channels
	{
		get { return ImDrawListSplitter_Get__Channels2574(out _, _objectPtr);}
		set {ImDrawListSplitter_Set__Channels2574(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawListSplitter_ClearFreeMemory2579(IntPtr objectPtr);

	/// <summary><code>IMGUI_API void              ClearFreeMemory();</code>
		///    IMGUI_API void              ClearFreeMemory(); </summary>
	public void ClearFreeMemory()
	{
		ImDrawListSplitter_ClearFreeMemory2579(_objectPtr);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawListSplitter_Split2580(IntPtr objectPtr, IntPtr DrawList, int Count);

	/// <summary><code>IMGUI_API void              Split(ImDrawList* draw_list, int count);</code>
		///    IMGUI_API void              Split(ImDrawList* draw_list, int count); </summary>
	public void Split(ImDrawList DrawList, int Count)
	{
		ImDrawListSplitter_Split2580(_objectPtr, DrawList.AsPtr, Count);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawListSplitter_Merge2581(IntPtr objectPtr, IntPtr DrawList);

	/// <summary><code>IMGUI_API void              Merge(ImDrawList* draw_list);</code>
		///    IMGUI_API void              Merge(ImDrawList* draw_list); </summary>
	public void Merge(ImDrawList DrawList)
	{
		ImDrawListSplitter_Merge2581(_objectPtr, DrawList.AsPtr);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawListSplitter_SetCurrentChannel2582(IntPtr objectPtr, IntPtr DrawList, int ChannelIdx);

	/// <summary><code>IMGUI_API void              SetCurrentChannel(ImDrawList* draw_list, int channel_idx);</code>
		///    IMGUI_API void              SetCurrentChannel(ImDrawList* draw_list, int channel_idx); </summary>
	public void SetCurrentChannel(ImDrawList DrawList, int ChannelIdx)
	{
		ImDrawListSplitter_SetCurrentChannel2582(_objectPtr, DrawList.AsPtr, ChannelIdx);
	}
	}
	public class ImDrawList
	{
		private IntPtr _objectPtr;
		public IntPtr AsPtr { get => _objectPtr; }
		public ImDrawList(IntPtr Ptr){ _objectPtr = Ptr; }

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.LPArray)]
	private static extern List<ImDrawCmd> ImDrawList_Get_CmdBuffer2628(out int ReturnListSize, IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawList_Set_CmdBuffer2628(IntPtr objectPtr, [MarshalAs(UnmanagedType.LPArray)]List<ImDrawCmd>  Value);

	/// <summary><code>ImVector<ImDrawCmd>     CmdBuffer;          </code>
		/// Draw commands. Typically 1 command = 1 GPU draw call, unless the command is a callback. </summary>
	public List<ImDrawCmd> CmdBuffer
	{
		get { return ImDrawList_Get_CmdBuffer2628(out _, _objectPtr);}
		set {ImDrawList_Set_CmdBuffer2628(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.LPArray)]
	private static extern List<ImDrawIdx> ImDrawList_Get_IdxBuffer2629(out int ReturnListSize, IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawList_Set_IdxBuffer2629(IntPtr objectPtr, [MarshalAs(UnmanagedType.LPArray)]List<ImDrawIdx>  Value);

	/// <summary><code>ImVector<ImDrawIdx>     IdxBuffer;          </code>
		/// Index buffer. Each command consume ImDrawCmd::ElemCount of those </summary>
	public List<ImDrawIdx> IdxBuffer
	{
		get { return ImDrawList_Get_IdxBuffer2629(out _, _objectPtr);}
		set {ImDrawList_Set_IdxBuffer2629(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.LPArray)]
	private static extern List<ImDrawVert> ImDrawList_Get_VtxBuffer2630(out int ReturnListSize, IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawList_Set_VtxBuffer2630(IntPtr objectPtr, [MarshalAs(UnmanagedType.LPArray)]List<ImDrawVert>  Value);

	/// <summary><code>ImVector<ImDrawVert>    VtxBuffer;          </code>
		/// Vertex buffer. </summary>
	public List<ImDrawVert> VtxBuffer
	{
		get { return ImDrawList_Get_VtxBuffer2630(out _, _objectPtr);}
		set {ImDrawList_Set_VtxBuffer2630(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.LPArray)]
	private static extern ImDrawListFlags ImDrawList_Get_Flags2631(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawList_Set_Flags2631(IntPtr objectPtr, [MarshalAs(UnmanagedType.LPArray)]ImDrawListFlags  Value);

	/// <summary><code>ImDrawListFlags         Flags;              </code>
		/// Flags, you may poke into these to adjust anti-aliasing settings per-primitive. </summary>
	public ImDrawListFlags Flags
	{
		get { return ImDrawList_Get_Flags2631(_objectPtr);}
		set {ImDrawList_Set_Flags2631(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.LPStr)]
	private static extern string ImDrawList_Get__OwnerName2636(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawList_Set__OwnerName2636(IntPtr objectPtr, [MarshalAs(UnmanagedType.LPStr)]string  Value);

	/// <summary><code>const char*             _OwnerName;         </code>
		/// Pointer to owner window's name for debugging </summary>
	public string _OwnerName
	{
		get { return ImDrawList_Get__OwnerName2636(_objectPtr);}
		set {ImDrawList_Set__OwnerName2636(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawList_PushClipRect2650(IntPtr objectPtr, out  Vector2 ClipRectMin, out  Vector2 ClipRectMax, [MarshalAs(UnmanagedType.I1)]bool IntersectWithCurrentClipRect);

	/// <summary><code>IMGUI_API void  PushClipRect(const ImVec2& clip_rect_min, const ImVec2& clip_rect_max, bool intersect_with_current_clip_rect = false);  </code>
		/// Render-level scissoring. This is passed down to your render function but not used for CPU-side coarse clipping. Prefer using higher-level ImGui::PushClipRect() to affect logic (hit-testing and widget culling) </summary>
	public void PushClipRect(out  Vector2 ClipRectMin, out  Vector2 ClipRectMax)
	{
		ImDrawList_PushClipRect2650(_objectPtr, out ClipRectMin, out ClipRectMax, false);
	}

	/// <summary><code>IMGUI_API void  PushClipRect(const ImVec2& clip_rect_min, const ImVec2& clip_rect_max, bool intersect_with_current_clip_rect = false);  </code>
		/// Render-level scissoring. This is passed down to your render function but not used for CPU-side coarse clipping. Prefer using higher-level ImGui::PushClipRect() to affect logic (hit-testing and widget culling) </summary>
	public void PushClipRect(out  Vector2 ClipRectMin, out  Vector2 ClipRectMax, bool IntersectWithCurrentClipRect)
	{
		ImDrawList_PushClipRect2650(_objectPtr, out ClipRectMin, out ClipRectMax, IntersectWithCurrentClipRect);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawList_PushClipRectFullScreen2651(IntPtr objectPtr);

	/// <summary><code>IMGUI_API void  PushClipRectFullScreen();</code>
		///    IMGUI_API void  PushClipRectFullScreen(); </summary>
	public void PushClipRectFullScreen()
	{
		ImDrawList_PushClipRectFullScreen2651(_objectPtr);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawList_PopClipRect2652(IntPtr objectPtr);

	/// <summary><code>IMGUI_API void  PopClipRect();</code>
		///    IMGUI_API void  PopClipRect(); </summary>
	public void PopClipRect()
	{
		ImDrawList_PopClipRect2652(_objectPtr);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawList_PushTextureID2653(IntPtr objectPtr, ImTextureID TextureId);

	/// <summary><code>IMGUI_API void  PushTextureID(ImTextureID texture_id);</code>
		///    IMGUI_API void  PushTextureID(ImTextureID texture_id); </summary>
	public void PushTextureID(ImTextureID TextureId)
	{
		ImDrawList_PushTextureID2653(_objectPtr, TextureId);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawList_PopTextureID2654(IntPtr objectPtr);

	/// <summary><code>IMGUI_API void  PopTextureID();</code>
		///    IMGUI_API void  PopTextureID(); </summary>
	public void PopTextureID()
	{
		ImDrawList_PopTextureID2654(_objectPtr);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawList_AddLine2665(IntPtr objectPtr, out  Vector2 P1, out  Vector2 P2, uint Col, float Thickness);

	/// <summary><code>IMGUI_API void  AddLine(const ImVec2& p1, const ImVec2& p2, ImU32 col, float thickness = 1.0f);</code>
		///    IMGUI_API void  AddLine(const ImVec2& p1, const ImVec2& p2, ImU32 col, float thickness = 1.0f); </summary>
	public void AddLine(out  Vector2 P1, out  Vector2 P2, uint Col)
	{
		ImDrawList_AddLine2665(_objectPtr, out P1, out P2, Col, (float)1.0f);
	}

	/// <summary><code>IMGUI_API void  AddLine(const ImVec2& p1, const ImVec2& p2, ImU32 col, float thickness = 1.0f);</code>
		///    IMGUI_API void  AddLine(const ImVec2& p1, const ImVec2& p2, ImU32 col, float thickness = 1.0f); </summary>
	public void AddLine(out  Vector2 P1, out  Vector2 P2, uint Col, float Thickness)
	{
		ImDrawList_AddLine2665(_objectPtr, out P1, out P2, Col, Thickness);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawList_AddRect2666(IntPtr objectPtr, out  Vector2 PMin, out  Vector2 PMax, uint Col, float Rounding, ImDrawFlags Flags, float Thickness);

	/// <summary><code>IMGUI_API void  AddRect(const ImVec2& p_min, const ImVec2& p_max, ImU32 col, float rounding = 0.0f, ImDrawFlags flags = 0, float thickness = 1.0f);   </code>
		/// a: upper-left, b: lower-right (== upper-left + size) </summary>
	public void AddRect(out  Vector2 PMin, out  Vector2 PMax, uint Col)
	{
		ImDrawList_AddRect2666(_objectPtr, out PMin, out PMax, Col, (float)0.0f, (ImDrawFlags)0, (float)1.0f);
	}

	/// <summary><code>IMGUI_API void  AddRect(const ImVec2& p_min, const ImVec2& p_max, ImU32 col, float rounding = 0.0f, ImDrawFlags flags = 0, float thickness = 1.0f);   </code>
		/// a: upper-left, b: lower-right (== upper-left + size) </summary>
	public void AddRect(out  Vector2 PMin, out  Vector2 PMax, uint Col, float Rounding)
	{
		ImDrawList_AddRect2666(_objectPtr, out PMin, out PMax, Col, Rounding, (ImDrawFlags)0, (float)1.0f);
	}

	/// <summary><code>IMGUI_API void  AddRect(const ImVec2& p_min, const ImVec2& p_max, ImU32 col, float rounding = 0.0f, ImDrawFlags flags = 0, float thickness = 1.0f);   </code>
		/// a: upper-left, b: lower-right (== upper-left + size) </summary>
	public void AddRect(out  Vector2 PMin, out  Vector2 PMax, uint Col, float Rounding, ImDrawFlags Flags)
	{
		ImDrawList_AddRect2666(_objectPtr, out PMin, out PMax, Col, Rounding, Flags, (float)1.0f);
	}

	/// <summary><code>IMGUI_API void  AddRect(const ImVec2& p_min, const ImVec2& p_max, ImU32 col, float rounding = 0.0f, ImDrawFlags flags = 0, float thickness = 1.0f);   </code>
		/// a: upper-left, b: lower-right (== upper-left + size) </summary>
	public void AddRect(out  Vector2 PMin, out  Vector2 PMax, uint Col, float Rounding, ImDrawFlags Flags, float Thickness)
	{
		ImDrawList_AddRect2666(_objectPtr, out PMin, out PMax, Col, Rounding, Flags, Thickness);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawList_AddRectFilled2667(IntPtr objectPtr, out  Vector2 PMin, out  Vector2 PMax, uint Col, float Rounding, ImDrawFlags Flags);

	/// <summary><code>IMGUI_API void  AddRectFilled(const ImVec2& p_min, const ImVec2& p_max, ImU32 col, float rounding = 0.0f, ImDrawFlags flags = 0);                     </code>
		/// a: upper-left, b: lower-right (== upper-left + size) </summary>
	public void AddRectFilled(out  Vector2 PMin, out  Vector2 PMax, uint Col)
	{
		ImDrawList_AddRectFilled2667(_objectPtr, out PMin, out PMax, Col, (float)0.0f, (ImDrawFlags)0);
	}

	/// <summary><code>IMGUI_API void  AddRectFilled(const ImVec2& p_min, const ImVec2& p_max, ImU32 col, float rounding = 0.0f, ImDrawFlags flags = 0);                     </code>
		/// a: upper-left, b: lower-right (== upper-left + size) </summary>
	public void AddRectFilled(out  Vector2 PMin, out  Vector2 PMax, uint Col, float Rounding)
	{
		ImDrawList_AddRectFilled2667(_objectPtr, out PMin, out PMax, Col, Rounding, (ImDrawFlags)0);
	}

	/// <summary><code>IMGUI_API void  AddRectFilled(const ImVec2& p_min, const ImVec2& p_max, ImU32 col, float rounding = 0.0f, ImDrawFlags flags = 0);                     </code>
		/// a: upper-left, b: lower-right (== upper-left + size) </summary>
	public void AddRectFilled(out  Vector2 PMin, out  Vector2 PMax, uint Col, float Rounding, ImDrawFlags Flags)
	{
		ImDrawList_AddRectFilled2667(_objectPtr, out PMin, out PMax, Col, Rounding, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawList_AddRectFilledMultiColor2668(IntPtr objectPtr, out  Vector2 PMin, out  Vector2 PMax, uint ColUprLeft, uint ColUprRight, uint ColBotRight, uint ColBotLeft);

	/// <summary><code>IMGUI_API void  AddRectFilledMultiColor(const ImVec2& p_min, const ImVec2& p_max, ImU32 col_upr_left, ImU32 col_upr_right, ImU32 col_bot_right, ImU32 col_bot_left);</code>
		///    IMGUI_API void  AddRectFilledMultiColor(const ImVec2& p_min, const ImVec2& p_max, ImU32 col_upr_left, ImU32 col_upr_right, ImU32 col_bot_right, ImU32 col_bot_left); </summary>
	public void AddRectFilledMultiColor(out  Vector2 PMin, out  Vector2 PMax, uint ColUprLeft, uint ColUprRight, uint ColBotRight, uint ColBotLeft)
	{
		ImDrawList_AddRectFilledMultiColor2668(_objectPtr, out PMin, out PMax, ColUprLeft, ColUprRight, ColBotRight, ColBotLeft);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawList_AddQuad2669(IntPtr objectPtr, out  Vector2 P1, out  Vector2 P2, out  Vector2 P3, out  Vector2 P4, uint Col, float Thickness);

	/// <summary><code>IMGUI_API void  AddQuad(const ImVec2& p1, const ImVec2& p2, const ImVec2& p3, const ImVec2& p4, ImU32 col, float thickness = 1.0f);</code>
		///    IMGUI_API void  AddQuad(const ImVec2& p1, const ImVec2& p2, const ImVec2& p3, const ImVec2& p4, ImU32 col, float thickness = 1.0f); </summary>
	public void AddQuad(out  Vector2 P1, out  Vector2 P2, out  Vector2 P3, out  Vector2 P4, uint Col)
	{
		ImDrawList_AddQuad2669(_objectPtr, out P1, out P2, out P3, out P4, Col, (float)1.0f);
	}

	/// <summary><code>IMGUI_API void  AddQuad(const ImVec2& p1, const ImVec2& p2, const ImVec2& p3, const ImVec2& p4, ImU32 col, float thickness = 1.0f);</code>
		///    IMGUI_API void  AddQuad(const ImVec2& p1, const ImVec2& p2, const ImVec2& p3, const ImVec2& p4, ImU32 col, float thickness = 1.0f); </summary>
	public void AddQuad(out  Vector2 P1, out  Vector2 P2, out  Vector2 P3, out  Vector2 P4, uint Col, float Thickness)
	{
		ImDrawList_AddQuad2669(_objectPtr, out P1, out P2, out P3, out P4, Col, Thickness);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawList_AddQuadFilled2670(IntPtr objectPtr, out  Vector2 P1, out  Vector2 P2, out  Vector2 P3, out  Vector2 P4, uint Col);

	/// <summary><code>IMGUI_API void  AddQuadFilled(const ImVec2& p1, const ImVec2& p2, const ImVec2& p3, const ImVec2& p4, ImU32 col);</code>
		///    IMGUI_API void  AddQuadFilled(const ImVec2& p1, const ImVec2& p2, const ImVec2& p3, const ImVec2& p4, ImU32 col); </summary>
	public void AddQuadFilled(out  Vector2 P1, out  Vector2 P2, out  Vector2 P3, out  Vector2 P4, uint Col)
	{
		ImDrawList_AddQuadFilled2670(_objectPtr, out P1, out P2, out P3, out P4, Col);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawList_AddTriangle2671(IntPtr objectPtr, out  Vector2 P1, out  Vector2 P2, out  Vector2 P3, uint Col, float Thickness);

	/// <summary><code>IMGUI_API void  AddTriangle(const ImVec2& p1, const ImVec2& p2, const ImVec2& p3, ImU32 col, float thickness = 1.0f);</code>
		///    IMGUI_API void  AddTriangle(const ImVec2& p1, const ImVec2& p2, const ImVec2& p3, ImU32 col, float thickness = 1.0f); </summary>
	public void AddTriangle(out  Vector2 P1, out  Vector2 P2, out  Vector2 P3, uint Col)
	{
		ImDrawList_AddTriangle2671(_objectPtr, out P1, out P2, out P3, Col, (float)1.0f);
	}

	/// <summary><code>IMGUI_API void  AddTriangle(const ImVec2& p1, const ImVec2& p2, const ImVec2& p3, ImU32 col, float thickness = 1.0f);</code>
		///    IMGUI_API void  AddTriangle(const ImVec2& p1, const ImVec2& p2, const ImVec2& p3, ImU32 col, float thickness = 1.0f); </summary>
	public void AddTriangle(out  Vector2 P1, out  Vector2 P2, out  Vector2 P3, uint Col, float Thickness)
	{
		ImDrawList_AddTriangle2671(_objectPtr, out P1, out P2, out P3, Col, Thickness);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawList_AddTriangleFilled2672(IntPtr objectPtr, out  Vector2 P1, out  Vector2 P2, out  Vector2 P3, uint Col);

	/// <summary><code>IMGUI_API void  AddTriangleFilled(const ImVec2& p1, const ImVec2& p2, const ImVec2& p3, ImU32 col);</code>
		///    IMGUI_API void  AddTriangleFilled(const ImVec2& p1, const ImVec2& p2, const ImVec2& p3, ImU32 col); </summary>
	public void AddTriangleFilled(out  Vector2 P1, out  Vector2 P2, out  Vector2 P3, uint Col)
	{
		ImDrawList_AddTriangleFilled2672(_objectPtr, out P1, out P2, out P3, Col);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawList_AddCircle2673(IntPtr objectPtr, out  Vector2 Center, float Radius, uint Col, int NumSegments, float Thickness);

	/// <summary><code>IMGUI_API void  AddCircle(const ImVec2& center, float radius, ImU32 col, int num_segments = 0, float thickness = 1.0f);</code>
		///    IMGUI_API void  AddCircle(const ImVec2& center, float radius, ImU32 col, int num_segments = 0, float thickness = 1.0f); </summary>
	public void AddCircle(out  Vector2 Center, float Radius, uint Col)
	{
		ImDrawList_AddCircle2673(_objectPtr, out Center, Radius, Col, (int)0, (float)1.0f);
	}

	/// <summary><code>IMGUI_API void  AddCircle(const ImVec2& center, float radius, ImU32 col, int num_segments = 0, float thickness = 1.0f);</code>
		///    IMGUI_API void  AddCircle(const ImVec2& center, float radius, ImU32 col, int num_segments = 0, float thickness = 1.0f); </summary>
	public void AddCircle(out  Vector2 Center, float Radius, uint Col, int NumSegments)
	{
		ImDrawList_AddCircle2673(_objectPtr, out Center, Radius, Col, NumSegments, (float)1.0f);
	}

	/// <summary><code>IMGUI_API void  AddCircle(const ImVec2& center, float radius, ImU32 col, int num_segments = 0, float thickness = 1.0f);</code>
		///    IMGUI_API void  AddCircle(const ImVec2& center, float radius, ImU32 col, int num_segments = 0, float thickness = 1.0f); </summary>
	public void AddCircle(out  Vector2 Center, float Radius, uint Col, int NumSegments, float Thickness)
	{
		ImDrawList_AddCircle2673(_objectPtr, out Center, Radius, Col, NumSegments, Thickness);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawList_AddCircleFilled2674(IntPtr objectPtr, out  Vector2 Center, float Radius, uint Col, int NumSegments);

	/// <summary><code>IMGUI_API void  AddCircleFilled(const ImVec2& center, float radius, ImU32 col, int num_segments = 0);</code>
		///    IMGUI_API void  AddCircleFilled(const ImVec2& center, float radius, ImU32 col, int num_segments = 0); </summary>
	public void AddCircleFilled(out  Vector2 Center, float Radius, uint Col)
	{
		ImDrawList_AddCircleFilled2674(_objectPtr, out Center, Radius, Col, (int)0);
	}

	/// <summary><code>IMGUI_API void  AddCircleFilled(const ImVec2& center, float radius, ImU32 col, int num_segments = 0);</code>
		///    IMGUI_API void  AddCircleFilled(const ImVec2& center, float radius, ImU32 col, int num_segments = 0); </summary>
	public void AddCircleFilled(out  Vector2 Center, float Radius, uint Col, int NumSegments)
	{
		ImDrawList_AddCircleFilled2674(_objectPtr, out Center, Radius, Col, NumSegments);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawList_AddNgon2675(IntPtr objectPtr, out  Vector2 Center, float Radius, uint Col, int NumSegments, float Thickness);

	/// <summary><code>IMGUI_API void  AddNgon(const ImVec2& center, float radius, ImU32 col, int num_segments, float thickness = 1.0f);</code>
		///    IMGUI_API void  AddNgon(const ImVec2& center, float radius, ImU32 col, int num_segments, float thickness = 1.0f); </summary>
	public void AddNgon(out  Vector2 Center, float Radius, uint Col, int NumSegments)
	{
		ImDrawList_AddNgon2675(_objectPtr, out Center, Radius, Col, NumSegments, (float)1.0f);
	}

	/// <summary><code>IMGUI_API void  AddNgon(const ImVec2& center, float radius, ImU32 col, int num_segments, float thickness = 1.0f);</code>
		///    IMGUI_API void  AddNgon(const ImVec2& center, float radius, ImU32 col, int num_segments, float thickness = 1.0f); </summary>
	public void AddNgon(out  Vector2 Center, float Radius, uint Col, int NumSegments, float Thickness)
	{
		ImDrawList_AddNgon2675(_objectPtr, out Center, Radius, Col, NumSegments, Thickness);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawList_AddNgonFilled2676(IntPtr objectPtr, out  Vector2 Center, float Radius, uint Col, int NumSegments);

	/// <summary><code>IMGUI_API void  AddNgonFilled(const ImVec2& center, float radius, ImU32 col, int num_segments);</code>
		///    IMGUI_API void  AddNgonFilled(const ImVec2& center, float radius, ImU32 col, int num_segments); </summary>
	public void AddNgonFilled(out  Vector2 Center, float Radius, uint Col, int NumSegments)
	{
		ImDrawList_AddNgonFilled2676(_objectPtr, out Center, Radius, Col, NumSegments);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawList_AddText2677(IntPtr objectPtr, out  Vector2 Pos, uint Col, [MarshalAs(UnmanagedType.LPStr)]string TextBegin, [MarshalAs(UnmanagedType.LPStr)]string TextEnd);

	/// <summary><code>IMGUI_API void  AddText(const ImVec2& pos, ImU32 col, const char* text_begin, const char* text_end = NULL);</code>
		///    IMGUI_API void  AddText(const ImVec2& pos, ImU32 col, const char* text_begin, const char* text_end = NULL); </summary>
	public void AddText(out  Vector2 Pos, uint Col, string TextBegin)
	{
		ImDrawList_AddText2677(_objectPtr, out Pos, Col, TextBegin, default);
	}

	/// <summary><code>IMGUI_API void  AddText(const ImVec2& pos, ImU32 col, const char* text_begin, const char* text_end = NULL);</code>
		///    IMGUI_API void  AddText(const ImVec2& pos, ImU32 col, const char* text_begin, const char* text_end = NULL); </summary>
	public void AddText(out  Vector2 Pos, uint Col, string TextBegin, string TextEnd)
	{
		ImDrawList_AddText2677(_objectPtr, out Pos, Col, TextBegin, TextEnd);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawList_AddPolyline2679(IntPtr objectPtr, out  Vector2 Points, int NumPoints, uint Col, ImDrawFlags Flags, float Thickness);

	/// <summary><code>IMGUI_API void  AddPolyline(const ImVec2* points, int num_points, ImU32 col, ImDrawFlags flags, float thickness);</code>
		///    IMGUI_API void  AddPolyline(const ImVec2* points, int num_points, ImU32 col, ImDrawFlags flags, float thickness); </summary>
	public void AddPolyline(out  Vector2 Points, int NumPoints, uint Col, ImDrawFlags Flags, float Thickness)
	{
		ImDrawList_AddPolyline2679(_objectPtr, out Points, NumPoints, Col, Flags, Thickness);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawList_AddConvexPolyFilled2680(IntPtr objectPtr, out  Vector2 Points, int NumPoints, uint Col);

	/// <summary><code>IMGUI_API void  AddConvexPolyFilled(const ImVec2* points, int num_points, ImU32 col);</code>
		///    IMGUI_API void  AddConvexPolyFilled(const ImVec2* points, int num_points, ImU32 col); </summary>
	public void AddConvexPolyFilled(out  Vector2 Points, int NumPoints, uint Col)
	{
		ImDrawList_AddConvexPolyFilled2680(_objectPtr, out Points, NumPoints, Col);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawList_AddBezierCubic2681(IntPtr objectPtr, out  Vector2 P1, out  Vector2 P2, out  Vector2 P3, out  Vector2 P4, uint Col, float Thickness, int NumSegments);

	/// <summary><code>IMGUI_API void  AddBezierCubic(const ImVec2& p1, const ImVec2& p2, const ImVec2& p3, const ImVec2& p4, ImU32 col, float thickness, int num_segments = 0); </code>
		/// Cubic Bezier (4 control points) </summary>
	public void AddBezierCubic(out  Vector2 P1, out  Vector2 P2, out  Vector2 P3, out  Vector2 P4, uint Col, float Thickness)
	{
		ImDrawList_AddBezierCubic2681(_objectPtr, out P1, out P2, out P3, out P4, Col, Thickness, (int)0);
	}

	/// <summary><code>IMGUI_API void  AddBezierCubic(const ImVec2& p1, const ImVec2& p2, const ImVec2& p3, const ImVec2& p4, ImU32 col, float thickness, int num_segments = 0); </code>
		/// Cubic Bezier (4 control points) </summary>
	public void AddBezierCubic(out  Vector2 P1, out  Vector2 P2, out  Vector2 P3, out  Vector2 P4, uint Col, float Thickness, int NumSegments)
	{
		ImDrawList_AddBezierCubic2681(_objectPtr, out P1, out P2, out P3, out P4, Col, Thickness, NumSegments);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawList_AddBezierQuadratic2682(IntPtr objectPtr, out  Vector2 P1, out  Vector2 P2, out  Vector2 P3, uint Col, float Thickness, int NumSegments);

	/// <summary><code>IMGUI_API void  AddBezierQuadratic(const ImVec2& p1, const ImVec2& p2, const ImVec2& p3, ImU32 col, float thickness, int num_segments = 0);               </code>
		/// Quadratic Bezier (3 control points) </summary>
	public void AddBezierQuadratic(out  Vector2 P1, out  Vector2 P2, out  Vector2 P3, uint Col, float Thickness)
	{
		ImDrawList_AddBezierQuadratic2682(_objectPtr, out P1, out P2, out P3, Col, Thickness, (int)0);
	}

	/// <summary><code>IMGUI_API void  AddBezierQuadratic(const ImVec2& p1, const ImVec2& p2, const ImVec2& p3, ImU32 col, float thickness, int num_segments = 0);               </code>
		/// Quadratic Bezier (3 control points) </summary>
	public void AddBezierQuadratic(out  Vector2 P1, out  Vector2 P2, out  Vector2 P3, uint Col, float Thickness, int NumSegments)
	{
		ImDrawList_AddBezierQuadratic2682(_objectPtr, out P1, out P2, out P3, Col, Thickness, NumSegments);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawList_AddImage2688(IntPtr objectPtr, ImTextureID UserTextureId, out  Vector2 PMin, out  Vector2 PMax, out  Vector2 UvMin, out  Vector2 UvMax, uint Col);

	/// <summary><code>IMGUI_API void  AddImage(ImTextureID user_texture_id, const ImVec2& p_min, const ImVec2& p_max, const ImVec2& uv_min = ImVec2(0, 0), const ImVec2& uv_max = ImVec2(1, 1), ImU32 col = IM_COL32_WHITE);</code>
		///    IMGUI_API void  AddImage(ImTextureID user_texture_id, const ImVec2& p_min, const ImVec2& p_max, const ImVec2& uv_min = ImVec2(0, 0), const ImVec2& uv_max = ImVec2(1, 1), ImU32 col = IM_COL32_WHITE); </summary>
	public void AddImage(ImTextureID UserTextureId, out  Vector2 PMin, out  Vector2 PMax)
	{
		 Vector2 param3 = new  Vector2 (0,  0);
		 Vector2 param4 = new  Vector2 (1,  1);
		ImDrawList_AddImage2688(_objectPtr, UserTextureId, out PMin, out PMax, out param3, out param4, 0xFFFFFFFF);
	}

	/// <summary><code>IMGUI_API void  AddImage(ImTextureID user_texture_id, const ImVec2& p_min, const ImVec2& p_max, const ImVec2& uv_min = ImVec2(0, 0), const ImVec2& uv_max = ImVec2(1, 1), ImU32 col = IM_COL32_WHITE);</code>
		///    IMGUI_API void  AddImage(ImTextureID user_texture_id, const ImVec2& p_min, const ImVec2& p_max, const ImVec2& uv_min = ImVec2(0, 0), const ImVec2& uv_max = ImVec2(1, 1), ImU32 col = IM_COL32_WHITE); </summary>
	public void AddImage(ImTextureID UserTextureId, out  Vector2 PMin, out  Vector2 PMax, out  Vector2 UvMin)
	{
		 Vector2 param4 = new  Vector2 (1,  1);
		ImDrawList_AddImage2688(_objectPtr, UserTextureId, out PMin, out PMax, out UvMin, out param4, 0xFFFFFFFF);
	}

	/// <summary><code>IMGUI_API void  AddImage(ImTextureID user_texture_id, const ImVec2& p_min, const ImVec2& p_max, const ImVec2& uv_min = ImVec2(0, 0), const ImVec2& uv_max = ImVec2(1, 1), ImU32 col = IM_COL32_WHITE);</code>
		///    IMGUI_API void  AddImage(ImTextureID user_texture_id, const ImVec2& p_min, const ImVec2& p_max, const ImVec2& uv_min = ImVec2(0, 0), const ImVec2& uv_max = ImVec2(1, 1), ImU32 col = IM_COL32_WHITE); </summary>
	public void AddImage(ImTextureID UserTextureId, out  Vector2 PMin, out  Vector2 PMax, out  Vector2 UvMin, out  Vector2 UvMax)
	{
		ImDrawList_AddImage2688(_objectPtr, UserTextureId, out PMin, out PMax, out UvMin, out UvMax, 0xFFFFFFFF);
	}

	/// <summary><code>IMGUI_API void  AddImage(ImTextureID user_texture_id, const ImVec2& p_min, const ImVec2& p_max, const ImVec2& uv_min = ImVec2(0, 0), const ImVec2& uv_max = ImVec2(1, 1), ImU32 col = IM_COL32_WHITE);</code>
		///    IMGUI_API void  AddImage(ImTextureID user_texture_id, const ImVec2& p_min, const ImVec2& p_max, const ImVec2& uv_min = ImVec2(0, 0), const ImVec2& uv_max = ImVec2(1, 1), ImU32 col = IM_COL32_WHITE); </summary>
	public void AddImage(ImTextureID UserTextureId, out  Vector2 PMin, out  Vector2 PMax, out  Vector2 UvMin, out  Vector2 UvMax, uint Col)
	{
		ImDrawList_AddImage2688(_objectPtr, UserTextureId, out PMin, out PMax, out UvMin, out UvMax, Col);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawList_AddImageQuad2689(IntPtr objectPtr, ImTextureID UserTextureId, out  Vector2 P1, out  Vector2 P2, out  Vector2 P3, out  Vector2 P4, out  Vector2 Uv1, out  Vector2 Uv2, out  Vector2 Uv3, out  Vector2 Uv4, uint Col);

	/// <summary><code>IMGUI_API void  AddImageQuad(ImTextureID user_texture_id, const ImVec2& p1, const ImVec2& p2, const ImVec2& p3, const ImVec2& p4, const ImVec2& uv1 = ImVec2(0, 0), const ImVec2& uv2 = ImVec2(1, 0), const ImVec2& uv3 = ImVec2(1, 1), const ImVec2& uv4 = ImVec2(0, 1), ImU32 col = IM_COL32_WHITE);</code>
		///    IMGUI_API void  AddImageQuad(ImTextureID user_texture_id, const ImVec2& p1, const ImVec2& p2, const ImVec2& p3, const ImVec2& p4, const ImVec2& uv1 = ImVec2(0, 0), const ImVec2& uv2 = ImVec2(1, 0), const ImVec2& uv3 = ImVec2(1, 1), const ImVec2& uv4 = ImVec2(0, 1), ImU32 col = IM_COL32_WHITE); </summary>
	public void AddImageQuad(ImTextureID UserTextureId, out  Vector2 P1, out  Vector2 P2, out  Vector2 P3, out  Vector2 P4)
	{
		 Vector2 param5 = new  Vector2 (0,  0);
		 Vector2 param6 = new  Vector2 (1,  0);
		 Vector2 param7 = new  Vector2 (1,  1);
		 Vector2 param8 = new  Vector2 (0,  1);
		ImDrawList_AddImageQuad2689(_objectPtr, UserTextureId, out P1, out P2, out P3, out P4, out param5, out param6, out param7, out param8, 0xFFFFFFFF);
	}

	/// <summary><code>IMGUI_API void  AddImageQuad(ImTextureID user_texture_id, const ImVec2& p1, const ImVec2& p2, const ImVec2& p3, const ImVec2& p4, const ImVec2& uv1 = ImVec2(0, 0), const ImVec2& uv2 = ImVec2(1, 0), const ImVec2& uv3 = ImVec2(1, 1), const ImVec2& uv4 = ImVec2(0, 1), ImU32 col = IM_COL32_WHITE);</code>
		///    IMGUI_API void  AddImageQuad(ImTextureID user_texture_id, const ImVec2& p1, const ImVec2& p2, const ImVec2& p3, const ImVec2& p4, const ImVec2& uv1 = ImVec2(0, 0), const ImVec2& uv2 = ImVec2(1, 0), const ImVec2& uv3 = ImVec2(1, 1), const ImVec2& uv4 = ImVec2(0, 1), ImU32 col = IM_COL32_WHITE); </summary>
	public void AddImageQuad(ImTextureID UserTextureId, out  Vector2 P1, out  Vector2 P2, out  Vector2 P3, out  Vector2 P4, out  Vector2 Uv1)
	{
		 Vector2 param6 = new  Vector2 (1,  0);
		 Vector2 param7 = new  Vector2 (1,  1);
		 Vector2 param8 = new  Vector2 (0,  1);
		ImDrawList_AddImageQuad2689(_objectPtr, UserTextureId, out P1, out P2, out P3, out P4, out Uv1, out param6, out param7, out param8, 0xFFFFFFFF);
	}

	/// <summary><code>IMGUI_API void  AddImageQuad(ImTextureID user_texture_id, const ImVec2& p1, const ImVec2& p2, const ImVec2& p3, const ImVec2& p4, const ImVec2& uv1 = ImVec2(0, 0), const ImVec2& uv2 = ImVec2(1, 0), const ImVec2& uv3 = ImVec2(1, 1), const ImVec2& uv4 = ImVec2(0, 1), ImU32 col = IM_COL32_WHITE);</code>
		///    IMGUI_API void  AddImageQuad(ImTextureID user_texture_id, const ImVec2& p1, const ImVec2& p2, const ImVec2& p3, const ImVec2& p4, const ImVec2& uv1 = ImVec2(0, 0), const ImVec2& uv2 = ImVec2(1, 0), const ImVec2& uv3 = ImVec2(1, 1), const ImVec2& uv4 = ImVec2(0, 1), ImU32 col = IM_COL32_WHITE); </summary>
	public void AddImageQuad(ImTextureID UserTextureId, out  Vector2 P1, out  Vector2 P2, out  Vector2 P3, out  Vector2 P4, out  Vector2 Uv1, out  Vector2 Uv2)
	{
		 Vector2 param7 = new  Vector2 (1,  1);
		 Vector2 param8 = new  Vector2 (0,  1);
		ImDrawList_AddImageQuad2689(_objectPtr, UserTextureId, out P1, out P2, out P3, out P4, out Uv1, out Uv2, out param7, out param8, 0xFFFFFFFF);
	}

	/// <summary><code>IMGUI_API void  AddImageQuad(ImTextureID user_texture_id, const ImVec2& p1, const ImVec2& p2, const ImVec2& p3, const ImVec2& p4, const ImVec2& uv1 = ImVec2(0, 0), const ImVec2& uv2 = ImVec2(1, 0), const ImVec2& uv3 = ImVec2(1, 1), const ImVec2& uv4 = ImVec2(0, 1), ImU32 col = IM_COL32_WHITE);</code>
		///    IMGUI_API void  AddImageQuad(ImTextureID user_texture_id, const ImVec2& p1, const ImVec2& p2, const ImVec2& p3, const ImVec2& p4, const ImVec2& uv1 = ImVec2(0, 0), const ImVec2& uv2 = ImVec2(1, 0), const ImVec2& uv3 = ImVec2(1, 1), const ImVec2& uv4 = ImVec2(0, 1), ImU32 col = IM_COL32_WHITE); </summary>
	public void AddImageQuad(ImTextureID UserTextureId, out  Vector2 P1, out  Vector2 P2, out  Vector2 P3, out  Vector2 P4, out  Vector2 Uv1, out  Vector2 Uv2, out  Vector2 Uv3)
	{
		 Vector2 param8 = new  Vector2 (0,  1);
		ImDrawList_AddImageQuad2689(_objectPtr, UserTextureId, out P1, out P2, out P3, out P4, out Uv1, out Uv2, out Uv3, out param8, 0xFFFFFFFF);
	}

	/// <summary><code>IMGUI_API void  AddImageQuad(ImTextureID user_texture_id, const ImVec2& p1, const ImVec2& p2, const ImVec2& p3, const ImVec2& p4, const ImVec2& uv1 = ImVec2(0, 0), const ImVec2& uv2 = ImVec2(1, 0), const ImVec2& uv3 = ImVec2(1, 1), const ImVec2& uv4 = ImVec2(0, 1), ImU32 col = IM_COL32_WHITE);</code>
		///    IMGUI_API void  AddImageQuad(ImTextureID user_texture_id, const ImVec2& p1, const ImVec2& p2, const ImVec2& p3, const ImVec2& p4, const ImVec2& uv1 = ImVec2(0, 0), const ImVec2& uv2 = ImVec2(1, 0), const ImVec2& uv3 = ImVec2(1, 1), const ImVec2& uv4 = ImVec2(0, 1), ImU32 col = IM_COL32_WHITE); </summary>
	public void AddImageQuad(ImTextureID UserTextureId, out  Vector2 P1, out  Vector2 P2, out  Vector2 P3, out  Vector2 P4, out  Vector2 Uv1, out  Vector2 Uv2, out  Vector2 Uv3, out  Vector2 Uv4)
	{
		ImDrawList_AddImageQuad2689(_objectPtr, UserTextureId, out P1, out P2, out P3, out P4, out Uv1, out Uv2, out Uv3, out Uv4, 0xFFFFFFFF);
	}

	/// <summary><code>IMGUI_API void  AddImageQuad(ImTextureID user_texture_id, const ImVec2& p1, const ImVec2& p2, const ImVec2& p3, const ImVec2& p4, const ImVec2& uv1 = ImVec2(0, 0), const ImVec2& uv2 = ImVec2(1, 0), const ImVec2& uv3 = ImVec2(1, 1), const ImVec2& uv4 = ImVec2(0, 1), ImU32 col = IM_COL32_WHITE);</code>
		///    IMGUI_API void  AddImageQuad(ImTextureID user_texture_id, const ImVec2& p1, const ImVec2& p2, const ImVec2& p3, const ImVec2& p4, const ImVec2& uv1 = ImVec2(0, 0), const ImVec2& uv2 = ImVec2(1, 0), const ImVec2& uv3 = ImVec2(1, 1), const ImVec2& uv4 = ImVec2(0, 1), ImU32 col = IM_COL32_WHITE); </summary>
	public void AddImageQuad(ImTextureID UserTextureId, out  Vector2 P1, out  Vector2 P2, out  Vector2 P3, out  Vector2 P4, out  Vector2 Uv1, out  Vector2 Uv2, out  Vector2 Uv3, out  Vector2 Uv4, uint Col)
	{
		ImDrawList_AddImageQuad2689(_objectPtr, UserTextureId, out P1, out P2, out P3, out P4, out Uv1, out Uv2, out Uv3, out Uv4, Col);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawList_AddImageRounded2690(IntPtr objectPtr, ImTextureID UserTextureId, out  Vector2 PMin, out  Vector2 PMax, out  Vector2 UvMin, out  Vector2 UvMax, uint Col, float Rounding, ImDrawFlags Flags);

	/// <summary><code>IMGUI_API void  AddImageRounded(ImTextureID user_texture_id, const ImVec2& p_min, const ImVec2& p_max, const ImVec2& uv_min, const ImVec2& uv_max, ImU32 col, float rounding, ImDrawFlags flags = 0);</code>
		///    IMGUI_API void  AddImageRounded(ImTextureID user_texture_id, const ImVec2& p_min, const ImVec2& p_max, const ImVec2& uv_min, const ImVec2& uv_max, ImU32 col, float rounding, ImDrawFlags flags = 0); </summary>
	public void AddImageRounded(ImTextureID UserTextureId, out  Vector2 PMin, out  Vector2 PMax, out  Vector2 UvMin, out  Vector2 UvMax, uint Col, float Rounding)
	{
		ImDrawList_AddImageRounded2690(_objectPtr, UserTextureId, out PMin, out PMax, out UvMin, out UvMax, Col, Rounding, (ImDrawFlags)0);
	}

	/// <summary><code>IMGUI_API void  AddImageRounded(ImTextureID user_texture_id, const ImVec2& p_min, const ImVec2& p_max, const ImVec2& uv_min, const ImVec2& uv_max, ImU32 col, float rounding, ImDrawFlags flags = 0);</code>
		///    IMGUI_API void  AddImageRounded(ImTextureID user_texture_id, const ImVec2& p_min, const ImVec2& p_max, const ImVec2& uv_min, const ImVec2& uv_max, ImU32 col, float rounding, ImDrawFlags flags = 0); </summary>
	public void AddImageRounded(ImTextureID UserTextureId, out  Vector2 PMin, out  Vector2 PMax, out  Vector2 UvMin, out  Vector2 UvMax, uint Col, float Rounding, ImDrawFlags Flags)
	{
		ImDrawList_AddImageRounded2690(_objectPtr, UserTextureId, out PMin, out PMax, out UvMin, out UvMax, Col, Rounding, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawList_PathArcTo2699(IntPtr objectPtr, out  Vector2 Center, float Radius, float AMin, float AMax, int NumSegments);

	/// <summary><code>IMGUI_API void  PathArcTo(const ImVec2& center, float radius, float a_min, float a_max, int num_segments = 0);</code>
		///    IMGUI_API void  PathArcTo(const ImVec2& center, float radius, float a_min, float a_max, int num_segments = 0); </summary>
	public void PathArcTo(out  Vector2 Center, float Radius, float AMin, float AMax)
	{
		ImDrawList_PathArcTo2699(_objectPtr, out Center, Radius, AMin, AMax, (int)0);
	}

	/// <summary><code>IMGUI_API void  PathArcTo(const ImVec2& center, float radius, float a_min, float a_max, int num_segments = 0);</code>
		///    IMGUI_API void  PathArcTo(const ImVec2& center, float radius, float a_min, float a_max, int num_segments = 0); </summary>
	public void PathArcTo(out  Vector2 Center, float Radius, float AMin, float AMax, int NumSegments)
	{
		ImDrawList_PathArcTo2699(_objectPtr, out Center, Radius, AMin, AMax, NumSegments);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawList_PathArcToFast2700(IntPtr objectPtr, out  Vector2 Center, float Radius, int AMinOf12, int AMaxOf12);

	/// <summary><code>IMGUI_API void  PathArcToFast(const ImVec2& center, float radius, int a_min_of_12, int a_max_of_12);                </code>
		/// Use precomputed angles for a 12 steps circle </summary>
	public void PathArcToFast(out  Vector2 Center, float Radius, int AMinOf12, int AMaxOf12)
	{
		ImDrawList_PathArcToFast2700(_objectPtr, out Center, Radius, AMinOf12, AMaxOf12);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawList_PathBezierCubicCurveTo2701(IntPtr objectPtr, out  Vector2 P2, out  Vector2 P3, out  Vector2 P4, int NumSegments);

	/// <summary><code>IMGUI_API void  PathBezierCubicCurveTo(const ImVec2& p2, const ImVec2& p3, const ImVec2& p4, int num_segments = 0); </code>
		/// Cubic Bezier (4 control points) </summary>
	public void PathBezierCubicCurveTo(out  Vector2 P2, out  Vector2 P3, out  Vector2 P4)
	{
		ImDrawList_PathBezierCubicCurveTo2701(_objectPtr, out P2, out P3, out P4, (int)0);
	}

	/// <summary><code>IMGUI_API void  PathBezierCubicCurveTo(const ImVec2& p2, const ImVec2& p3, const ImVec2& p4, int num_segments = 0); </code>
		/// Cubic Bezier (4 control points) </summary>
	public void PathBezierCubicCurveTo(out  Vector2 P2, out  Vector2 P3, out  Vector2 P4, int NumSegments)
	{
		ImDrawList_PathBezierCubicCurveTo2701(_objectPtr, out P2, out P3, out P4, NumSegments);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawList_PathBezierQuadraticCurveTo2702(IntPtr objectPtr, out  Vector2 P2, out  Vector2 P3, int NumSegments);

	/// <summary><code>IMGUI_API void  PathBezierQuadraticCurveTo(const ImVec2& p2, const ImVec2& p3, int num_segments = 0);               </code>
		/// Quadratic Bezier (3 control points) </summary>
	public void PathBezierQuadraticCurveTo(out  Vector2 P2, out  Vector2 P3)
	{
		ImDrawList_PathBezierQuadraticCurveTo2702(_objectPtr, out P2, out P3, (int)0);
	}

	/// <summary><code>IMGUI_API void  PathBezierQuadraticCurveTo(const ImVec2& p2, const ImVec2& p3, int num_segments = 0);               </code>
		/// Quadratic Bezier (3 control points) </summary>
	public void PathBezierQuadraticCurveTo(out  Vector2 P2, out  Vector2 P3, int NumSegments)
	{
		ImDrawList_PathBezierQuadraticCurveTo2702(_objectPtr, out P2, out P3, NumSegments);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawList_PathRect2703(IntPtr objectPtr, out  Vector2 RectMin, out  Vector2 RectMax, float Rounding, ImDrawFlags Flags);

	/// <summary><code>IMGUI_API void  PathRect(const ImVec2& rect_min, const ImVec2& rect_max, float rounding = 0.0f, ImDrawFlags flags = 0);</code>
		///    IMGUI_API void  PathRect(const ImVec2& rect_min, const ImVec2& rect_max, float rounding = 0.0f, ImDrawFlags flags = 0); </summary>
	public void PathRect(out  Vector2 RectMin, out  Vector2 RectMax)
	{
		ImDrawList_PathRect2703(_objectPtr, out RectMin, out RectMax, (float)0.0f, (ImDrawFlags)0);
	}

	/// <summary><code>IMGUI_API void  PathRect(const ImVec2& rect_min, const ImVec2& rect_max, float rounding = 0.0f, ImDrawFlags flags = 0);</code>
		///    IMGUI_API void  PathRect(const ImVec2& rect_min, const ImVec2& rect_max, float rounding = 0.0f, ImDrawFlags flags = 0); </summary>
	public void PathRect(out  Vector2 RectMin, out  Vector2 RectMax, float Rounding)
	{
		ImDrawList_PathRect2703(_objectPtr, out RectMin, out RectMax, Rounding, (ImDrawFlags)0);
	}

	/// <summary><code>IMGUI_API void  PathRect(const ImVec2& rect_min, const ImVec2& rect_max, float rounding = 0.0f, ImDrawFlags flags = 0);</code>
		///    IMGUI_API void  PathRect(const ImVec2& rect_min, const ImVec2& rect_max, float rounding = 0.0f, ImDrawFlags flags = 0); </summary>
	public void PathRect(out  Vector2 RectMin, out  Vector2 RectMax, float Rounding, ImDrawFlags Flags)
	{
		ImDrawList_PathRect2703(_objectPtr, out RectMin, out RectMax, Rounding, Flags);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawList_AddCallback2706(IntPtr objectPtr, ImDrawCallback Callback, IntPtr CallbackData);

	/// <summary><code>IMGUI_API void  AddCallback(ImDrawCallback callback, void* callback_data);  </code>
		/// Your rendering function must check for 'UserCallback' in ImDrawCmd and call the function instead of rendering triangles. </summary>
	public void AddCallback(ImDrawCallback Callback, IntPtr CallbackData)
	{
		ImDrawList_AddCallback2706(_objectPtr, Callback, CallbackData);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawList_AddDrawCmd2707(IntPtr objectPtr);

	/// <summary><code>IMGUI_API void  AddDrawCmd();                                               </code>
		/// This is useful if you need to forcefully create a new draw call (to allow for dependent rendering / blending). Otherwise primitives are merged into the same draw-call as much as possible </summary>
	public void AddDrawCmd()
	{
		ImDrawList_AddDrawCmd2707(_objectPtr);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern IntPtr ImDrawList_CloneOutput2708(IntPtr objectPtr);

	/// <summary><code>IMGUI_API ImDrawList* CloneOutput() const;                                  </code>
		/// Create a clone of the CmdBuffer/IdxBuffer/VtxBuffer. </summary>
	public ImDrawList CloneOutput()
	{
		return new ImDrawList(ImDrawList_CloneOutput2708(_objectPtr));
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawList_PrimReserve2723(IntPtr objectPtr, int IdxCount, int VtxCount);

	/// <summary><code>IMGUI_API void  PrimReserve(int idx_count, int vtx_count);</code>
		///    IMGUI_API void  PrimReserve(int idx_count, int vtx_count); </summary>
	public void PrimReserve(int IdxCount, int VtxCount)
	{
		ImDrawList_PrimReserve2723(_objectPtr, IdxCount, VtxCount);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawList_PrimUnreserve2724(IntPtr objectPtr, int IdxCount, int VtxCount);

	/// <summary><code>IMGUI_API void  PrimUnreserve(int idx_count, int vtx_count);</code>
		///    IMGUI_API void  PrimUnreserve(int idx_count, int vtx_count); </summary>
	public void PrimUnreserve(int IdxCount, int VtxCount)
	{
		ImDrawList_PrimUnreserve2724(_objectPtr, IdxCount, VtxCount);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawList_PrimRect2725(IntPtr objectPtr, out  Vector2 A, out  Vector2 B, uint Col);

	/// <summary><code>IMGUI_API void  PrimRect(const ImVec2& a, const ImVec2& b, ImU32 col);      </code>
		/// Axis aligned rectangle (composed of two triangles) </summary>
	public void PrimRect(out  Vector2 A, out  Vector2 B, uint Col)
	{
		ImDrawList_PrimRect2725(_objectPtr, out A, out B, Col);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawList_PrimRectUV2726(IntPtr objectPtr, out  Vector2 A, out  Vector2 B, out  Vector2 UvA, out  Vector2 UvB, uint Col);

	/// <summary><code>IMGUI_API void  PrimRectUV(const ImVec2& a, const ImVec2& b, const ImVec2& uv_a, const ImVec2& uv_b, ImU32 col);</code>
		///    IMGUI_API void  PrimRectUV(const ImVec2& a, const ImVec2& b, const ImVec2& uv_a, const ImVec2& uv_b, ImU32 col); </summary>
	public void PrimRectUV(out  Vector2 A, out  Vector2 B, out  Vector2 UvA, out  Vector2 UvB, uint Col)
	{
		ImDrawList_PrimRectUV2726(_objectPtr, out A, out B, out UvA, out UvB, Col);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawList_PrimQuadUV2727(IntPtr objectPtr, out  Vector2 A, out  Vector2 B, out  Vector2 C, out  Vector2 D, out  Vector2 UvA, out  Vector2 UvB, out  Vector2 UvC, out  Vector2 UvD, uint Col);

	/// <summary><code>IMGUI_API void  PrimQuadUV(const ImVec2& a, const ImVec2& b, const ImVec2& c, const ImVec2& d, const ImVec2& uv_a, const ImVec2& uv_b, const ImVec2& uv_c, const ImVec2& uv_d, ImU32 col);</code>
		///    IMGUI_API void  PrimQuadUV(const ImVec2& a, const ImVec2& b, const ImVec2& c, const ImVec2& d, const ImVec2& uv_a, const ImVec2& uv_b, const ImVec2& uv_c, const ImVec2& uv_d, ImU32 col); </summary>
	public void PrimQuadUV(out  Vector2 A, out  Vector2 B, out  Vector2 C, out  Vector2 D, out  Vector2 UvA, out  Vector2 UvB, out  Vector2 UvC, out  Vector2 UvD, uint Col)
	{
		ImDrawList_PrimQuadUV2727(_objectPtr, out A, out B, out C, out D, out UvA, out UvB, out UvC, out UvD, Col);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawList__ResetForNewFrame2737(IntPtr objectPtr);

	/// <summary><code>IMGUI_API void  _ResetForNewFrame();</code>
		///    IMGUI_API void  _ResetForNewFrame(); </summary>
	public void _ResetForNewFrame()
	{
		ImDrawList__ResetForNewFrame2737(_objectPtr);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawList__ClearFreeMemory2738(IntPtr objectPtr);

	/// <summary><code>IMGUI_API void  _ClearFreeMemory();</code>
		///    IMGUI_API void  _ClearFreeMemory(); </summary>
	public void _ClearFreeMemory()
	{
		ImDrawList__ClearFreeMemory2738(_objectPtr);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawList__TryMergeDrawCmds2740(IntPtr objectPtr);

	/// <summary><code>IMGUI_API void  _TryMergeDrawCmds();</code>
		///    IMGUI_API void  _TryMergeDrawCmds(); </summary>
	public void _TryMergeDrawCmds()
	{
		ImDrawList__TryMergeDrawCmds2740(_objectPtr);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawList__OnChangedClipRect2741(IntPtr objectPtr);

	/// <summary><code>IMGUI_API void  _OnChangedClipRect();</code>
		///    IMGUI_API void  _OnChangedClipRect(); </summary>
	public void _OnChangedClipRect()
	{
		ImDrawList__OnChangedClipRect2741(_objectPtr);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawList__OnChangedTextureID2742(IntPtr objectPtr);

	/// <summary><code>IMGUI_API void  _OnChangedTextureID();</code>
		///    IMGUI_API void  _OnChangedTextureID(); </summary>
	public void _OnChangedTextureID()
	{
		ImDrawList__OnChangedTextureID2742(_objectPtr);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawList__OnChangedVtxOffset2743(IntPtr objectPtr);

	/// <summary><code>IMGUI_API void  _OnChangedVtxOffset();</code>
		///    IMGUI_API void  _OnChangedVtxOffset(); </summary>
	public void _OnChangedVtxOffset()
	{
		ImDrawList__OnChangedVtxOffset2743(_objectPtr);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern int ImDrawList__CalcCircleAutoSegmentCount2744(IntPtr objectPtr, float Radius);

	/// <summary><code>IMGUI_API int   _CalcCircleAutoSegmentCount(float radius) const;</code>
		///    IMGUI_API int   _CalcCircleAutoSegmentCount(float radius) const; </summary>
	public int _CalcCircleAutoSegmentCount(float Radius)
	{
		return ImDrawList__CalcCircleAutoSegmentCount2744(_objectPtr, Radius);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawList__PathArcToFastEx2745(IntPtr objectPtr, out  Vector2 Center, float Radius, int AMinSample, int AMaxSample, int AStep);

	/// <summary><code>IMGUI_API void  _PathArcToFastEx(const ImVec2& center, float radius, int a_min_sample, int a_max_sample, int a_step);</code>
		///    IMGUI_API void  _PathArcToFastEx(const ImVec2& center, float radius, int a_min_sample, int a_max_sample, int a_step); </summary>
	public void _PathArcToFastEx(out  Vector2 Center, float Radius, int AMinSample, int AMaxSample, int AStep)
	{
		ImDrawList__PathArcToFastEx2745(_objectPtr, out Center, Radius, AMinSample, AMaxSample, AStep);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawList__PathArcToN2746(IntPtr objectPtr, out  Vector2 Center, float Radius, float AMin, float AMax, int NumSegments);

	/// <summary><code>IMGUI_API void  _PathArcToN(const ImVec2& center, float radius, float a_min, float a_max, int num_segments);</code>
		///    IMGUI_API void  _PathArcToN(const ImVec2& center, float radius, float a_min, float a_max, int num_segments); </summary>
	public void _PathArcToN(out  Vector2 Center, float Radius, float AMin, float AMax, int NumSegments)
	{
		ImDrawList__PathArcToN2746(_objectPtr, out Center, Radius, AMin, AMax, NumSegments);
	}
	}
	public class ImDrawData
	{
		private IntPtr _objectPtr;
		public IntPtr AsPtr { get => _objectPtr; }
		public ImDrawData(IntPtr Ptr){ _objectPtr = Ptr; }

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImDrawData_Get_Valid2754(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawData_Set_Valid2754(IntPtr objectPtr, [MarshalAs(UnmanagedType.I1)]bool  Value);

	/// <summary><code>bool                Valid;              </code>
		/// Only valid after Render() is called and before the next NewFrame() is called. </summary>
	public bool Valid
	{
		get { return ImDrawData_Get_Valid2754(_objectPtr);}
		set {ImDrawData_Set_Valid2754(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern int ImDrawData_Get_CmdListsCount2755(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawData_Set_CmdListsCount2755(IntPtr objectPtr, int  Value);

	/// <summary><code>int                 CmdListsCount;      </code>
		/// Number of ImDrawList* to render (should always be == CmdLists.size) </summary>
	public int CmdListsCount
	{
		get { return ImDrawData_Get_CmdListsCount2755(_objectPtr);}
		set {ImDrawData_Set_CmdListsCount2755(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern int ImDrawData_Get_TotalIdxCount2756(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawData_Set_TotalIdxCount2756(IntPtr objectPtr, int  Value);

	/// <summary><code>int                 TotalIdxCount;      </code>
		/// For convenience, sum of all ImDrawList's IdxBuffer.Size </summary>
	public int TotalIdxCount
	{
		get { return ImDrawData_Get_TotalIdxCount2756(_objectPtr);}
		set {ImDrawData_Set_TotalIdxCount2756(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern int ImDrawData_Get_TotalVtxCount2757(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawData_Set_TotalVtxCount2757(IntPtr objectPtr, int  Value);

	/// <summary><code>int                 TotalVtxCount;      </code>
		/// For convenience, sum of all ImDrawList's VtxBuffer.Size </summary>
	public int TotalVtxCount
	{
		get { return ImDrawData_Get_TotalVtxCount2757(_objectPtr);}
		set {ImDrawData_Set_TotalVtxCount2757(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern Vector2 ImDrawData_Get_DisplayPos2759(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawData_Set_DisplayPos2759(IntPtr objectPtr, Vector2  Value);

	/// <summary><code>ImVec2              DisplayPos;         </code>
		/// Top-left position of the viewport to render (== top-left of the orthogonal projection matrix to use) (== GetMainViewport()->Pos for the main viewport, == (0.0) in most single-viewport applications) </summary>
	public Vector2 DisplayPos
	{
		get { return ImDrawData_Get_DisplayPos2759(_objectPtr);}
		set {ImDrawData_Set_DisplayPos2759(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern Vector2 ImDrawData_Get_DisplaySize2760(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawData_Set_DisplaySize2760(IntPtr objectPtr, Vector2  Value);

	/// <summary><code>ImVec2              DisplaySize;        </code>
		/// Size of the viewport to render (== GetMainViewport()->Size for the main viewport, == io.DisplaySize in most single-viewport applications) </summary>
	public Vector2 DisplaySize
	{
		get { return ImDrawData_Get_DisplaySize2760(_objectPtr);}
		set {ImDrawData_Set_DisplaySize2760(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern Vector2 ImDrawData_Get_FramebufferScale2761(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawData_Set_FramebufferScale2761(IntPtr objectPtr, Vector2  Value);

	/// <summary><code>ImVec2              FramebufferScale;   </code>
		/// Amount of pixels for each unit of DisplaySize. Based on io.DisplayFramebufferScale. Generally (1,1) on normal display, (2,2) on OSX with Retina display. </summary>
	public Vector2 FramebufferScale
	{
		get { return ImDrawData_Get_FramebufferScale2761(_objectPtr);}
		set {ImDrawData_Set_FramebufferScale2761(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern IntPtr ImDrawData_Get_OwnerViewport2762(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawData_Set_OwnerViewport2762(IntPtr objectPtr, IntPtr  Value);

	/// <summary><code>ImGuiViewport*      OwnerViewport;      </code>
		/// Viewport carrying the ImDrawData instance, might be of use to the renderer (generally not). </summary>
	public ImGuiViewport OwnerViewport
	{
		get { return new ImGuiViewport(ImDrawData_Get_OwnerViewport2762(_objectPtr));}
		set {ImDrawData_Set_OwnerViewport2762(_objectPtr, value.AsPtr);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawData_Clear2766(IntPtr objectPtr);

	/// <summary><code>IMGUI_API void  Clear();</code>
		///    IMGUI_API void  Clear(); </summary>
	public void Clear()
	{
		ImDrawData_Clear2766(_objectPtr);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawData_AddDrawList2767(IntPtr objectPtr, IntPtr DrawList);

	/// <summary><code>IMGUI_API void  AddDrawList(ImDrawList* draw_list);     </code>
		/// Helper to add an external draw list into an existing ImDrawData. </summary>
	public void AddDrawList(ImDrawList DrawList)
	{
		ImDrawData_AddDrawList2767(_objectPtr, DrawList.AsPtr);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawData_DeIndexAllBuffers2768(IntPtr objectPtr);

	/// <summary><code>IMGUI_API void  DeIndexAllBuffers();                    </code>
		/// Helper to convert all buffers from indexed to non-indexed, in case you cannot render indexed. Note: this is slow and most likely a waste of resources. Always prefer indexed rendering! </summary>
	public void DeIndexAllBuffers()
	{
		ImDrawData_DeIndexAllBuffers2768(_objectPtr);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImDrawData_ScaleClipRects2769(IntPtr objectPtr, out  Vector2 FbScale);

	/// <summary><code>IMGUI_API void  ScaleClipRects(const ImVec2& fb_scale); </code>
		/// Helper to scale the ClipRect field of each ImDrawCmd. Use if your final output buffer is at a different scale than Dear ImGui expects, or if there is a difference between your window resolution and framebuffer resolution. </summary>
	public void ScaleClipRects(out  Vector2 FbScale)
	{
		ImDrawData_ScaleClipRects2769(_objectPtr, out FbScale);
	}
	}
	public class ImGuiViewport
	{
		private IntPtr _objectPtr;
		public IntPtr AsPtr { get => _objectPtr; }
		public ImGuiViewport(IntPtr Ptr){ _objectPtr = Ptr; }

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern ImGuiViewportFlags ImGuiViewport_Get_Flags3049(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiViewport_Set_Flags3049(IntPtr objectPtr, ImGuiViewportFlags  Value);

	/// <summary><code>ImGuiViewportFlags  Flags;                  </code>
		/// See ImGuiViewportFlags_ </summary>
	public ImGuiViewportFlags Flags
	{
		get { return ImGuiViewport_Get_Flags3049(_objectPtr);}
		set {ImGuiViewport_Set_Flags3049(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern Vector2 ImGuiViewport_Get_Pos3050(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiViewport_Set_Pos3050(IntPtr objectPtr, Vector2  Value);

	/// <summary><code>ImVec2              Pos;                    </code>
		/// Main Area: Position of the viewport (Dear ImGui coordinates are the same as OS desktop/native coordinates) </summary>
	public Vector2 Pos
	{
		get { return ImGuiViewport_Get_Pos3050(_objectPtr);}
		set {ImGuiViewport_Set_Pos3050(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern Vector2 ImGuiViewport_Get_Size3051(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiViewport_Set_Size3051(IntPtr objectPtr, Vector2  Value);

	/// <summary><code>ImVec2              Size;                   </code>
		/// Main Area: Size of the viewport. </summary>
	public Vector2 Size
	{
		get { return ImGuiViewport_Get_Size3051(_objectPtr);}
		set {ImGuiViewport_Set_Size3051(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern Vector2 ImGuiViewport_Get_WorkPos3052(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiViewport_Set_WorkPos3052(IntPtr objectPtr, Vector2  Value);

	/// <summary><code>ImVec2              WorkPos;                </code>
		/// Work Area: Position of the viewport minus task bars, menus bars, status bars (>= Pos) </summary>
	public Vector2 WorkPos
	{
		get { return ImGuiViewport_Get_WorkPos3052(_objectPtr);}
		set {ImGuiViewport_Set_WorkPos3052(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern Vector2 ImGuiViewport_Get_WorkSize3053(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiViewport_Set_WorkSize3053(IntPtr objectPtr, Vector2  Value);

	/// <summary><code>ImVec2              WorkSize;               </code>
		/// Work Area: Size of the viewport minus task bars, menu bars, status bars (<= Size) </summary>
	public Vector2 WorkSize
	{
		get { return ImGuiViewport_Get_WorkSize3053(_objectPtr);}
		set {ImGuiViewport_Set_WorkSize3053(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern IntPtr ImGuiViewport_Get_PlatformHandleRaw3056(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiViewport_Set_PlatformHandleRaw3056(IntPtr objectPtr, IntPtr  Value);

	/// <summary><code>void*               PlatformHandleRaw;      </code>
		/// void* to hold lower-level, platform-native window handle (under Win32 this is expected to be a HWND, unused for other platforms) </summary>
	public IntPtr PlatformHandleRaw
	{
		get { return ImGuiViewport_Get_PlatformHandleRaw3056(_objectPtr);}
		set {ImGuiViewport_Set_PlatformHandleRaw3056(_objectPtr, value);}
	}	}
	public class ImGuiPlatformImeData
	{
		private IntPtr _objectPtr;
		public IntPtr AsPtr { get => _objectPtr; }
		public ImGuiPlatformImeData(IntPtr Ptr){ _objectPtr = Ptr; }

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGuiPlatformImeData_Get_WantVisible3072(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiPlatformImeData_Set_WantVisible3072(IntPtr objectPtr, [MarshalAs(UnmanagedType.I1)]bool  Value);

	/// <summary><code>bool    WantVisible;        </code>
		/// A widget wants the IME to be visible </summary>
	public bool WantVisible
	{
		get { return ImGuiPlatformImeData_Get_WantVisible3072(_objectPtr);}
		set {ImGuiPlatformImeData_Set_WantVisible3072(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern Vector2 ImGuiPlatformImeData_Get_InputPos3073(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiPlatformImeData_Set_InputPos3073(IntPtr objectPtr, Vector2  Value);

	/// <summary><code>ImVec2  InputPos;           </code>
		/// Position of the input cursor </summary>
	public Vector2 InputPos
	{
		get { return ImGuiPlatformImeData_Get_InputPos3073(_objectPtr);}
		set {ImGuiPlatformImeData_Set_InputPos3073(_objectPtr, value);}
	}
	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern float ImGuiPlatformImeData_Get_InputLineHeight3074(IntPtr objectPtr);

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGuiPlatformImeData_Set_InputLineHeight3074(IntPtr objectPtr, float  Value);

	/// <summary><code>float   InputLineHeight;    </code>
		/// Line height </summary>
	public float InputLineHeight
	{
		get { return ImGuiPlatformImeData_Get_InputLineHeight3074(_objectPtr);}
		set {ImGuiPlatformImeData_Set_InputLineHeight3074(_objectPtr, value);}
	}	public static partial class ImGui
	{
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern ImGuiKey ImGui_GetKeyIndex3088(ImGuiKey Key);

	/// <summary><code>IMGUI_API ImGuiKey     GetKeyIndex(ImGuiKey key);  </code>
		/// map ImGuiKey_* values into legacy native key index. == io.KeyMap[key] </summary>
	public static ImGuiKey GetKeyIndex(ImGuiKey Key)
	{
		return ImGui_GetKeyIndex3088(Key);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_SetItemAllowOverlap3098();

	/// <summary><code>IMGUI_API void      SetItemAllowOverlap();                                              </code>
		/// Use SetNextItemAllowOverlap() before item. </summary>
	public static void SetItemAllowOverlap()
	{
		ImGui_SetItemAllowOverlap3098();
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool ImGui_ImageButton3103(ImTextureID UserTextureId, out  Vector2 Size, out  Vector2 Uv0, out  Vector2 Uv1, int FramePadding, out  Vector4 BgCol, out  Vector4 TintCol);

	/// <summary><code>IMGUI_API bool      ImageButton(ImTextureID user_texture_id, const ImVec2& size, const ImVec2& uv0 = ImVec2(0, 0), const ImVec2& uv1 = ImVec2(1, 1), int frame_padding = -1, const ImVec4& bg_col = ImVec4(0, 0, 0, 0), const ImVec4& tint_col = ImVec4(1, 1, 1, 1)); </code>
		/// Use new ImageButton() signature (explicit item id, regular FramePadding) </summary>
	public static bool ImageButton(ImTextureID UserTextureId, out  Vector2 Size)
	{
		 Vector2 param2 = new  Vector2 (0,  0);
		 Vector2 param3 = new  Vector2 (1,  1);
		 Vector4 param5 = new  Vector4 (0,  0,  0,  0);
		 Vector4 param6 = new  Vector4 (1,  1,  1,  1);
		return ImGui_ImageButton3103(UserTextureId, out Size, out param2, out param3, -1, out param5, out param6);
	}

	/// <summary><code>IMGUI_API bool      ImageButton(ImTextureID user_texture_id, const ImVec2& size, const ImVec2& uv0 = ImVec2(0, 0), const ImVec2& uv1 = ImVec2(1, 1), int frame_padding = -1, const ImVec4& bg_col = ImVec4(0, 0, 0, 0), const ImVec4& tint_col = ImVec4(1, 1, 1, 1)); </code>
		/// Use new ImageButton() signature (explicit item id, regular FramePadding) </summary>
	public static bool ImageButton(ImTextureID UserTextureId, out  Vector2 Size, out  Vector2 Uv0)
	{
		 Vector2 param3 = new  Vector2 (1,  1);
		 Vector4 param5 = new  Vector4 (0,  0,  0,  0);
		 Vector4 param6 = new  Vector4 (1,  1,  1,  1);
		return ImGui_ImageButton3103(UserTextureId, out Size, out Uv0, out param3, -1, out param5, out param6);
	}

	/// <summary><code>IMGUI_API bool      ImageButton(ImTextureID user_texture_id, const ImVec2& size, const ImVec2& uv0 = ImVec2(0, 0), const ImVec2& uv1 = ImVec2(1, 1), int frame_padding = -1, const ImVec4& bg_col = ImVec4(0, 0, 0, 0), const ImVec4& tint_col = ImVec4(1, 1, 1, 1)); </code>
		/// Use new ImageButton() signature (explicit item id, regular FramePadding) </summary>
	public static bool ImageButton(ImTextureID UserTextureId, out  Vector2 Size, out  Vector2 Uv0, out  Vector2 Uv1)
	{
		 Vector4 param5 = new  Vector4 (0,  0,  0,  0);
		 Vector4 param6 = new  Vector4 (1,  1,  1,  1);
		return ImGui_ImageButton3103(UserTextureId, out Size, out Uv0, out Uv1, -1, out param5, out param6);
	}

	/// <summary><code>IMGUI_API bool      ImageButton(ImTextureID user_texture_id, const ImVec2& size, const ImVec2& uv0 = ImVec2(0, 0), const ImVec2& uv1 = ImVec2(1, 1), int frame_padding = -1, const ImVec4& bg_col = ImVec4(0, 0, 0, 0), const ImVec4& tint_col = ImVec4(1, 1, 1, 1)); </code>
		/// Use new ImageButton() signature (explicit item id, regular FramePadding) </summary>
	public static bool ImageButton(ImTextureID UserTextureId, out  Vector2 Size, out  Vector2 Uv0, out  Vector2 Uv1, int FramePadding)
	{
		 Vector4 param5 = new  Vector4 (0,  0,  0,  0);
		 Vector4 param6 = new  Vector4 (1,  1,  1,  1);
		return ImGui_ImageButton3103(UserTextureId, out Size, out Uv0, out Uv1, FramePadding, out param5, out param6);
	}

	/// <summary><code>IMGUI_API bool      ImageButton(ImTextureID user_texture_id, const ImVec2& size, const ImVec2& uv0 = ImVec2(0, 0), const ImVec2& uv1 = ImVec2(1, 1), int frame_padding = -1, const ImVec4& bg_col = ImVec4(0, 0, 0, 0), const ImVec4& tint_col = ImVec4(1, 1, 1, 1)); </code>
		/// Use new ImageButton() signature (explicit item id, regular FramePadding) </summary>
	public static bool ImageButton(ImTextureID UserTextureId, out  Vector2 Size, out  Vector2 Uv0, out  Vector2 Uv1, int FramePadding, out  Vector4 BgCol)
	{
		 Vector4 param6 = new  Vector4 (1,  1,  1,  1);
		return ImGui_ImageButton3103(UserTextureId, out Size, out Uv0, out Uv1, FramePadding, out BgCol, out param6);
	}

	/// <summary><code>IMGUI_API bool      ImageButton(ImTextureID user_texture_id, const ImVec2& size, const ImVec2& uv0 = ImVec2(0, 0), const ImVec2& uv1 = ImVec2(1, 1), int frame_padding = -1, const ImVec4& bg_col = ImVec4(0, 0, 0, 0), const ImVec4& tint_col = ImVec4(1, 1, 1, 1)); </code>
		/// Use new ImageButton() signature (explicit item id, regular FramePadding) </summary>
	public static bool ImageButton(ImTextureID UserTextureId, out  Vector2 Size, out  Vector2 Uv0, out  Vector2 Uv1, int FramePadding, out  Vector4 BgCol, out  Vector4 TintCol)
	{
		return ImGui_ImageButton3103(UserTextureId, out Size, out Uv0, out Uv1, FramePadding, out BgCol, out TintCol);
	}

	[DllImport("Dear ImGui", CallingConvention = CallingConvention.StdCall)]
	private static extern void ImGui_CalcListClipping3108(int ItemsCount, float ItemsHeight, out int OutItemsDisplayStart, out int OutItemsDisplayEnd);

	/// <summary><code>IMGUI_API void      CalcListClipping(int items_count, float items_height, int* out_items_display_start, int* out_items_display_end); </code>
		/// Calculate coarse clipping for large list of evenly sized items. Prefer using ImGuiListClipper. </summary>
	public static void CalcListClipping(int ItemsCount, float ItemsHeight, out int OutItemsDisplayStart, out int OutItemsDisplayEnd)
	{
		ImGui_CalcListClipping3108(ItemsCount, ItemsHeight, out OutItemsDisplayStart, out OutItemsDisplayEnd);
	}

	}
#endregion // Functions
}