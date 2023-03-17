using Orleans.Serialization;
using Orleans.Serialization.Buffers;
using Orleans.Serialization.Cloning;
using Orleans.Serialization.Serializers;
using Orleans.Serialization.WireProtocol;
using Serialize.Linq.Serializers;
using System.Buffers;
using System.Linq.Expressions;

namespace Web3raffle.Utilities.Serializers;

public class ExpressionTypeSerializer : IGeneralizedCodec, IGeneralizedCopier, ITypeFilter
{
	private readonly ExpressionSerializer expressionSerializer;

	public ExpressionTypeSerializer()
	{
		var serializer = new BinarySerializer();
		this.expressionSerializer = new ExpressionSerializer(serializer);
	}

	private byte[] SerializeBytes(Expression exp)
	{
		return this.expressionSerializer.SerializeBinary(exp);
	}

	private Expression DeserializeBytes(byte[] bytes)
	{
		return this.expressionSerializer.DeserializeBinary(bytes);
	}

	private object Copy(Expression exp)
	{
		var copyRefBytes = this.SerializeBytes(exp);
		var copyResults = this.DeserializeBytes(copyRefBytes);

		return copyResults;
	}

	public object DeepCopy(object source, CopyContext context)
	{
		if (source is not Expression exp)
		{
			throw new NotSupportedException("Only expressions are supported");
		}

		return this.Copy(exp);
	}

	public bool IsSupportedType(Type itemType)
	{
		var isExpression = itemType == typeof(Expression);
		var isExpressionAssigned = itemType.IsAssignableFrom(typeof(Expression));
		var isExpressionSubclassed = itemType.IsSubclassOf(typeof(Expression));

		if (isExpression || isExpressionAssigned || isExpressionSubclassed)
		{
			return true;
		}

		return false;
	}

	public void WriteField<TBufferWriter>(ref Writer<TBufferWriter> writer, uint fieldIdDelta, Type expectedType, object value) where TBufferWriter : IBufferWriter<byte>
		=> throw new NotImplementedException();

	public object ReadValue<TInput>(ref Reader<TInput> reader, Field field)
		=> throw new NotImplementedException();

	public bool? IsTypeAllowed(Type type)
	{
		if (this.IsSupportedType(type))
		{
			return true;
		}

		return false;
	}
}