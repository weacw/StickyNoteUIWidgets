using System.Collections;
using System.Collections.Generic;
using Unity.UIWidgets.engine;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.material;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Color = Unity.UIWidgets.ui.Color;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

public class StickyNoteCanvas : WidgetCanvas
{
    //protected override Widget getWidget()
    //{
    //    return new StickyNoteWriterViewStatefulWidget();
    //}
    protected override void OnEnable()
    {
        base.OnEnable();
        FontManager.instance.addFont(Resources.Load<Font>(path: "Material-Design-Iconic-Font"));
    }

    protected override string initialRoute { get { return "/"; } }
    protected override Dictionary<string, WidgetBuilder> routes
    {
        get
        {
            return new Dictionary<string, WidgetBuilder>
            {
                {"/",(context)=>new StickyNoteStatefulWidget() },
                {"ARView",(context)=>new StickyNoteARViewStatefulWidget() },
                {"WriteView",(context)=>new StickyNoteWriterViewStatefulWidget() }
            };
        }
    }

    protected override PageRouteFactory pageRouteBuilder
    {
        get
        {
            return (RouteSettings settings, WidgetBuilder builder) => new PageRouteBuilder(
                settings: settings,
                pageBuilder: (BuildContext context,
                              Unity.UIWidgets.animation.Animation<double> animation,
                              Unity.UIWidgets.animation.Animation<double> secondaryAnimation) => builder(context),
                transitionsBuilder: (BuildContext context,
                              Unity.UIWidgets.animation.Animation<double> animation,
                              Unity.UIWidgets.animation.Animation<double> secondaryAnimation, Widget child) =>
                new _FadeUpwardsPageTransition(
                                                routeAnimation: animation,
                                                child: child)
                );
        }
    }
}
#region StickyNote main view
public class StickyNoteStatefulWidget : StatefulWidget
{
    public StickyNoteStatefulWidget(Key key = null) : base(key)
    {
    }

    public override State createState()
    {
        return new StickyNoteState();
    }
}

public class StickyNoteState : State<StickyNoteStatefulWidget>
{
    public List<Widget> rows = new List<Widget>();

    public override void initState()
    {
        base.initState();
        _getData();
    }
    public override Widget build(BuildContext context)
    {
        var container = new Container(
            color: CLColors.background3,
            child: new Column(
                    crossAxisAlignment: Unity.UIWidgets.rendering.CrossAxisAlignment.start,
                    children: new List<Widget>
                    {
                        this._buildTitle(context),
                        this._buildBody(context),
                        this._buildFooter(context)
                    }
                )
            );
        return container;
    }


    private Widget _buildTitle(BuildContext context)
    {
        return new Container(
                padding: EdgeInsets.only(left: 20, top: 128),
                child: new Text("StickyNote Scenes", style: new TextStyle(fontSize: 20.0, fontWeight: FontWeight.w700))
            );
    }


    private Widget _buildBody(BuildContext context)
    {
        return new Flexible(
                        child: new Container(
                                padding: EdgeInsets.only(top: 30),
                                child: new ListView(
                                    physics: new AlwaysScrollableScrollPhysics(),
                                    children: rows
                                )
                            )
                    );
    }


    private Widget _buildFooter(BuildContext context)
    {
        return new Container(
            padding: EdgeInsets.only(250),
            height: 50,
            width: Screen.width,
            child: new GestureDetector(
                       onTap: () => { Navigator.pushName(context, "WriteView"); },
                       child: new Icon(icon: Icons.write, size: 30)
                    )
            );
    }


    private void _getData()
    {
        ItemList tmp = new ItemList();
        for (int i = 0; i < 20; i++)
        {
            tmp.tmp.Add(new Item("Item " + i, System.DateTime.Now.ToShortDateString()));
        }

        string json = JsonUtility.ToJson(tmp);

        tmp = JsonUtility.FromJson<ItemList>(json);
        Debug.Log(tmp.tmp.Count);
        foreach (var item in tmp.tmp)
        {
            rows.Add(new ItemArticle(item.title, item.date));
        }
    }

}
class ItemArticle : StatelessWidget
{
    public string title;
    public string date;

