﻿// Decompiled by AS3 Sorcerer 1.99
// http://www.as3sorcerer.com/

//_0K_m._Q_s

package _0K_m{
    import flash.geom.Point;
    import com.company.assembleegameclient.objects.GameObject;

    public class _Q_s extends _I_b {

        public var start_:Point;
        public var _0E_u:Number;
        public var color_:int;

        public function _Q_s(_arg1:GameObject, _arg2:Number, _arg3:int){
            this.start_ = new Point(_arg1.x_, _arg1.y_);
            this._0E_u = _arg2;
            this.color_ = _arg3;
        }
        override public function update(_arg1:int, _arg2:int):Boolean{
            var _local7:Number;
            var _local8:Point;
            var _local9:Particle;
            x_ = this.start_.x;
            y_ = this.start_.y;
            var _local3 = 200;
            var _local4 = 200;
            var _local5:int = (4 + (this._0E_u * 2));
            var _local6:int;
            while (_local6 < _local5)
            {
                _local7 = (((_local6 * 2) * Math.PI) / _local5);
                _local8 = new Point((this.start_.x + (this._0E_u * Math.cos(_local7))), (this.start_.y + (this._0E_u * Math.sin(_local7))));
                _local9 = new _7c(_local3, this.color_, _local4, this.start_, _local8);
                map_.addObj(_local9, x_, y_);
                _local6++;
            };
            return (false);
        }

    }
}//package _0K_m

