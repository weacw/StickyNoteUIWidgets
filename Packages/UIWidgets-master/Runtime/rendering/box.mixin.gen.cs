using System;
using System.Collections.Generic;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.ui;

namespace Unity.UIWidgets.rendering {
    public abstract class
        RenderBoxContainerDefaultsMixinContainerRenderObjectMixinRenderBox<ChildType, ParentDataType>
        : ContainerRenderObjectMixinRenderBox<ChildType, ParentDataType>
        where ChildType : RenderBox
        where ParentDataType : ContainerParentDataMixinBoxParentData<ChildType> {
        public double? defaultComputeDistanceToFirstActualBaseline(TextBaseline baseline) {
            var child = this.firstChild;
            while (child != null) {
                var childParentData = (ParentDataType) child.parentData;
                double? result = child.getDistanceToActualBaseline(baseline);
                if (result != null) {
                    return result.Value + childParentData.offset.dy;
                }

                child = childParentData.nextSibling;
            }

            return null;
        }

        public double? defaultComputeDistanceToHighestActualBaseline(TextBaseline baseline) {
            double? result = null;
            var child = this.firstChild;
            while (child != null) {
                var childParentData = (ParentDataType) child.parentData;
                double? candidate = child.getDistanceToActualBaseline(baseline);
                if (candidate != null) {
                    candidate += childParentData.offset.dy;
                    if (result != null) {
                        result = Math.Min(result.Value, candidate.Value);
                    }
                    else {
                        result = candidate;
                    }
                }

                child = childParentData.nextSibling;
            }

            return result;
        }

        public void defaultPaint(PaintingContext context, Offset offset) {
            var child = this.firstChild;
            while (child != null) {
                var childParentData = (ParentDataType) child.parentData;
                context.paintChild(child, childParentData.offset + offset);
                child = childParentData.nextSibling;
            }
        }

        public bool defaultHitTestChildren(HitTestResult result, Offset position) {
            // the x, y parameters have the top left of the node's box as the origin
            ChildType child = this.lastChild;
            while (child != null) {
                ParentDataType childParentData = (ParentDataType) child.parentData;
                if (child.hitTest(result, position: position - childParentData.offset)) {
                    return true;
                }

                child = childParentData.previousSibling;
            }

            return false;
        }

        public List<ChildType> getChildrenAsList() {
            var result = new List<ChildType>();
            var child = this.firstChild;
            while (child != null) {
                var childParentData = (ParentDataType) child.parentData;
                result.Add(child);
                child = childParentData.nextSibling;
            }

            return result;
        }
    }
}