    public ItemArticle(string title, string date)
    {
        this.title = title;
        this.date = date;
    }


    public override Widget build(BuildContext context)
    {
        return new GestureDetector(
            onTap: () =>
            {
                Navigator.pushName(context, "ARView");
            },
            child: new Container(
            padding: EdgeInsets.only(left: 22),
            height: 80,
            child: new Column(
                    children: new List<Widget>
                    {
                        new Container(
                            width:Screen.width,
                            child:new Text(title,style:new TextStyle(fontSize:18))),
                        new Container(
                            margin:EdgeInsets.only(top:10),
                            width:Screen.width,
                            child:new Text(date,style:new TextStyle(fontSize:10))),
                        new Divider(height:16)
                    }
                )
            )
            );
    }

    private void TESTdebug(string str)
    {
        Debug.Log(str);
    }
}

#endregion


#region StickyNote AR view
public class StickyNoteARViewStatefulWidget : StatefulWidget
{
    public StickyNoteARViewStatefulWidget(Key key = null) : base(key)
    {
    }

    public override State createState()
    {
        return new StickyNoteARViewState();
    }
}

public class StickyNoteARViewState : State<StickyNoteARViewStatefulWidget>
{
    public override Widget build(BuildContext context)
    {
        return new Container(
            child: new Column(
                children: new List<Widget>
                    {
                      this._buidHeader(context),
                      this._buildFooter(context),

                    }
                )
            );
    }



    private Widget _buidHeader(BuildContext context)
    {
        return new Container(
                width: Screen.width,
                padding: EdgeInsets.only(left: -260, top: 70),
                child: new GestureDetector(
                    onTap: () =>
                    {
                        Navigator.pop(context);
                    },
                    child: new Icon(icon: Icons.back, size: 30)
                )
            );
    }

    private Widget _buildFooter(BuildContext context)
    {
        return new Container(
                    margin: EdgeInsets.only(top: 450),
                    child: new GestureDetector(
                        onTap: () => { Debug.Log("Place object"); },
                        child: new Icon(icon: Icons.place, size: 50)
                        )
                   );
    }
}
#endregion



#region StickyNote Writer view
public class StickyNoteWriterViewStatefulWidget : StatefulWidget
{
    public StickyNoteWriterViewStatefulWidget(Key key = null) : base(key)
    {
    }

    public override State createState()
    {
        return new StickyNoteWriterViewState();
    }
}

public class StickyNoteWriterViewState : State<StickyNoteWriterViewStatefulWidget>
{
    TextEditingController descController = new TextEditingController("");

    public StickyNoteWriterViewState()
    {
    }

    public override Widget build(BuildContext context)
    {
        var container = new Container(
            color: CLColors.background3,
            child: new Column(
                    crossAxisAlignment: Unity.UIWidgets.rendering.CrossAxisAlignment.start,
                    children: new List<Widget>
                    {
                        this._buildHeader(context),
                        this._buildInputArea(context)

                    }
                )
            );
        return container;
    }


    private Widget _buildHeader(BuildContext context)
    {
        return new Container(
              child: new Column(
                  children: new List<Widget>
                          {
                                new Container(
                                    margin:EdgeInsets.only(left:250,top:50),
                                    child:new Text("Finished", style: new TextStyle(fontSize: 15,color:CLColors.blue))
                                 ),
                             new Container(                                 
                                    margin:EdgeInsets.only(left:-140,top:78),
                                    child:new Text("Writting", style: new TextStyle(fontSize: 20.0, fontWeight: FontWeight.w700))
                                 ),
                          }
                  )
          );
    }

