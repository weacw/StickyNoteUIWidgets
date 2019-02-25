﻿using System;
using System.Collections.Generic;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.service;
using Unity.UIWidgets.ui;
using UnityEngine;
using Canvas = Unity.UIWidgets.ui.Canvas;
using Rect = Unity.UIWidgets.ui.Rect;

namespace Unity.UIWidgets.painting {
    public class TextPainter {
        TextSpan _text;
        TextAlign _textAlign;
        TextDirection? _textDirection;
        double _textScaleFactor;
        Paragraph _layoutTemplate;
        Paragraph _paragraph;
        bool _needsLayout = true;
        int? _maxLines;
        string _ellipsis;
        double _lastMinWidth;
        double _lastMaxWidth;

        public TextPainter(TextSpan text,
            TextAlign textAlign = TextAlign.left,
            TextDirection textDirection = TextDirection.ltr,
            double textScaleFactor = 1.0,
            int? maxLines = null,
            string ellipsis = "") {
            this._text = text;
            this._textAlign = textAlign;
            this._textDirection = textDirection;
            this._textScaleFactor = textScaleFactor;
            this._maxLines = maxLines;
            this._ellipsis = ellipsis;
        }


        public double textScaleFactor {
            get { return this._textScaleFactor; }
            set {
                if (this._textScaleFactor == value) {
                    return;
                }

                this._textScaleFactor = value;
                this._paragraph = null;
                this._layoutTemplate = null;
                this._needsLayout = true;
            }
        }

        public string ellipsis {
            get { return this._ellipsis; }
            set {
                if (this._ellipsis == value) {
                    return;
                }

                this._ellipsis = value;
                this._paragraph = null;
                this._needsLayout = true;
            }
        }

        public TextSpan text {
            get { return this._text; }
            set {
                if (this.text.Equals(value)) {
                    return;
                }

                if (!Equals(this._text == null ? null : this._text.style, value == null ? null : value.style)) {
                    this._layoutTemplate = null;
                }

                this._text = value;
                this._paragraph = null;
                this._needsLayout = true;
            }
        }

        public Size size {
            get {
                Debug.Assert(!this._needsLayout);
                return new Size(this.width, this.height);
            }
        }

        public TextDirection? textDirection {
            get { return this._textDirection; }
            set {
                if (this.textDirection == value) {
                    return;
                }

                this._textDirection = value;
                this._paragraph = null;
                this._layoutTemplate = null;
                this._needsLayout = true;
            }
        }

        public TextAlign textAlign {
            get { return this._textAlign; }
            set {
                if (this._textAlign == value) {
                    return;
                }

                this._textAlign = value;
                this._paragraph = null;
                this._needsLayout = true;
            }
        }

        public bool didExceedMaxLines {
            get {
                Debug.Assert(!this._needsLayout);
                return this._paragraph.didExceedMaxLines;
            }
        }

        public int? maxLines {
            get { return this._maxLines; }
            set {
                if (this._maxLines == value) {
                    return;
                }

                this._maxLines = value;
                this._paragraph = null;
                this._needsLayout = true;
            }
        }

        public double minIntrinsicWidth {
            get {
                Debug.Assert(!this._needsLayout);
                return this._applyFloatingPointHack(this._paragraph.minIntrinsicWidth);
            }
        }

        public double maxIntrinsicWidth {
            get {
                Debug.Assert(!this._needsLayout);
                return this._applyFloatingPointHack(this._paragraph.maxIntrinsicWidth);
            }
        }

        public double height {
            get {
                Debug.Assert(!this._needsLayout);
                return this._applyFloatingPointHack(this._paragraph.height);
            }
        }

        public double width {
            get {
                Debug.Assert(!this._needsLayout);
                return this._applyFloatingPointHack(this._paragraph.width);
            }
        }

        public double computeDistanceToActualBaseline(TextBaseline baseline) {
            Debug.Assert(!this._needsLayout);
            switch (baseline) {
                case TextBaseline.alphabetic:
                    return this._paragraph.alphabeticBaseline;
                case TextBaseline.ideographic:
                    return this._paragraph.ideographicBaseline;
            }

            return 0.0;
        }

