// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace Base
{

using global::System;
using global::System.Collections.Generic;
using global::Google.FlatBuffers;

public struct Test : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_22_12_06(); }
  public static Test GetRootAsTest(ByteBuffer _bb) { return GetRootAsTest(_bb, new Test()); }
  public static Test GetRootAsTest(ByteBuffer _bb, Test obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public Test __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public string Name { get { int o = __p.__offset(4); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
#if ENABLE_SPAN_T
  public Span<byte> GetNameBytes() { return __p.__vector_as_span<byte>(4, 1); }
#else
  public ArraySegment<byte>? GetNameBytes() { return __p.__vector_as_arraysegment(4); }
#endif
  public byte[] GetNameArray() { return __p.__vector_as_array<byte>(4); }
  public int Damage { get { int o = __p.__offset(6); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }

  public static Offset<Base.Test> CreateTest(FlatBufferBuilder builder,
      StringOffset nameOffset = default(StringOffset),
      int damage = 0) {
    builder.StartTable(2);
    Test.AddDamage(builder, damage);
    Test.AddName(builder, nameOffset);
    return Test.EndTest(builder);
  }

  public static void StartTest(FlatBufferBuilder builder) { builder.StartTable(2); }
  public static void AddName(FlatBufferBuilder builder, StringOffset nameOffset) { builder.AddOffset(0, nameOffset.Value, 0); }
  public static void AddDamage(FlatBufferBuilder builder, int damage) { builder.AddInt(1, damage, 0); }
  public static Offset<Base.Test> EndTest(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<Base.Test>(o);
  }
}


}
