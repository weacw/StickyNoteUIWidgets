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
using TextStyle = Unity.UIWidgets.painting.TextStyle;

public class StickyNoteCanvas : WidgetCanvas
{
    protected override Widget getWidget()
    {
        return new StickyNoteStatefulWidget();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        FontManager.instance.addFont(Resources.Load<Font>(path: "Material-Design-Iconic-Font"));
    }
}


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
                child: new Text("StickyNote Scenes", style: new TextStyle(fontSize: 20.0,fontWeight:FontWeight.w700))
            );
    }


    private Widget _buildBody(BuildContext context)
    {
        return new NotificationListener<ScrollNotification>(
                onNotification: (ScrollNotification notification) => { return true; },
                child: new Flexible(
                        child: new ListView(
                                physics: new AlwaysScrollableScrollPhysics(),
                                children: new List<Widget>
                                {
                                    this._buildRowList(context)
                                }
                            )
                    )
            );
    }


    private Widget _buildFooter(BuildContext context)
    {
        return new Container(
            padding: EdgeInsets.only(left:300),
            height:50,                     
            width:Screen.width,
            child: new Container(
                child:new Icon(icon:Icons.test,size:30)
                )
            );        
    }

    private Widget _buildRowList(BuildContext context)
    {
        return new Container(
                padding:EdgeInsets.only(top:30),
                child:new Column(
                        children:new List<Widget>
                        {
                             new ItemArticle("My test app "+Random.Range(1,10000), "2019-02-22"),
                             new ItemArticle("My test app "+Random.Range(1,10000), "2019-02-22"),
                             new ItemArticle("My test app "+Random.Range(1,10000), "2019-02-22"),
                             new ItemArticle("My test app "+Random.Range(1,10000), "2019-02-22"),
                             new ItemArticle("My test app "+Random.Range(1,10000), "2019-02-22"),
                             new ItemArticle("My test app "+Random.Range(1,10000), "2019-02-22"),
                             new ItemArticle("My test app "+Random.Range(1,10000), "2019-02-22"),
                             new ItemArticle("My test app "+Random.Range(1,10000), "2019-02-22"),
                             new ItemArticle("My test app "+Random.Range(1,10000), "2019-02-22"),
                             new ItemArticle("My test app "+Random.Range(1,10000), "2019-02-22"),
                             new ItemArticle("My test app "+Random.Range(1,10000), "2019-02-22"),
                             new ItemArticle("My test app "+Random.Range(1,10000), "2019-02-22"),
                             new ItemArticle("My test app "+Random.Range(1,10000), "2019-02-22"),
                             new ItemArticle("My test app "+Random.Range(1,10000), "2019-02-22"),
                             new ItemArticle("My test app "+Random.Range(1,10000), "2019-02-22"),
                             new ItemArticle("My test app "+Random.Range(1,10000), "2019-02-22"),
                             new ItemArticle("My test app "+Random.Range(1,10000), "2019-02-22"),
                             new ItemArticle("My test app "+Random.Range(1,10000), "2019-02-22"),
                             new ItemArticle("My test app "+Random.Range(1,10000), "2019-02-22"),
                             new ItemArticle("My test app "+Random.Range(1,10000), "2019-02-22"),
                             new ItemArticle("My test app "+Random.Range(1,10000), "2019-02-22"),
                             new ItemArticle("My test app "+Random.Range(1,10000), "2019-02-22"),
                             new ItemArticle("My test app "+Random.Range(1,10000), "2019-02-22"),
                             new ItemArticle("My test app "+Random.Range(1,10000), "2019-02-22"),
                             new ItemArticle("My test app "+Random.Range(1,10000), "2019-02-22"),
                             new ItemArticle("My test app "+Random.Range(1,10000), "2019-02-22"),
                             new ItemArticle("My test app "+Random.Range(1,10000), "2019-02-22"),
                             new ItemArticle("My test app "+Random.Range(1,10000), "2019-02-22"),
                             new ItemArticle("My test app "+Random.Range(1,10000), "2019-02-22"),
                             new ItemArticle("My test app "+Random.Range(1,10000), "2019-02-22"),
                             new ItemArticle("My test app "+Random.Range(1,10000), "2019-02-22"),
    }
                    )
            );
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
        return new Container(
            padding: EdgeInsets.only(left: 22),
            height: 80,
            child: new Column(
                    children: new List<Widget>
                    {
                        new Container(
                            width:Screen.width,
                            child:new Text(title,style:new TextStyle(fontSize:18))),
                        new Container(
                          //  margin:EdgeInsets.only(top:10),
                            width:Screen.width,
                            child:new Text(date,style:new TextStyle(fontSize:10))),
                        new Divider(height:16)
                    }
                )
            );
    }
}

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
    public static readonly IconData notifications = new IconData(0x00ae, fontFamily: "Material Icons");
    public static readonly IconData account_circle = new IconData(0x00b6, fontFamily: "Material Icons");
    public static readonly IconData search = new IconData(0xe8b6, fontFamily: "Material Icons");
    public static readonly IconData keyboard_arrow_down = new IconData(0xe313, fontFamily: "Material Icons");
    public static readonly IconData test = new IconData(0xf158, fontFamily: "Material-Design-Iconic-Font");
}