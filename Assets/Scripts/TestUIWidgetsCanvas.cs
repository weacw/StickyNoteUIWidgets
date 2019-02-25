using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Unity.UIWidgets.engine;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.material;
using Color = Unity.UIWidgets.ui.Color;
using Image = Unity.UIWidgets.widgets.Image;
using TextStyle = Unity.UIWidgets.painting.TextStyle;
using Unity.UIWidgets.gestures;

public class TestUIWidgetsCanvas : WidgetCanvas
{
    protected override Widget getWidget()
    {
        return new AsScreen();
    }




}

public class AsScreen : StatefulWidget
{
    public AsScreen(Key key = null) : base(key)
    {
    }

    public override State createState()
    {
        return new AsScreenState();
    }
}

public class AsScreenState : State<AsScreen>
{
    public override Widget build(BuildContext context)
    {
        var container = new Container(
                     color: CLColors.black,
                     child: new Container(
                         child: new Column(
                             children: new List<Widget> {
                                this._buildHeader(context),
                                this._buildContentList(context),
                                this._buildFooter(context)

                             }
                         )
                     )
                 );
        return container;
    }

    Widget _buildHeader(BuildContext context)
    {
        return new Container(
          child: new Row(
              mainAxisAlignment: MainAxisAlignment.center,
              children: new List<Widget>
                  {
                    new Container(
                        padding:EdgeInsets.only(left:Screen.width*0.053f,top:120),
                        width:Screen.width,
                        color: CLColors.background3,
                        child:
                        new Text(
                                "StickyNote Scenes",
                                style:new TextStyle(
                                fontSize:20,
                                color:CLColors.black,
                                fontWeight:FontWeight.w700
                                )
                        ))
                  }
              )
          );
    }
    bool _onNotification(ScrollNotification notification, BuildContext context)
    {
        return true;
    }
    Widget _buildContentList(BuildContext context)
    {
        return new NotificationListener<ScrollNotification>(
            onNotification: (ScrollNotification notification) =>
            {
                this._onNotification(notification, context);
                return true;
            },
            child: new Flexible(
                child: new ListView(
                    physics: new AlwaysScrollableScrollPhysics(),
                    children: new List<Widget>
                    {
                        new CustomButton(onPressed:()=>Debug.Log("Do"),child: _buildTopAssetsRow(context))
                    }
                )
            //child:ListView.builder(physics:new AlwaysScrollableScrollPhysics(),

            )
        );
    }

    Widget _buildTopAssetsRow(BuildContext context)
    {
        var test = new AssetCard("Myarworldmap", "2019-2-21");
        return new Container(
            margin: EdgeInsets.only(left: Screen.width * 0.03f),
            child: new Column(
                    children: new List<Widget>
                    {
                        test,
                        test,
                        test,
                        test,
                        test,
                        test,
                        test,
                        test,
                        test,
                        test,
                        test,
                        test,
                        test,
                        test,
                        test
                    }
                )
            );
    }

    Widget _buildFooter(BuildContext context)
    {
        return new Container(
            color: CLColors.white,
            height: 35,
            child: new Row(
                    children: new List<Widget>
                    {                        
                       new CustomButton(padding:EdgeInsets.only(left:Screen.width*0.85),onPressed: () => { Debug.Log("Do"); },child:new Text("Write"))
                    }
                )
            );
    }
    public class AssetCard : StatelessWidget
    {
        public readonly string title;
        public readonly string data;

        public AssetCard(string title, string data)
        {
            this.title = title;
            this.data = data;            
        }


        public override Widget build(BuildContext context)
        {
            return new Container(
                  child: new Container(
                      child: new Row(
                           children: new List<Widget> {
                                new Container(
                                    width: Screen.width,
                                    margin:EdgeInsets.only(left:15),
                                    height: 50,
                                    //child: new Column(
                                    //        //children:new List<Widget>
                                    //        //{
                                    //        //    new Container(
                                    //        //        margin:EdgeInsets.only(left:-Screen.width*0.5f),
                                    //        //        child:new Text("My ARWorldMap Scene",textAlign:TextAlign.left,style:new TextStyle(fontSize:15))
                                    //        //        ),
                                    //        //    new Container(
                                    //        //        margin:EdgeInsets.only(left:-Screen.width*0.5f,top:3),
                                    //        //        child:new Text("2019-02-21",textAlign:TextAlign.left,style:new TextStyle(fontSize:10))
                                    //        //        ),
                                    //        //        new Divider(height:5)
                                    //        //}
                                    //        children:
                                    //    )                                   
                                       child:new ListTile(leading:new Text("dsf"))
                                ) }
                        )
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
            Color backgroundColor = null,
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
        public readonly Color backgroundColor;

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
}

public static class CLColors
{
    public static readonly Color primary = new Color(0xFFE91E63);
    public static readonly Color secondary1 = new Color(0xFF00BCD4);
    public static readonly Color secondary2 = new Color(0xFFF0513C);
    public static readonly Color background1 = new Color(0xFF292929);
    public static readonly Color background2 = new Color(0xFF383838);
    public static readonly Color background3 = new Color(0xFFF5F5F5);
    public static readonly Color background4 = new Color(0xFF00BCD4);
    public static readonly Color icon1 = new Color(0xFFFFFFFF);
    public static readonly Color icon2 = new Color(0xFFA4A4A4);
    public static readonly Color text1 = new Color(0xFFFFFFFF);
    public static readonly Color text2 = new Color(0xFFD8D8D8);
    public static readonly Color text3 = new Color(0xFF959595);
    public static readonly Color text4 = new Color(0xFF002835);
    public static readonly Color text5 = new Color(0xFF9E9E9E);
    public static readonly Color text6 = new Color(0xFF002835);
    public static readonly Color text7 = new Color(0xFF5A5A5B);
    public static readonly Color text8 = new Color(0xFF239988);
    public static readonly Color text9 = new Color(0xFFB3B5B6);
    public static readonly Color text10 = new Color(0xFF00BCD4);
    public static readonly Color dividingLine1 = new Color(0xFF666666);
    public static readonly Color dividingLine2 = new Color(0xFF404040);

    public static readonly Color transparent = new Color(0x00000000);
    public static readonly Color white = new Color(0xFFFFFFFF);
    public static readonly Color black = new Color(0xFF000000);
    public static readonly Color red = new Color(0xFFFF0000);
    public static readonly Color green = new Color(0xFF00FF00);
    public static readonly Color blue = new Color(0xFF0000FF);

    public static readonly Color header = new Color(0xFF060B0C);
}

