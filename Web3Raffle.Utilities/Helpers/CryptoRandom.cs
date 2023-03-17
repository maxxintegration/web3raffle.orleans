﻿using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

namespace Web3raffle.Utilities.Helpers
{
	/// <summary>
	/// A random number generator based on the RNGCryptoServiceProvider.
	/// Adapted from the "Tales from the CryptoRandom" article in MSDN Magazine (September 2007)
	/// but with explicit guarantee to be thread safe. Note that this implementation also includes
	/// an optional (enabled by default) random buffer which provides a significant speed boost as
	/// it greatly reduces the amount of calls into unmanaged land.
	/// </summary>
	public class CryptoRandom : Random
	{
		private readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();

		private byte[]? _buffer;

		private int _bufferPosition;

		/// <summary>
		/// Initializes a new instance of the <see cref="CryptoRandom"/> class with.
		/// Using this overload will enable the random buffer pool.
		/// </summary>
		public CryptoRandom() : this(true) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="CryptoRandom"/> class.
		/// This method will disregard whatever value is passed as seed and it's only implemented
		/// in order to be fully backwards compatible with <see cref="Random"/>.
		/// Using this overload will enable the random buffer pool.
		/// </summary>
		/// <param name="ignoredSeed">The ignored seed.</param>
		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "ignoredSeed", Justification = "Cannot remove this parameter as we implement the full API of System.Random")]
		[SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
		public CryptoRandom(int ignoredSeed) : this(true) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="CryptoRandom"/> class with
		/// optional random buffer.
		/// </summary>
		/// <param name="enableRandomPool">set to <c>true</c> to enable the random pool buffer for increased performance.</param>
		public CryptoRandom(bool enableRandomPool)
		{
			this.IsRandomPoolEnabled = enableRandomPool;
		}

		/// <summary>
		/// Gets a value indicating whether this instance has random pool enabled.
		/// </summary>
		/// <value>
		///     <c>true</c> if this instance has random pool enabled; otherwise, <c>false</c>.
		/// </value>
		public bool IsRandomPoolEnabled { get; private set; }

		/// <summary>
		/// Returns a nonnegative random number.
		/// </summary>
		/// <returns>
		/// A 32-bit signed integer greater than or equal to zero and less than <see cref="F:System.Int32.MaxValue"/>.
		/// </returns>
		public override int Next()
		{
			// Mask away the sign bit so that we always return nonnegative integers
			return (int)this.GetRandomUInt32() & 0x7FFFFFFF;
		}

		/// <summary>
		/// Returns a nonnegative random number less than the specified maximum.
		/// </summary>
		/// <param name="maxValue">The exclusive upper bound of the random number to be generated. <paramref name="maxValue"/> must be greater than or equal to zero.</param>
		/// <returns>
		/// A 32-bit signed integer greater than or equal to zero, and less than <paramref name="maxValue"/>; that is, the range of return values ordinarily includes zero but not <paramref name="maxValue"/>. However, if <paramref name="maxValue"/> equals zero, <paramref name="maxValue"/> is returned.
		/// </returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///     <paramref name="maxValue"/> is less than zero.
		/// </exception>
		public override int Next(int maxValue)
		{
			if (maxValue < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(maxValue));
			}

			return this.Next(0, maxValue);
		}

		/// <summary>
		/// Returns a random number within a specified range.
		/// </summary>
		/// <param name="minValue">The inclusive lower bound of the random number returned.</param>
		/// <param name="maxValue">The exclusive upper bound of the random number returned. <paramref name="maxValue"/> must be greater than or equal to <paramref name="minValue"/>.</param>
		/// <returns>
		/// A 32-bit signed integer greater than or equal to <paramref name="minValue"/> and less than <paramref name="maxValue"/>; that is, the range of return values includes <paramref name="minValue"/> but not <paramref name="maxValue"/>. If <paramref name="minValue"/> equals <paramref name="maxValue"/>, <paramref name="minValue"/> is returned.
		/// </returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///     <paramref name="minValue"/> is greater than <paramref name="maxValue"/>.
		/// </exception>
		public override int Next(int minValue, int maxValue)
		{
			if (minValue > maxValue)
			{
				throw new ArgumentOutOfRangeException(nameof(minValue));
			}

			if (minValue == maxValue)
			{
				return minValue;
			}

			long diff = maxValue - minValue;

			while (true)
			{
				uint rand = this.GetRandomUInt32();

				long max = 1 + (long)uint.MaxValue;
				long remainder = max % diff;

				if (rand < max - remainder)
				{
					return (int)(minValue + rand % diff);
				}
			}
		}

		/// <summary>
		/// Returns a random number between 0.0 and 1.0.
		/// </summary>
		/// <returns>
		/// A double-precision floating point number greater than or equal to 0.0, and less than 1.0.
		/// </returns>
		public override double NextDouble()
		{
			return this.GetRandomUInt32() / (1.0 + uint.MaxValue);
		}

		/// <summary>
		/// Fills the elements of a specified array of bytes with random numbers.
		/// </summary>
		/// <param name="buffer">An array of bytes to contain random numbers.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///     <paramref name="buffer"/> is null.
		/// </exception>
		public override void NextBytes(byte[] buffer)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException(nameof(buffer));
			}

			lock (this)
			{
				if (this.IsRandomPoolEnabled && this._buffer == null)
				{
					this.InitBuffer();
				}

				// Can we fit the requested number of bytes in the buffer?
				if (this.IsRandomPoolEnabled && this._buffer!.Length <= buffer.Length)
				{
					int count = buffer.Length;

					this.EnsureRandomBuffer(count);

					Buffer.BlockCopy(this._buffer, this._bufferPosition, buffer, 0, count);

					this._bufferPosition += count;
				}
				else
				{
					// Draw bytes directly from the RNGCryptoProvider
					this._rng.GetBytes(buffer);
				}
			}
		}

		private void InitBuffer()
		{
			if (this.IsRandomPoolEnabled)
			{
				if (this._buffer == null || this._buffer.Length != 512)
				{
					this._buffer = new byte[512];
				}
			}
			else
			{
				if (this._buffer == null || this._buffer.Length != 4)
				{
					this._buffer = new byte[4];
				}
			}

			this._rng.GetBytes(this._buffer);
			this._bufferPosition = 0;
		}

		/// <summary>
		/// Gets one random unsigned 32bit integer in a thread safe manner.
		/// </summary>
		private uint GetRandomUInt32()
		{
			lock (this)
			{
				this.EnsureRandomBuffer(4);

				uint rand = BitConverter.ToUInt32(this._buffer!, this._bufferPosition);

				this._bufferPosition += 4;

				return rand;
			}
		}

		/// <summary>
		/// Ensures that we have enough bytes in the random buffer.
		/// </summary>
		/// <param name="requiredBytes">The number of required bytes.</param>
		private void EnsureRandomBuffer(int requiredBytes)
		{
			if (this._buffer == null)
			{
				this.InitBuffer();
			}

			if (requiredBytes > this._buffer!.Length)
			{
				throw new ArgumentOutOfRangeException(nameof(requiredBytes), "cannot be greater than random buffer");
			}

			if (this._buffer!.Length - this._bufferPosition < requiredBytes)
			{
				this.InitBuffer();
			}
		}
	}
}