    private Widget _buildInputArea(BuildContext context)
    {
        return new Container(
                child: new Column(
                        children: new List<Widget>
                        {
                            new Flexible(child:new Container(
                                    height:400,
                                    margin:EdgeInsets.all(15),
                                    decoration:new BoxDecoration(border:Border.all(new Color(0xFF000000),1)),
                                    child:new EditableText(maxLines:20,
                                        controller:this.descController,
                                        selectionControls:MaterialUtils.materialTextSelectionControls,
                                        focusNode:new FocusNode(),
                                        style:new TextStyle(
                                                fontSize:15,
                                                height:1.5f,
                                                color:new Color(0xFF1389FD)
                                            ),
                                        selectionColor:Color.fromARGB(255,255,0,0),
                                        cursorColor:Color.fromARGB(255,0,0,0))
                                    )
                                )
                        }
                    )
            );
    }
}
#endregion

public class CustomButton : StatelessWidget
{
    public CustomButton(
        Key key = null,
        GestureTapCallback onPressed = null,
        EdgeInsets padding = null,
        Unity.UIWidgets.ui.Color backgroundColor = null,
        Widget child = null
    ) : base(key: key)
    {
        this.onPressed = onPressed;
        this.padding = padding ?? EdgeInsets.all(8.0);
        this.backgroundColor = backgroundColor ?? CLColors.transparent;
        this.child = child;
    }

    public readonly GestureTapCallback onPressed;
    public readonly EdgeInsets padding;
    public readonly Widget child;
    public readonly Unity.UIWidgets.ui.Color backgroundColor;

    public override Widget build(BuildContext context)
    {
        return new GestureDetector(
            onTap: this.onPressed,
            child: new Container(
                padding: this.padding,
                color: this.backgroundColor,
                child: this.child
            )
        );
    }
}
public static class Icons
{
    public static readonly IconData write = new IconData(0xf158, fontFamily: "Material-Design-Iconic-Font");
    public static readonly IconData back = new IconData(0x2039, fontFamily: "Material-Design-Iconic-Font");
    public static readonly IconData place = new IconData(0x2609, fontFamily: "Material-Design-Iconic-Font");
}
class _FadeUpwardsPageTransition : StatelessWidget
{
    internal _FadeUpwardsPageTransition(
        Key key = null,
        Unity.UIWidgets.animation.Animation<double> routeAnimation = null, // The route's linear 0.0 - 1.0 animation.
        Widget child = null
    ) : base(key: key)
    {
        this._positionAnimation = _bottomUpTween.chain(_fastOutSlowInTween).animate(routeAnimation);
        this._opacityAnimation = _easeInTween.animate(routeAnimation);
        this.child = child;
    }

    static Unity.UIWidgets.animation.Tween<Offset> _bottomUpTween = new Unity.UIWidgets.animation.OffsetTween(
        begin: new Offset(0.0, 0.25),
        end: Offset.zero
    );

    static Unity.UIWidgets.animation.Animatable<double> _fastOutSlowInTween = new Unity.UIWidgets.animation.CurveTween(curve: Unity.UIWidgets.animation.Curves.easeOut);
    static Unity.UIWidgets.animation.Animatable<double> _easeInTween = new Unity.UIWidgets.animation.CurveTween(curve: Unity.UIWidgets.animation.Curves.easeIn);

    readonly Unity.UIWidgets.animation.Animation<Offset> _positionAnimation;
    readonly Unity.UIWidgets.animation.Animation<double> _opacityAnimation;
    public readonly Widget child;

    public override Widget build(BuildContext context)
    {
        return new SlideTransition(
            position: this._positionAnimation,
            child: new FadeTransition(
                opacity: this._opacityAnimation,
                child: this.child
            )
        );
    }
}


[System.Serializable]
public class Item
{
    public string title;
    public string date;
    public Item(string item, string date)
    {
        this.title = item;
        this.date = date;
    }
}
[System.Serializable]
public class ItemList
{
    public List<Item> tmp = new List<Item>();
}