        public void layout(double minWidth = 0.0, double maxWidth = double.PositiveInfinity) {
            Debug.Assert(this.text != null,
                "TextPainter.text must be set to a non-null value before using the TextPainter.");
            Debug.Assert(this.textDirection != null,
                "TextPainter.textDirection must be set to a non-null value before using the TextPainter.");
            if (!this._needsLayout && minWidth == this._lastMinWidth && maxWidth == this._lastMaxWidth) {
                return;
            }

            this._needsLayout = false;
            if (this._paragraph == null) {
                var builder = new ParagraphBuilder(this._createParagraphStyle());
                this._text.build(builder, this.textScaleFactor);
                this._paragraph = builder.build();
            }

            this._lastMinWidth = minWidth;
            this._lastMaxWidth = maxWidth;
            this._paragraph.layout(new ParagraphConstraints(maxWidth));

            if (minWidth != maxWidth) {
                var newWidth = MathUtils.clamp(this.maxIntrinsicWidth, minWidth, maxWidth);
                if (newWidth != this.width) {
                    this._paragraph.layout(new ParagraphConstraints(newWidth));
                }
            }
        }

        public void paint(Canvas canvas, Offset offset) {
            Debug.Assert(!this._needsLayout);
            this._paragraph.paint(canvas, offset);
        }

        public Offset getOffsetForCaret(TextPosition position, Rect caretPrototype) {
            D.assert(!this._needsLayout);
            var offset = position.offset;
            if (offset > 0) {
                var prevCodeUnit = this._text.codeUnitAt(offset);
                if (prevCodeUnit == null) // out of upper bounds
                {
                    var rectNextLine = this._paragraph.getNextLineStartRect();
                    if (rectNextLine != null) {
                        return new Offset(rectNextLine.start, rectNextLine.top);
                    }
                }
            }

            switch (position.affinity) {
                case TextAffinity.upstream:
                    return this._getOffsetFromUpstream(offset, caretPrototype) ??
                           this._getOffsetFromDownstream(offset, caretPrototype) ?? this._emptyOffset;
                case TextAffinity.downstream:
                    return this._getOffsetFromDownstream(offset, caretPrototype) ??
                           this._getOffsetFromUpstream(offset, caretPrototype) ?? this._emptyOffset;
            }

            return null;
        }

        public Paragraph.LineRange getLineRange(int lineNumber) {
            D.assert(!this._needsLayout);
            return this._paragraph.getLineRange(lineNumber);
        }

        public Paragraph.LineRange getLineRange(TextPosition textPosition) {
            return this.getLineRange(this.getLineIndex(textPosition));
        }

        public List<TextBox> getBoxesForSelection(TextSelection selection) {
            D.assert(!this._needsLayout);
            var results = this._paragraph.getRectsForRange(selection.start, selection.end);
            return results;
        }

        public TextPosition getPositionForOffset(Offset offset) {
            D.assert(!this._needsLayout);
            var result = this._paragraph.getGlyphPositionAtCoordinate(offset.dx, offset.dy);
            return new TextPosition(result.position, result.affinity);
        }

        public TextRange getWordBoundary(TextPosition position) {
            D.assert(!this._needsLayout);
            var range = this._paragraph.getWordBoundary(position.offset);
            return new TextRange(range.start, range.end);
        }

        public TextPosition getPositionVerticalMove(TextPosition position, int move) {
            D.assert(!this._needsLayout);
            var offset = this.getOffsetForCaret(position, Rect.zero);
            var lineIndex = Math.Min(Math.Max(this._paragraph.getLine(position) + move, 0),
                this._paragraph.getLineCount() - 1);
            var targetLineStart = this._paragraph.getLineRange(lineIndex).start;
            var newLineOffset = this.getOffsetForCaret(new TextPosition(targetLineStart), Rect.zero);
            return this.getPositionForOffset(new Offset(offset.dx, newLineOffset.dy));
        }

        public int getLineIndex(TextPosition position) {
            D.assert(!this._needsLayout);
            return this._paragraph.getLine(position);
        }

        public int getLineCount() {
            D.assert(!this._needsLayout);
            return this._paragraph.getLineCount();
        }

        public TextPosition getWordRight(TextPosition position) {
            D.assert(!this._needsLayout);
            var offset = position.offset;
            while (true) {
                var range = this._paragraph.getWordBoundary(offset);
                if (range.end == range.start) {
                    break;
                }

                if (!char.IsWhiteSpace((char) (this.text.codeUnitAt(range.start) ?? 0))) {
                    return new TextPosition(range.end);
                }

                offset = range.end;
            }

            return new TextPosition(offset, position.affinity);
        }

        public TextPosition getWordLeft(TextPosition position) {
            D.assert(!this._needsLayout);
            var offset = Math.Max(position.offset - 1, 0);
            while (true) {
                var range = this._paragraph.getWordBoundary(offset);
                if (!char.IsWhiteSpace((char) (this.text.codeUnitAt(range.start) ?? 0))) {
                    return new TextPosition(range.start);
                }

                offset = Math.Max(range.start - 1, 0);
                if (offset == 0) {
                    break;
                }
            }

            return new TextPosition(offset, position.affinity);
        }

        ParagraphStyle _createParagraphStyle(TextDirection defaultTextDirection = TextDirection.ltr) {
            if (this._text.style == null) {
                return new ParagraphStyle(
                    textAlign: this.textAlign,
                    textDirection: this.textDirection ?? defaultTextDirection,
                    maxLines: this.maxLines,
                    ellipsis: this.ellipsis
                );
            }

            return this._text.style.getParagraphStyle(this.textAlign, this.textDirection ?? defaultTextDirection,
                this.ellipsis, this.maxLines, this.textScaleFactor);
        }

        public double preferredLineHeight {
            get {
                if (this._layoutTemplate == null) {
                    var builder = new ParagraphBuilder(this._createParagraphStyle(TextDirection.ltr)
                    ); // direction doesn't matter, text is just a space
                    if (this.text != null && this.text.style != null) {
                        builder.pushStyle(this.text.style);
                    }

                    builder.addText(" ");
                    this._layoutTemplate = builder.build();
                    this._layoutTemplate.layout(new ParagraphConstraints(double.PositiveInfinity));
                }

                return this._layoutTemplate.height;
            }
        }

        double _applyFloatingPointHack(double layoutValue) {
            return Math.Ceiling(layoutValue);
        }


        Offset _getOffsetFromUpstream(int offset, Rect caretPrototype) {
            var prevCodeUnit = this._text.codeUnitAt(offset - 1);
            if (prevCodeUnit == null) {
                return null;
            }

            var prevRuneOffset = _isUtf16Surrogate((int) prevCodeUnit) ? offset - 2 : offset - 1;
            var boxes = this._paragraph.getRectsForRange(prevRuneOffset, offset);
            if (boxes.Count == 0) {
                return null;
            }

            var box = boxes[0];
            var caretEnd = box.end;
            var dx = box.direction == TextDirection.rtl ? caretEnd : caretEnd - caretPrototype.width;
            return new Offset(dx, box.top);
        }

        Offset _getOffsetFromDownstream(int offset, Rect caretPrototype) {
            var nextCodeUnit = this._text.codeUnitAt(offset);
            if (nextCodeUnit == null) {
                return null;
            }

            var nextRuneOffset = _isUtf16Surrogate((int) nextCodeUnit) ? offset + 2 : offset + 1;
            var boxes = this._paragraph.getRectsForRange(offset, nextRuneOffset);
            if (boxes.Count == 0) {
                return null;
            }

            var box = boxes[0];
            var caretStart = box.start;
            var dx = box.direction == TextDirection.rtl ? caretStart - caretPrototype.width : caretStart;
            return new Offset(dx, box.top);
        }

        Offset _emptyOffset {
            get {
                D.assert(!this._needsLayout);
                switch (this.textAlign) {
                    case TextAlign.left:
                        return Offset.zero;
                    case TextAlign.right:
                        return new Offset(this.width, 0.0);
                    case TextAlign.center:
                        return new Offset(this.width / 2.0, 0.0);
                    case TextAlign.justify:
                        if (this.textDirection == TextDirection.rtl) {
                            return new Offset(this.width, 0.0);
                        }

                        return Offset.zero;
                }

                return null;
            }
        }

        static bool _isUtf16Surrogate(int value) {
            return (value & 0xF800) == 0xD800;
        }
    }
}