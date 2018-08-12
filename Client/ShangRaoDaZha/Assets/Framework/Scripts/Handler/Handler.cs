using System;
using FrameworkForCSharp.NetWorks;

public class Handler
{

  // ByteBuffer
	private static Action<NetworkMessage>[] message_handlers = new Action<NetworkMessage>[1024];

    static Handler()
    {
        new ResultCodeHandler();
        new LoginHandler();
        new RoomManagerHandler();
        new MainPanelHandler();
       new  ClubInfoHander();
    }

    public static void addServerHandler(Opcodes opcode, Action<NetworkMessage> handler)
    {
        message_handlers[(UInt16)opcode] = handler;
    }
    
    public static void dispatchMessage(NetworkMessage message)
    {
        if (message.cmd < message_handlers.Length && message_handlers[message.cmd] != null)
        {
            message_handlers[message.cmd](message);
        }
        else
        {
            //Logger.info("client:{0}:{1} request unprocessed cmd:{2}, kick it!", player.name, player.guid, (Opcodes)message.cmd);
        }
    }
}



#region  库
/*
namespace FrameworkForCSharp.NetWorks
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    public class ByteBuffer : MarshalByRefObject
    {
        private byte[] _data;
        private uint readPos = 0;
        private uint writePos = 0;

        public ByteBuffer(uint size = 0x100)
        {
            this._data = new byte[size];
        }

        public void clear()
        {
            this.writePos = this.readPos = 0;
        }

        public uint getUInt32(uint index)
        {
            if ((index + 4) > this._data.Length)
            {
                throw new ByteBufferException("invalid data");
            }
            return BitConverter.ToUInt32(this._data, (int)index);
        }

        public ulong getUInt64(uint index)
        {
            if ((index + 8) > this._data.Length)
            {
                throw new ByteBufferException("invalid data");
            }
            return BitConverter.ToUInt64(this._data, (int)index);
        }

        public bool readBool()
        {
            bool flag = BitConverter.ToBoolean(this._data, (int)this.readPos);
            this.readPos++;
            return flag;
        }

        public void readBytes(byte[] val, uint start = 0, uint length = 0xffffffff)
        {
            if (length == uint.MaxValue)
            {
                length = (uint)val.Length;
            }
            if ((this.readPos + length) > this._data.Length)
            {
                throw new ByteBufferException("invalid data");
            }
            byte[] buffer = new byte[length];
            Array.Copy(this._data, (long)this.readPos, val, (long)start, (long)length);
            this.readPos += length;
        }

        public char readChar()
        {
            char ch = BitConverter.ToChar(this._data, (int)this.readPos);
            this.readPos += 2;
            return ch;
        }

        public float readFloat()
        {
            if ((this.readPos + 4) > this._data.Length)
            {
                throw new ByteBufferException("invalid data");
            }
            float num = BitConverter.ToSingle(this._data, (int)this.readPos);
            this.readPos += 4;
            return num;
        }

        public short readInt16()
        {
            short num = BitConverter.ToInt16(this._data, (int)this.readPos);
            this.readPos += 2;
            return num;
        }

        public int readInt32()
        {
            int num = BitConverter.ToInt32(this._data, (int)this.readPos);
            this.readPos += 4;
            return num;
        }

        public long readInt64()
        {
            long num = BitConverter.ToInt64(this._data, (int)this.readPos);
            this.readPos += 8;
            return num;
        }

        public string readString()
        {
            int count = 0;
            while ((this.readPos < this._data.Length) && (this._data[this.readPos] > 0))
            {
                count++;
            }
            return Encoding.UTF8.GetString(this._data, (((int)this.readPos) - count) - 1, count);
        }

        public ushort readUInt16()
        {
            ushort num = BitConverter.ToUInt16(this._data, (int)this.readPos);
            this.readPos += 2;
            return num;
        }

        public uint readUInt32()
        {
            uint num = BitConverter.ToUInt32(this._data, (int)this.readPos);
            this.readPos += 4;
            return num;
        }

        public ulong readUInt64()
        {
            ulong num = BitConverter.ToUInt64(this._data, (int)this.readPos);
            this.readPos += 8;
            return num;
        }

        public byte readUInt8()
        {
            byte num = this._data[this.readPos];
            this.readPos++;
            return num;
        }

        public void setUInt16(uint index, ushort value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (this.writePos > (this._data.Length - bytes.Length))
            {
                Array.Resize<byte>(ref this._data, (this._data.Length + bytes.Length) << 1);
            }
            Array.Copy(bytes, 0L, this._data, (long)index, (long)bytes.Length);
        }

        public void setUInt32(uint index, uint value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (this.writePos > (this._data.Length - bytes.Length))
            {
                Array.Resize<byte>(ref this._data, (this._data.Length + bytes.Length) << 1);
            }
            Array.Copy(bytes, 0L, this._data, (long)index, (long)bytes.Length);
        }

        public void setUInt8(uint index, byte value)
        {
            byte[] sourceArray = new byte[] { value };
            if (this.writePos > (this._data.Length - sourceArray.Length))
            {
                Array.Resize<byte>(ref this._data, (this._data.Length + sourceArray.Length) << 1);
            }
            Array.Copy(sourceArray, 0L, this._data, (long)index, (long)sourceArray.Length);
        }

        public void writeBool(bool value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (this.writePos > (this._data.Length - bytes.Length))
            {
                Array.Resize<byte>(ref this._data, (this._data.Length + bytes.Length) << 1);
            }
            Array.Copy(bytes, 0L, this._data, (long)this.writePos, (long)bytes.Length);
            this.writePos += (uint)bytes.Length;
        }

        public void writeBytes(byte[] val, uint start = 0, uint length = 0xffffffff)
        {
            if (length == uint.MaxValue)
            {
                length = ((uint)val.Length) - start;
            }
            if (this.writePos > (this._data.Length - length))
            {
                Array.Resize<byte>(ref this._data, (int)((this._data.Length + length) << 1));
            }
            Array.Copy(val, (long)start, this._data, (long)this.writePos, (long)length);
            this.writePos += length;
        }

        public void writeChar(char value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (this.writePos > (this._data.Length - bytes.Length))
            {
                Array.Resize<byte>(ref this._data, (this._data.Length + bytes.Length) << 1);
            }
            Array.Copy(bytes, 0L, this._data, (long)this.writePos, (long)bytes.Length);
            this.writePos += (uint)bytes.Length;
        }

        public void writeFloat(float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (this.writePos > (this._data.Length - bytes.Length))
            {
                Array.Resize<byte>(ref this._data, (this._data.Length + bytes.Length) << 1);
            }
            Array.Copy(bytes, 0L, this._data, (long)this.writePos, (long)bytes.Length);
            this.writePos += (uint)bytes.Length;
        }

        public void writeInt16(short value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (this.writePos > (this._data.Length - bytes.Length))
            {
                Array.Resize<byte>(ref this._data, (this._data.Length + bytes.Length) << 1);
            }
            Array.Copy(bytes, 0L, this._data, (long)this.writePos, (long)bytes.Length);
            this.writePos += (uint)bytes.Length;
        }

        public void writeInt32(int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (this.writePos > (this._data.Length - bytes.Length))
            {
                Array.Resize<byte>(ref this._data, (this._data.Length + bytes.Length) << 1);
            }
            Array.Copy(bytes, 0L, this._data, (long)this.writePos, (long)bytes.Length);
            this.writePos += (uint)bytes.Length;
        }

        public void writeInt64(long value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (this.writePos > (this._data.Length - bytes.Length))
            {
                Array.Resize<byte>(ref this._data, (this._data.Length + bytes.Length) << 1);
            }
            Array.Copy(bytes, 0L, this._data, (long)this.writePos, (long)bytes.Length);
            this.writePos += (uint)bytes.Length;
        }

        public void writeString(string val)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(val);
            this.writeBytes(bytes, 0, uint.MaxValue);
            this.writeUInt8(0);
        }

        public void writeUInt16(ushort value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (this.writePos > (this._data.Length - bytes.Length))
            {
                Array.Resize<byte>(ref this._data, (this._data.Length + bytes.Length) << 1);
            }
            Array.Copy(bytes, 0L, this._data, (long)this.writePos, (long)bytes.Length);
            this.writePos += (uint)bytes.Length;
        }

        public void writeUInt32(uint value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (this.writePos > (this._data.Length - bytes.Length))
            {
                Array.Resize<byte>(ref this._data, (this._data.Length + bytes.Length) << 1);
            }
            Array.Copy(bytes, 0L, this._data, (long)this.writePos, (long)bytes.Length);
            this.writePos += (uint)bytes.Length;
        }

        public void writeUInt64(ulong value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (this.writePos > (this._data.Length - bytes.Length))
            {
                Array.Resize<byte>(ref this._data, (this._data.Length + bytes.Length) << 1);
            }
            Array.Copy(bytes, 0L, this._data, (long)this.writePos, (long)bytes.Length);
            this.writePos += (uint)bytes.Length;
        }

        public void writeUInt8(byte value)
        {
            byte[] sourceArray = new byte[] { value };
            if (this.writePos > (this._data.Length - sourceArray.Length))
            {
                Array.Resize<byte>(ref this._data, (this._data.Length + sourceArray.Length) << 1);
            }
            Array.Copy(sourceArray, 0L, this._data, (long)this.writePos, (long)sourceArray.Length);
            this.writePos += (uint)sourceArray.Length;
        }

        public byte[] data
        {
            get
            {
                return this._data;
            }
        }
           

        public uint rpos
        {
            get
            {
                return this.readPos;
            }
        }
          
        public uint wpos
        {
            get
            {
                return this.writePos;
            }
        }
    }
}

namespace FrameworkForCSharp.NetWorks
{
    using System;
    using System.Runtime.InteropServices;

    public class AuthConnectionClient : Connection
    {
        private string m_AuthKey;

        public AuthConnectionClient(string key = "FU HAI LANG IS GOD!")
        {
            this.m_AuthKey = key;
            base.addHandler(1, new Action<NetworkMessage>(this.OnAuthAck));
        }

        private void OnAuthAck(NetworkMessage message)
        {
            this.OnAuthSuccessed();
        }

        protected virtual void OnAuthSuccessed()
        {
        }

        protected override void OnConnected()
        {
            NetworkMessage message = NetworkMessage.Create(0, (uint)(this.m_AuthKey.Length + 1));
            message.writeString(this.m_AuthKey);
            base.send(message, uint.MaxValue);
        }
    }
}

namespace FrameworkForCSharp.NetWorks
{
    using System;
    using System.Net.Sockets;
    using System.Runtime.InteropServices;

    public class AuthConnectionServer : Connection
    {
        private string m_AuthKey;

        public AuthConnectionServer(string key = "FU HAI LANG IS GOD!")
        {
            this.m_AuthKey = key;
            base.addHandler(0, new Action<NetworkMessage>(this.OnAuth));
        }

        private void OnAuth(NetworkMessage message)
        {
            string str = message.readString();
            if (this.m_AuthKey != str)
            {
                this.OnAuthResult(false);
                base.close();
            }
            else
            {
                this.OnAuthResult(true);
                NetworkMessage message2 = NetworkMessage.Create(1, 0);
                base.send(message2, uint.MaxValue);
            }
        }

        protected virtual void OnAuthResult(bool success)
        {
        }

        protected sealed override void onDataReceived(SocketAsyncEventArgs e)
        {
            base.onDataReceived(e);
        }
    }
}

namespace FrameworkForCSharp.NetWorks
{
    using System;
    using System.Collections.Generic;
    using System.Net.Sockets;

    public class BufferManager
    {
        private byte[] buffer;
        private int bufferSize;
        private Stack<int> pool;
        private int poolIndex;
        private int totalBytes;

        public BufferManager(int totalBytes, int bufferSize)
        {
            this.totalBytes = totalBytes;
            this.bufferSize = bufferSize;
            this.pool = new Stack<int>();
            this.buffer = new byte[totalBytes];
        }

        public void freeBuffer(SocketAsyncEventArgs args)
        {
            this.pool.Push(args.Offset);
            args.SetBuffer(null, 0, 0);
        }

        public bool setBuffer(SocketAsyncEventArgs args)
        {
            if (this.pool.Count > 0)
            {
                args.SetBuffer(this.buffer, this.pool.Pop(), this.bufferSize);
            }
            else
            {
                if ((this.totalBytes - this.bufferSize) < this.poolIndex)
                {
                    return false;
                }
                args.SetBuffer(this.buffer, this.poolIndex, this.bufferSize);
                this.poolIndex += this.bufferSize;
            }
            return true;
        }
    }
}

namespace FrameworkForCSharp.NetWorks
{
    using System;

    public class ByteBufferException : Exception
    {
        public ByteBufferException(string message) : base(message)
        {
        }
    }
}

namespace FrameworkForCSharp.NetWorks
{
    using FrameworkForCSharp.Utils;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;

    public class Connection
    {
        public  Socket _socket;
        //[CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        //public int bufferOffset;
        //[CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        //public int bufferSize;
        //[CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        //public uint id;
        //[CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        //public SocketMaster master;
        //[CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        //public bool protocolSecurity;
        //[CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        //public SocketAsyncEventArgs readEventArgs;
        //[CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public SocketAsyncEventArgs sendEventArgs;
        public static List<Action> actions = new List<Action>();
        public NetworkMessage cacheMessage;
        public bool closed = false;
        public bool connected = false;
        public static short crypt_offset = 4;
        public bool ForWebSocket = false;
        public Action<NetworkMessage>[] handlers = new Action<NetworkMessage>[0x400];
        public byte[] headBuffer = new byte[NetworkMessage.HEAD_LEN];
        public int headOffset = 0;
        private static ConnectionProcessInfo[] infos = new ConnectionProcessInfo[0x400];
        public bool isWebSocketHandShaked = false;
        public static uint[] msgDispatchedCount = new uint[0x400];
        public const int OpcodeMax = 0x400;
        public static List<Action> pendingActions = new List<Action>();
        public bool receiving = false;
        public bool released = false;
        public ByteBuffer sendHeadBuffer = new ByteBuffer((uint)NetworkMessage.HEAD_LEN);
        public bool sending = false;
        public NetworkMessage sendingMessage;
        public int sendingMessageOffset = 0;
        public Queue<NetworkMessage> toSendMessages = new Queue<NetworkMessage>();
        private static ConnectionProcessInfo[] updateInfos = new ConnectionProcessInfo[0x400];

        public Connection()
        {
            this.readEventArgs = new SocketAsyncEventArgs();
            this.sendEventArgs = new SocketAsyncEventArgs();
            this.readEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(this.io_read_completed);
            this.sendEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(this.io_send_completed);
        }

        private void _dispatch(NetworkMessage message)
        {
            try
            {
                if (message.cmd < this.handlers.Length)
                {
                    if (delayDispatch(message.cmd))
                    {
                        pendingActions.Add(() => this._dispatch(message));
                    }
                    else
                    {
                        DateTime now = DateTime.Now;
                        Action<NetworkMessage> action = this.handlers[message.cmd];
                        if (action !=null)
                        {
                            action(message);
                        }
                        else
                        {
                            this.DefaultHandleMessage(message);
                        }
                        DateTime time2 = DateTime.Now;
                        addProcessCount(message.cmd, 1);
                        addProcessTime(message.cmd, (TimeSpan)(time2 - now));
                    }
                }
            }
            catch (ByteBufferException)
            {
                this.OnInvalidMessage(message);
            }
            catch (ObjectDisposedException)
            {
            }
        }

        public void addHandler(ushort opcode, Action<NetworkMessage> handler)
        {
            this.handlers[opcode] = handler;
        }

        private static void addProcessCount(ushort cmd, uint count = 1)
        {
            if (validOpcode(cmd))
            {
                checkrec(cmd);
                infos[cmd].addProcessCount(count);
                updateInfos[cmd].addProcessCount(count);
            }
        }

        private static void addProcessTime(ushort cmd, TimeSpan span)
        {
            if (validOpcode(cmd))
            {
                checkrec(cmd);
                infos[cmd].addProcessTime(span);
                updateInfos[cmd].addProcessTime(span);
            }
        }

        private static void checkrec(ushort cmd)
        {
            checkRecExist(cmd, infos);
            checkRecExist(cmd, updateInfos);
        }

        private static void checkRecExist(ushort cmd, ConnectionProcessInfo[] array)
        {
            if (array[cmd] == null)
            {
                array[cmd] = new ConnectionProcessInfo(cmd);
            }
        }

        public void close()
        {
            this.closeClient(null, SocketShutdown.Both, 1);
        }

        private void closeClient(SocketAsyncEventArgs e, SocketShutdown type = SocketShutdown.Both, int debugSkip = 0)
        {
            if (this.connected)
            {
                this.closed = true;
                try
                {
                    this.socket.Shutdown(type);
                }
                catch (Exception)
                {
                }
                finally
                {
                    this.socket.Close();
                }
                this.stop();
            }
        }

        public virtual void decrypt(byte[] data, int offset, int len)
        {
        }

        protected virtual void DefaultHandleMessage(NetworkMessage message)
        {
        }

        private static bool delayDispatch(ushort opcode)
        {
          return ((msgDispatchedCount[opcode] > 0) && (getProcessCount(opcode) >= msgDispatchedCount[opcode]));
          //  return false;
        }


        private void dispatch(NetworkMessage message)
        {
            if (this.protocolSecurity && !message.valid())
            {
                this.close();
            }
            else
            {
                List<Action> actions = Connection.actions;
                lock (actions)
                {
                    Connection.actions.Add(() => this._dispatch(message));
                }
            }
        }

        public static bool empty()
            {
        return ((actions.Count == 0) && (pendingActions.Count == 0));
        }
            

        public virtual void encrypt(byte[] data, int offset, int len)
        {
        }

        internal static void endUpdate(bool logProcessed)
        {
            if (logProcessed)
            {
                logInfo(updateInfos);
            }
        }

        ~Connection()
        {
        }

        private static uint getProcessCount(ushort cmd)
        {
            if (!validOpcode(cmd) || (updateInfos[cmd] == null))
            {
                return 0;
            }
            return updateInfos[cmd].processedCount;
        }

        private void io_read_completed(object sender, SocketAsyncEventArgs e)
        {
            this.process_receive(e);
        }

        private void io_send_completed(object sender, SocketAsyncEventArgs e)
        {
            this.process_send(e);
        }

        private static void logInfo(ConnectionProcessInfo[] array)
        {
        }

        internal static void logSummary()
        {
            logInfo(infos);
        }

        protected virtual void OnConnected()
        {
        }

        protected virtual void onDataReceived(SocketAsyncEventArgs e)
        {
            byte[] buffer;
            int srcOffset = 0;
            byte[] destinationArray = new byte[e.BytesTransferred];
            Array.Copy(e.Buffer, e.Offset, destinationArray, 0, e.BytesTransferred);
            if (this.ForWebSocket)
            {
                if (!this.isWebSocketHandShaked && Encoding.UTF8.GetString(destinationArray, 0, e.BytesTransferred).Contains("Sec-WebSocket-Key"))
                {
                    this.socket.Send(mySingleton<myFunction>.Instance.PackageHandShakeData(destinationArray, e.BytesTransferred));
                    this.isWebSocketHandShaked = true;
                    return;
                }
                buffer = mySingleton<myFunction>.Instance.AnalyzeClientData(destinationArray, e.BytesTransferred);
            }
            else
            {
                buffer = destinationArray;
            }
            int length = buffer.Length;
            while (length > 0)
            {
                if (this.cacheMessage != null)
                {
                    uint num5 = this.cacheMessage.size - this.cacheMessage.wpos;
                    if (length >= num5)
                    {
                        this.cacheMessage.writeBytes(buffer, (uint)srcOffset, num5);
                        length -= (int)num5;
                        srcOffset += (int)num5;
                        this.dispatch(this.cacheMessage);
                        this.cacheMessage = null;
                    }
                    else
                    {
                        this.cacheMessage.writeBytes(buffer, (uint)srcOffset, (uint)length);
                        length = 0;
                        srcOffset = 0;
                        break;
                    }
                }
                int count = this.headBuffer.Length - this.headOffset;
                if (length >= count)
                {
                    Buffer.BlockCopy(buffer, srcOffset, this.headBuffer, this.headOffset, count);
                    this.headOffset = 0;
                    length -= count;
                    srcOffset += count;
                    if (this.protocolSecurity)
                    {
                        this.decrypt(this.headBuffer, crypt_offset, NetworkMessage.HEAD_LEN);
                    }
                    uint size = BitConverter.ToUInt32(this.headBuffer, 0);
                    ushort cmd = BitConverter.ToUInt16(this.headBuffer, 4);
                    byte crc = this.headBuffer[6];
                    if ((cmd > 0x1d) && ((size < 0) || (size > 0x30d40)))
                    {
                        this.closeClient(null, SocketShutdown.Both, 0);
                        break;
                    }
                    if (length < size)
                    {
                        this.cacheMessage = NetworkMessage.Create(cmd, size, crc);
                        continue;
                    }
                    NetworkMessage message = NetworkMessage.Create(cmd, size, crc);
                    if (size > 0)
                    {
                        message.writeBytes(buffer, (uint)srcOffset, size);
                        length -= (int)size;
                        srcOffset += (int)size;
                    }
                    this.dispatch(message);
                    if (!this.socket.Connected)
                    {
                        break;
                    }
                }
                else
                {
                    Buffer.BlockCopy(buffer, srcOffset, this.headBuffer, this.headOffset, length);
                    this.headOffset += length;
                    length = 0;
                    srcOffset = 0;
                    break;
                }
            }
        }

        protected virtual void OnDisconnected()
        {
        }

        protected virtual void OnInvalidMessage(NetworkMessage message)
        {
        }

        private void process_receive(SocketAsyncEventArgs e)
        {
            this.receiving = false;
            if (!this.connected)
            {
                if (!this.released && (!this.receiving && !this.sending))
                {
                    this.master.notifyRelease(this, "receive");
                }
            }
            else if ((e.BytesTransferred > 0) && (e.SocketError == SocketError.Success))
            {
                this.onDataReceived(e);
                if (this.socket.Connected)
                {
                    this.startRead();
                }
            }
            else
            {
                this.closeClient(e, SocketShutdown.Receive, 0);
            }
        }

        private void process_send(SocketAsyncEventArgs e)
        {
            if (!this.connected)
            {
                this.sending = false;
                if (!this.released && (!this.receiving && !this.sending))
                {
                    this.master.notifyRelease(this, "send");
                }
            }
            else if (e.SocketError == SocketError.Success)
            {
                try
                {
                    if (e.BytesTransferred != e.Count)
                    {
                        this.sending = true;
                        Buffer.BlockCopy(e.Buffer, e.BytesTransferred, e.Buffer, this.sendEventArgs.Offset, e.Count - e.BytesTransferred);
                        this.sendEventArgs.SetBuffer(this.sendEventArgs.Offset, e.Count - e.BytesTransferred);
                        if (!this.socket.SendAsync(this.sendEventArgs))
                        {
                            this.process_send(this.sendEventArgs);
                        }
                    }
                    else
                    {
                        Queue<NetworkMessage> toSendMessages = this.toSendMessages;
                        lock (toSendMessages)
                        {
                            if (this.toSendMessages.Count > 0)
                            {
                                this.sendMessage();
                            }
                            else
                            {
                                this.sending = false;
                            }
                        }
                    }
                }
                catch (ObjectDisposedException)
                {
                    this.sending = false;
                }
            }
            else
            {
                this.sending = false;
                this.closeClient(e, SocketShutdown.Send, 0);
            }
        }

        internal void renew()
        {
            this.sendingMessage = null;
            this.sendingMessageOffset = 0;
            this.toSendMessages.Clear();
            this.cacheMessage = null;
            this.headOffset = 0;
        }

        public void send(NetworkMessage message, uint size = 0xffffffff)
        {
            Queue<NetworkMessage> toSendMessages = this.toSendMessages;
            lock (toSendMessages)
            {
                if (!this.connected)
                {
                    return;
                }
            }
            NetworkMessage item = message;
            if (message.used)
            {
                item = NetworkMessage.Create(message.cmd, message.wpos);
                item.writeBytes(message.data, 0, message.wpos);
            }
            item.used = true;
            if (size == uint.MaxValue)
            {
                item.size = item.wpos;
            }
            else
            {
                item.size = size;
            }
            if ((item.cmd > 0x1d) && ((item.size < 0) || (item.size > 0x30d40)))
            {
                throw new ArgumentException("invalid message size");
            }
            item.build();
            Queue<NetworkMessage> queue2 = this.toSendMessages;
            lock (queue2)
            {
                this.toSendMessages.Enqueue(item);
                if (!this.sending)
                {
                    try
                    {
                        this.sendMessage();
                    }
                    catch (ObjectDisposedException)
                    {
                        this.sending = false;
                    }
                }
            }
        }

        private void sendMessage()
        {
            NetworkMessage message = null;
            if ((this.toSendMessages.Count != 0) && this.connected)
            {
                message = this.toSendMessages.Peek();
                this.sending = true;
                this.sendEventArgs.SetBuffer(this.bufferOffset, this.bufferSize);
                int num = 0;
                int count = this.bufferSize - num;
                if (this.sendingMessageOffset > 0)
                {
                    int num3 = ((int)message.size) - this.sendingMessageOffset;
                    if (num3 <= count)
                    {
                        Buffer.BlockCopy(message.data, this.sendingMessageOffset, this.sendEventArgs.Buffer, this.sendEventArgs.Offset + num, num3);
                        this.sendEventArgs.SetBuffer(this.sendEventArgs.Offset, num3);
                        this.sendingMessage = null;
                        this.sendingMessageOffset = 0;
                    }
                    else
                    {
                        Buffer.BlockCopy(message.data, this.sendingMessageOffset, this.sendEventArgs.Buffer, this.sendEventArgs.Offset + num, count);
                        this.sendEventArgs.SetBuffer(this.sendEventArgs.Offset, count);
                        this.sendingMessageOffset += count;
                    }
                }
                else if (message.cmd == 0xffff)
                {
                    uint size = message.size;
                    Buffer.BlockCopy(message.data, 0, this.sendEventArgs.Buffer, this.sendEventArgs.Offset, (int)size);
                    this.sendEventArgs.SetBuffer(this.sendEventArgs.Offset, (int)size);
                }
                else
                {
                    this.sendHeadBuffer.setUInt32(0, message.size);
                    this.sendHeadBuffer.setUInt16(4, message.cmd);
                    this.sendHeadBuffer.setUInt8(6, message.crc);
                    this.encrypt(this.sendHeadBuffer.data, crypt_offset, NetworkMessage.HEAD_LEN);
                    Buffer.BlockCopy(this.sendHeadBuffer.data, 0, this.sendEventArgs.Buffer, this.sendEventArgs.Offset, NetworkMessage.HEAD_LEN);
                    count -= NetworkMessage.HEAD_LEN;
                    if (message.size <= count)
                    {
                        Buffer.BlockCopy(message.data, this.sendingMessageOffset, this.sendEventArgs.Buffer, (this.sendEventArgs.Offset + num) + NetworkMessage.HEAD_LEN, (int)message.size);
                        if (this.ForWebSocket)
                        {
                            byte[] destinationArray = new byte[NetworkMessage.HEAD_LEN + message.size];
                            Array.Copy(this.sendEventArgs.Buffer, this.sendEventArgs.Offset, destinationArray, 0, destinationArray.Length);
                            byte[] src = mySingleton<myFunction>.Instance.PackageServerData(destinationArray);
                            Buffer.BlockCopy(src, this.sendingMessageOffset, this.sendEventArgs.Buffer, this.sendEventArgs.Offset, src.Length);
                            this.sendEventArgs.SetBuffer(this.sendEventArgs.Offset, src.Length);
                        }
                        else
                        {
                            this.sendEventArgs.SetBuffer(this.sendEventArgs.Offset, NetworkMessage.HEAD_LEN + ((int)message.size));
                        }
                    }
                    else
                    {
                        Buffer.BlockCopy(message.data, this.sendingMessageOffset, this.sendEventArgs.Buffer, (this.sendEventArgs.Offset + num) + NetworkMessage.HEAD_LEN, count);
                        this.sendEventArgs.SetBuffer(this.sendEventArgs.Offset, NetworkMessage.HEAD_LEN + count);
                        this.sendingMessageOffset += count;
                        this.sendingMessage = message;
                    }
                }
                if (this.sendingMessageOffset == 0)
                {
                    if (this.toSendMessages.Count == 0)
                    {
                        return;
                    }
                    this.toSendMessages.Dequeue();
                }
                if (!this.socket.SendAsync(this.sendEventArgs))
                {
                    this.process_send(this.sendEventArgs);
                }
            }
        }

        private void sendMessage(NetworkMessage message)
        {
            try
            {
                if (message.cmd == 0xffff)
                {
                    int num = this.socket.Send(message.data, 0, (int)message.size, SocketFlags.None);
                }
                else
                {
                    int num2;
                    this.sendHeadBuffer.setUInt32(0, message.size);
                    this.sendHeadBuffer.setUInt16(4, message.cmd);
                    this.sendHeadBuffer.setUInt8(6, message.crc);
                    if (this.protocolSecurity)
                    {
                        this.encrypt(this.sendHeadBuffer.data, crypt_offset, NetworkMessage.HEAD_LEN);
                    }
                    for (num2 = 0; num2 < NetworkMessage.HEAD_LEN; num2 += this.socket.Send(this.sendHeadBuffer.data, num2, NetworkMessage.HEAD_LEN - num2, SocketFlags.None))
                    {
                    }
                    for (num2 = 0; num2 < message.size; num2 += this.socket.Send(message.data, num2, ((int)message.size) - num2, SocketFlags.None))
                    {
                    }
                }
            }
            catch (ObjectDisposedException)
            {
            }
            catch (SocketException)
            {
                this.closeClient(null, SocketShutdown.Send, 0);
            }
        }

        protected static void setMessageDispatchThreshold(ushort opcode, uint countPerCycle)
        {
            msgDispatchedCount[opcode] = countPerCycle;
        }

        internal void start()
        {
            List<Action> actions = Connection.actions;
            lock (actions)
            {
                Connection.actions.Add(new Action(this.OnConnected));
                this.connected = true;
                this.master.notifyAdd(this);
            }
            this.startRead();
        }

        private void startRead()
        {
            this.receiving = true;
            try
            {
                this.readEventArgs.SetBuffer(this.readEventArgs.Offset, this.bufferSize);
                if (!this.socket.ReceiveAsync(this.readEventArgs))
                {
                    this.process_receive(this.readEventArgs);
                }
            }
            catch (Exception)
            {
                this.close();
            }
        }

        internal static void startUpdate()
        {
            for (int i = 0; i < updateInfos.Length; i++)
            {
                updateInfos[i] = null;
            }
        }

        private void stop()
        {
            List<Action> actions = Connection.actions;
            lock (actions)
            {
                if (this.connected)
                {
                    this.connected = false;
                    Connection.actions.Add(() => this.OnDisconnected());
                    if ((!this.receiving && !this.sending) && !this.released)
                    {
                        this.master.notifyRelease(this, "stop");
                    }
                }
            }
        }

        public override string ToString()
        {
            // $"[id=>{this.id}]";
            return this.id.ToString();
        }
          

        public static void update()
        {
            Action[] actionArray;
            List<Action> actions = Connection.actions;
            lock (actions)
            {
                actionArray = Connection.actions.ToArray();
                Connection.actions.Clear();
            }
            foreach (Action action in actionArray)
            {
                action();
            }
            actionArray = pendingActions.ToArray();
            pendingActions.Clear();
            foreach (Action action2 in actionArray)
            {
                action2();
            }
        }

        private static bool validOpcode(ushort cmd)
        {
            if (cmd >= 0x400)
            {
                return false;
            }
            return true;
        }

        public int bufferOffset { get; set; }

        public int bufferSize { get; set; }

        public uint id { get; set; }

        public SocketMaster master { get; set; }

        public bool protocolSecurity { get; set; }

        public SocketAsyncEventArgs readEventArgs { get; set; }

        public string remoteEndPoint
        {
            get
            {
                try
                {
                    return ((this._socket != null) ? this._socket.RemoteEndPoint.ToString() : "n/a");
                }
                catch (Exception)
                {
                }
                return "n/a";
            }
        }

      //  public SocketAsyncEventArgs sendEventArgs { get; set; }

        protected internal Socket socket
        {
            get
            {
                return this._socket; ;
            }
               
            set
            {
                this._socket = value;
            }
            }
        }
    }



    namespace FrameworkForCSharp.NetWorks
    {
        using System;
        using System.Reflection;
        using System.Threading;

        public abstract class ConnectionManager : SocketMaster
        {
            protected ConnectionPool connectionPool;
            private Connection[] connections;
            private uint id;
            protected Semaphore maxNumAcceptedClients;
            private BufferManager readBufManager;
            private BufferManager sendBufManager;

            protected ConnectionManager()
            {
            }

            public void initPool<T>(int maxNumConnections, int bufSize) where T : Connection, new()
            {
                this.connections = new Connection[maxNumConnections];
                this.maxNumAcceptedClients = new Semaphore(maxNumConnections, maxNumConnections);
                this.connectionPool = new ConnectionPool(maxNumConnections);
                this.readBufManager = new BufferManager(bufSize * maxNumConnections, bufSize);
                this.sendBufManager = new BufferManager(bufSize * maxNumConnections, bufSize);
                for (int i = 0; i < maxNumConnections; i++)
                {
                    T local1 = Activator.CreateInstance<T>();
                    local1.bufferSize = bufSize;
                    uint id = this.id;
                    this.id = id + 1;
                    local1.id = id;
                    T item = local1;
                    this.readBufManager.setBuffer(item.readEventArgs);
                    this.sendBufManager.setBuffer(item.sendEventArgs);
                    item.bufferOffset = item.sendEventArgs.Offset;
                    this.connectionPool.push(item);
                }
            }

            public void notifyAdd(Connection conn)
            {
                conn.released = false;
                this.connections[conn.id] = conn;
            }

            public void notifyRelease(Connection conn, string source)
            {
                bool flag = this.connectionPool.push(conn);
                this.connections[conn.id] = null;
                this.maxNumAcceptedClients.Release();
            }

            public void visit(Func<Connection, bool> handler)
            {
                foreach (Connection connection in this.connections)
                {
                    if ((connection != null) && !handler(connection))
                    {
                        break;
                    }
                }
            }

            public uint connectionNum
            {
                get
                {
                    uint num = 0;
                    foreach (Connection connection in this.connections)
                    {
                        if (connection != null)
                        {
                            num++;
                        }
                    }
                    return num;
                }
            }

            public Connection this[uint id]
        {
            get
            {
                return this.connections[id];
            }
        }
               
        }
    }

    namespace FrameworkForCSharp.NetWorks
    {
        using System;
        using System.Collections.Generic;

        public class ConnectionPool
        {
            private Queue<Connection> pool;

            public ConnectionPool(int capacity)
            {
                this.pool = new Queue<Connection>(capacity);
            }

            public Connection pop()
            {
                if (this.pool.Count < 1)
                {
                }
                Queue<Connection> pool = this.pool;
                lock (pool)
                {
                    return this.pool.Dequeue();
                }
            }

            public bool push(Connection item)
            {
                Queue<Connection> pool = this.pool;
                lock (pool)
                {
                    if (item.released)
                    {
                        return false;
                    }
                    item.released = true;
                    this.pool.Enqueue(item);
                }
                return true;
            }
        }
    }

    namespace FrameworkForCSharp.NetWorks
    {
        using System;
        using System.Diagnostics;
        using System.Runtime.CompilerServices;
        using System.Runtime.InteropServices;

        internal class ConnectionProcessInfo
        {
        //    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        //    private ushort opcode;
        //[CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        //    private uint processedCount;
        //[CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        //    private TimeSpan processedTime;

            public ConnectionProcessInfo(ushort code)
            {
                this.opcode = code;
                this.processedTime = TimeSpan.FromSeconds(0.0);
            }

            public void addProcessCount(uint num = 1)
            {
                this.processedCount += num;
            }

            public void addProcessTime(TimeSpan span)
            {
                this.processedTime += span;
            }

        public override string ToString()
        {
            return "";
           // return $"[{this.opcode}] processed count:	{this.processedCount} used:	{this.processedTime.TotalMilliseconds} averageTime:	{((long)(this.processedTime.TotalMilliseconds / ((double)this.processedCount)))}ms    ";
        }
              

            public ushort opcode { get; private set; }

            public uint processedCount { get; private set; }

            public TimeSpan processedTime { get; private set; }
        }
    }

    namespace FrameworkForCSharp.NetWorks
    {
        using System;

        public class NetworkMessage : ByteBuffer
        {
            public ushort cmd;
            public byte crc;
            public static int HEAD_LEN = 7;
            public uint size;
            public bool used;

            protected NetworkMessage(ushort cmd, uint size) : base(size)
            {
                this.crc = 0;
                this.used = false;
                this.cmd = cmd;
                this.size = size;
            }

            public void build()
            {
            }

        public static NetworkMessage Create(ushort cmd, uint size)
        {
            return new NetworkMessage(cmd, size);
        }


        public static NetworkMessage Create(ushort cmd, uint size, byte crc)
        {
            return new NetworkMessage(cmd, size) { crc = crc };
        }


        public bool valid()
        {
            return true;
        }
               

            public void writeUInt8(bool p)
            {
                throw new NotImplementedException();
            }
        }
    }

  

        public enum Opcodes : ushort
        {
            ApplePushMessage = 0x19,
            AUTH_ACK = 1,
            AUTH_REQ = 0,
            Begin = 0,
            Character_Delete = 0x13,
            Character_Load = 0x11,
            Character_Load_Ack = 0x12,
            Character_Save = 0x10,
            Client_AgentCreateXYQPRoom = 0xa4,
            Client_Begin = 100,
            Client_BombPay_Query = 0x8f,
            Client_Character_Create = 0x71,
            Client_Character_Login = 0x72,
            Client_Character_Recharge = 0xa9,
            Client_Check_AuthCode = 0x68,
            Client_End = 0xb0,
            Client_ExchangDiamondToCard = 0xa7,
            Client_GetAgentCreateXYQPRoomList = 0xa5,
            Client_GetClientMask = 0x9a,
            Client_Lottery_Query = 150,
            Client_PlayerAskLandlordResult = 160,
            Client_PlayerBaoTingOutMagic = 0x9b,
            Client_PlayerCreateLandlordRoom = 0x9f,
            Client_PlayerCreateRoomList = 0x7b,
            Client_PlayerCreateSXMJRoom = 0x7e,
            Client_PlayerCreateTBZRoom = 0x7f,
            Client_PlayerCreateXYQPRoom = 0x91,
            Client_PlayerDealQueryLeaveResult = 130,
            Client_PlayerDetail_Info = 0x75,
            Client_PlayerDisposeRoom = 0x85,
            Client_PlayerDownChips = 0x94,
            Client_PlayerEnterRoom = 0x80,
            Client_PlayerLeaveRoom = 0x81,
            Client_PlayerOnForce = 0x84,
            Client_PlayerOperate = 0x83,
            Client_PlayerOutCards = 0xa1,
            Client_PlayerReady = 0x87,
            Client_PlayerReadyStart = 0x98,
            Client_PlayerSameIpAgreeGame = 0x99,
            Client_PlayerSetFangPaoScore = 0x92,
            Client_PlayerSpeak = 0x86,
            Client_PlayerStartGame = 0x95,
            Client_PlayerTuoGuan = 0xa2,
            Client_Query_AuthCode = 0x66,
            Client_Room_Record = 0x77,
            Client_Room_Record_Detail = 120,
            Client_RoomRecordBase = 0x7c,
            Client_RoomRecordDetail = 0x7d,
            Client_SendMoney = 0x8e,
            Client_SetInviteGuid = 0xaf,
            Client_ShareSuccess = 0x9c,
            Client_Versoin_Query = 0x6c,
            Client_WantMoney = 0xa3,
            Forget_Passward_Request = 0x69,
            Gate_Unique_Add = 14,
            Gate_Unique_Remove = 15,
            GM_Begin = 500,
            GM_Config_RoomCards = 0x1fc,
            GM_End = 0x201,
            GM_Modify_PlayerInfo = 0x1f8,
            GM_Modify_ServerConfig = 0x1fb,
            GM_Query_PlayerInfo = 0x1f6,
            GM_Query_PlayerInfo_Ack = 0x1f7,
            GM_Query_PlayerSendMoneyRecord = 510,
            GM_Query_PlayerSendMoneyRecord_Ack = 0x200,
            GM_Query_ServerConfig = 0x1f9,
            GM_Query_ServerConfig_Ack = 0x1fa,
            GM_Query_ToolSendMoneyRecord = 0x1fd,
            GM_Query_ToolSendMoneyRecord_Ack = 0x1ff,
            GM_ResultCode = 0x1f5,
            Http_Query = 11,
            Max = 0x202,
            Money_Send_Record_Save = 0x18,
            QueryAllOrderIds = 0xac,
            QueryAllOrderIdsAck = 0xad,
            RechargeSave = 170,
            RoomRecord_Load = 0x1a,
            RoomRecord_Load_Ack = 0x1c,
            RoomRecord_Save = 0x1b,
            Server_AgentRoomList = 0xa6,
            Server_AuthCode_Ack = 0x67,
            Server_Begin = 10,
            Server_Character_Create_Ack = 0x73,
            Server_Character_Info = 0x6f,
            Server_Character_LivingDataChanged = 110,
            Server_Character_Login_Ack = 0x74,
            Server_Character_Logout_Ack = 0x70,
            Server_Character_Recharge = 0xae,
            Server_CharacterDetail_Info = 0x76,
            Server_End = 0x1d,
            Server_GetBmobPayJson = 0xa8,
            Server_LandlordRoom_Info = 0x9e,
            Server_Lottery_Result = 0x97,
            Server_PaoMa_Message = 0x90,
            Server_PlayerChildRechargeRecord = 0xab,
            Server_PlayerCreateRoomList = 0x8a,
            Server_Query_CreateRoom_Info = 0x8b,
            Server_ResultCode = 0x6a,
            Server_ResultCode_String = 0x6b,
            Server_Room_Info = 0x93,
            Server_Room_Record_Ack = 0x79,
            Server_Room_Record_Detail_Ack = 0x7a,
            Server_RoomRecordBase = 140,
            Server_RoomRecordDetail = 0x8d,
            Server_ShareGainMoney = 0x9d,
            Server_SXMJRoom_Info = 0x88,
            Server_TBZRoom_Info = 0x89,
            Server_Version_Update = 0x65,
            Server_Versoin_Ack = 0x6d,
            Version_Ack = 12,
            Version_Info = 13,
            World_Record_Ack = 0x17,
            World_Record_Count_Ack = 0x15,
            World_Record_Count_Query = 20,
            World_Record_Save = 0x16
        }
   

    namespace FrameworkForCSharp.NetWorks
    {
        using System;
        using System.ComponentModel;

        public enum ResultCode : ushort
        {
            [Description("验证码错误")]
            AuthCode_Check_Failed = 0x12,
            [Description("验证码已过期")]
            AuthCode_Expired = 0x13,
            [Description("验证码发生失败")]
            AuthCode_Send_Failed = 20,
            [Description("验证太频繁")]
            AuthCode_Too_Often = 0x15,
            [Description("报停回合不能胡")]
            BaoTingRoundCanNotHu = 0x44,
            [Description("不能重复登录")]
            Can_Not_Login_Again = 12,
            [Description("不能报停")]
            CanNotBaoTing = 0x41,
            [Description("不能打这手牌")]
            CanNotCatchHoldCards = 0x55,
            [Description("当前状态不能销毁房间")]
            CanNotDisposeRoomThisStatus = 0x27,
            [Description("不能吃")]
            CanNotEat = 50,
            [Description("不能杠")]
            CanNotGang = 0x3a,
            [Description("不能杠这张牌")]
            CanNotGangThisCard = 0x34,
            [Description("不能胡")]
            CanNotHu = 0x39,
            [Description("不能吃什么打什么")]
            CanNotOperateLastChiPengCard = 0x38,
            [Description("不能出这张牌")]
            CanNotOutThisCard = 0x5e,
            [Description("不能碰")]
            CanNotPeng = 0x33,
            [Description("不能重复操作")]
            CanNotResultAgain = 0x24,
            [Description("每局只能设置一次")]
            CanNotSetFangPaoScoreAgain = 0x4f,
            [Description("只能设置一次邀请人")]
            CanNotSetInviteGuidAgain = 0x73,
            [Description("不能设置自己为邀请人")]
            CanNotSetInviteGuidSelf = 0x74,
            [Description("出的牌必须为混子")]
            CardMustMagicCard = 0x51,
            [Description("账号已注册")]
            Character_Has_Exist = 5,
            [Description("角色登陆产生异常")]
            Character_Login_Exception = 14,
            [Description("角色登陆失败")]
            Character_Login_Failed = 13,
            [Description("角色不存在")]
            Character_Not_Exist = 15,
            [Description("角色没有登录")]
            Character_Not_Login = 3,
            [Description("错误的吃牌")]
            ChiCardError = 0x36,
            [Description("筹码不足")]
            Chips_Not_Enough = 0x3b,
            [Description("配置的牌错误")]
            ConfigCardError = 0x4a,
            [Description("配置的房间不存在")]
            ConfigRoom_Not_Exist = 0x40,
            [Description("获取客户端创建信息产生异常")]
            CreateFromClientException = 7,
            [Description("获取客户端创建信息失败")]
            CreateFromClientFailed = 6,
            [Description("创建房间号失败")]
            CreateRoomCodeFailed = 0x1b,
            [Description("创建房间失败")]
            CreateRoomFailed = 0x1c,
            [Description("玩家创建房间数量已达上限")]
            CreateRoomNumberLimit = 0x18,
            [Description("当前回合碰限制")]
            CurRoundPengLimit = 0x3d,
            [Description("钻石不足")]
            DiamondNotEnough = 0x61,
            [Description("昵称已存在")]
            Duplicatoin_OtherName = 2,
            [Description("账号已注册")]
            Duplicatoin_RegisterName = 1,
            [Description("密码错误或用户名不存在")]
            Error_Password_Or_Character_Not_Exist = 11,
            [Description("错误的房间局数配置")]
            ErrorGameCountConfig = 0x56,
            [Description("非法的guid")]
            ErrorGuid = 110,
            [Description("错误的最大炸弹配置")]
            ErrorMaxBombCountConfig = 0x57,
            [Description("非法的放炮分数")]
            ErrorOperateScore = 0x5c,
            [Description("错误的玩的状态")]
            ErrorPlayStatus = 0x52,
            [Description("非法牌型")]
            ErrorPokerType = 0x54,
            [Description("位置错误")]
            ErrorPositionType = 0x2f,
            [Description("错误的充值金币数值")]
            ErrorRechargeGold = 0x6c,
            [Description("错误的充值游戏币数值")]
            ErrorRechargeMoney = 0x6b,
            [Description("房间状态不对")]
            ErrorRoomStatus = 0x25,
            [Description("非法的分数")]
            ErrorScore = 0x4e,
            [Description("放冲参数非法")]
            FangChongIndex_OutRange = 0x58,
            [Description("放炮参数非法")]
            FangPaoIndex_OutRange = 90,
            [Description("第一手必须出牌")]
            FirstHandMustOutCards = 0x53,
            [Description("局数下标越界")]
            GameCountIndex_OutRange = 0x19,
            [Description("钻石不足")]
            Gold_Not_Enough = 0x16,
            Ignore_Value = 0xfffe,
            [Description("注册平台号格式不对")]
            InValidAccountName = 8,
            [Description("设置放跑分数非法")]
            InvalidFangPaoScore = 80,
            [Description("昵称格式不对")]
            InValidOtherName = 9,
            [Description("密码格式不对")]
            InValidPassword = 10,
            [Description("手机号非法")]
            InvalidPhone = 0x10,
            [Description("上级玩家不是代理")]
            Invite_Not_Daili = 0x72,
            [Description("邀请人不存在")]
            InviteGuid_NotExist = 0x75,
            [Description("奖码设置错误")]
            JiangMaIndex_OutRange = 0x76,
            [Description("请先注销当前角色")]
            Logout_Cur_Character_First = 4,
            Max = 0xffff,
            [Description("方法不存在")]
            MethodIsNull = 100,
            [Description("游戏币不足(房卡不足)")]
            Money_Not_Enough = 0x17,
            [Description("没人可以硬扣")]
            NoOneCanYingKou = 0x43,
            [Description("没人弃牌")]
            NoOneDropCard = 0x2e,
            [Description("没有相同ip")]
            NoOneSameIp = 0x49,
            [Description("没有申请信息")]
            NoPlayerQueryDispose = 0x23,
            [Description("不需要等待玩家放炮")]
            NotWaitPlayerFangPao = 0x4c,
            [Description("缺少手机号参数")]
            NullGuid = 0x66,
            [Description("缺少订单号参数")]
            NullOrderId = 0x65,
            [Description("缺少充值金币参数")]
            NullRechargeGold = 0x68,
            [Description("缺少充值游戏币参数")]
            NullRechargeMoney = 0x67,
            [Description("缺少校验码参数")]
            NullSign = 0x69,
            [Description("手牌不存在")]
            OperateCardNotHoldCards = 0x37,
            [Description("订单号已存在")]
            OrderIdExist = 0x6f,
            [Description("订单号正在处理中")]
            OrderIdOnDeal = 0x6d,
            [Description("已经有玩家在申请")]
            OtherPlayerOnQueryDispose = 0x21,
            [Description("参数错误")]
            ParametersError = 0x63,
            [Description("支付方式设置错误")]
            PayTypeIndex_OutRange = 0x60,
            [Description("没有获取过验证码或验证码已过期")]
            Phone_Not_Query_AuthCode = 0x11,
            [Description("手机号未注册")]
            PhoneNotRegist = 0x70,
            [Description("玩家是代理")]
            Player_Is_Daili = 0x71,
            [Description("玩家不是代理")]
            Player_Not_Daili = 0x2b,
            [Description("不能操作")]
            PlayerCanNotOperate = 70,
            [Description("玩家数量参数非法")]
            PlayerCountIndex_OutRange = 0x5b,
            [Description("玩家已操作")]
            PlayerHasOperated = 60,
            [Description("玩家已经准备")]
            PlayerHasReadyForRoom = 0x48,
            [Description("玩家没有报停")]
            PlayerNotBaoTing = 0x3e,
            [Description("玩家不是房主")]
            PlayerNotCreator = 40,
            [Description("玩家没有这张牌")]
            PlayerNotHoldThisCard = 0x5f,
            [Description("玩家不在房间中")]
            PlayerNotInRoom = 0x22,
            [Description("玩家不需要补牌")]
            PlayerNotNeedBuPai = 0x29,
            [Description("当前位置已准备")]
            PositionHasDeady = 0x26,
            [Description("当前位置已经操作")]
            PositionHasOperate = 0x30,
            [Description("当前位置不能弃牌")]
            PostionCanNotDropCard = 0x2d,
            [Description("数据异常")]
            RecordDataLosed = 0x4b,
            [Description("剩余牌参数非法")]
            RemainCardIndex_OutRange = 0x59,
            [Description("房间已满")]
            RoomFull = 0x1f,
            [Description("房间不存在")]
            RoomNotExist = 0x1d,
            [Description("该房间不需要选择放炮")]
            RoomNotNeedSetFangPao = 0x4d,
            [Description("房间没有开始游戏")]
            RoomNotPlay = 0x2c,
            [Description("房间正在玩")]
            RoomOnPlay = 30,
            [Description("房间玩家太少")]
            RoomPlayerNeedMore = 0x45,
            [Description("房间没有空座位")]
            RoomPositionFull = 0x20,
            [Description("房间类型不存在")]
            RoomTypeError = 0x62,
            [Description("当前牌不能碰")]
            RoundPengLimit = 0x35,
            [Description("等待玩家补牌")]
            SameOneNeedBuPai = 0x2a,
            [Description("校验码失败")]
            SignCheckFailed = 0x6a,
            [Description("操作成功")]
            Successed = 0,
            [Description("今天已经抽奖过")]
            TodayHasLottery = 0x47,
            [Description("太远不能吃")]
            TooFarToEat = 0x31,
            [Description("封顶下标越界")]
            TopScoreIndex_OutRange = 0x1a,
            [Description("等待玩家选择是否硬扣")]
            WaitPlayerChooseYingKou = 0x42,
            [Description("等等玩家抢杠胡")]
            WaitPlayerQiangHu = 0x5d,
            [Description("硬扣不能胡")]
            YingKouCanNotHu = 0x3f
        }
    }

    namespace FrameworkForCSharp.NetWorks
    {
        using System;
        using System.Collections.Generic;
        using System.Net.Sockets;

        public class SocketAsyncEventArgsPool
        {
            private Stack<SocketAsyncEventArgs> pool;

            public SocketAsyncEventArgsPool(int capacity)
            {
                this.pool = new Stack<SocketAsyncEventArgs>(capacity);
            }

            public SocketAsyncEventArgs pop()
            {
                Stack<SocketAsyncEventArgs> pool = this.pool;
                lock (pool)
                {
                    return this.pool.Pop();
                }
            }

            public void push(SocketAsyncEventArgs item)
            {
                Stack<SocketAsyncEventArgs> pool = this.pool;
                lock (pool)
                {
                    this.pool.Push(item);
                }
            }
        }
    }

    namespace FrameworkForCSharp.NetWorks
    {
        using System;

        public interface SocketMaster
        {
            void notifyAdd(Connection conn);
            void notifyRelease(Connection conn, string source);
        }
    }


    namespace FrameworkForCSharp.NetWorks
    {
        using System;
        using System.Net;
        using System.Net.Sockets;
        using System.Runtime.InteropServices;
        using System.Threading;

        public class TcpClient : ConnectionManager, SocketMaster
        {
            private Action<FrameworkForCSharp.NetWorks.Connection> add;
            private SocketAsyncEventArgs arg = new SocketAsyncEventArgs();
            private int bufferSize = 0x1000;
            private Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            private Action<FrameworkForCSharp.NetWorks.Connection> del;
            private bool initialized = false;
            private string ip;
            private int maxNumConnections = 1;
            private Action<FrameworkForCSharp.NetWorks.Connection> onConnected;
            private ushort port;
            private ManualResetEvent semaphore = new ManualResetEvent(false);

            public TcpClient(int maxNumConnections = 1, int bufferSize = 0x1000)
            {
                this.maxNumConnections = maxNumConnections;
                this.bufferSize = bufferSize;
                this.arg.Completed += new EventHandler<SocketAsyncEventArgs>(this.io_completed);
            }

            public void connect<T>(string ip, ushort port, Action<FrameworkForCSharp.NetWorks.Connection> onConnected = null) where T : FrameworkForCSharp.NetWorks.Connection, new()
            {
                if (!this.initialized)
                {
                    this.initialized = true;
                    base.initPool<T>(this.maxNumConnections, this.bufferSize);
                }
                try
                {
                    this.clientSocket.Close();
                }
                catch (Exception)
                {
                }
                this.clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                this.ip = ip;
                this.port = port;
                this.onConnected = onConnected;
                IPEndPoint point = new IPEndPoint(IPAddress.Parse(ip), port);
                this.arg.RemoteEndPoint = point;
                base.maxNumAcceptedClients.WaitOne();
                try
                {
                    uint structure = 0;
                    byte[] array = new byte[Marshal.SizeOf(structure) * 3];
                    BitConverter.GetBytes((uint)1).CopyTo(array, 0);
                    BitConverter.GetBytes((uint)500).CopyTo(array, Marshal.SizeOf(structure));
                    BitConverter.GetBytes((uint)500).CopyTo(array, (int)(Marshal.SizeOf(structure) * 2));
                    this.clientSocket.IOControl(IOControlCode.KeepAliveValues, array, null);
                }
                catch (Exception)
                {
                }
                this.clientSocket.ConnectAsync(this.arg);
                this.semaphore.WaitOne();
            }

            public void connectIpv6<T>(string ip, ushort port, Action<FrameworkForCSharp.NetWorks.Connection> onConnected = null) where T : FrameworkForCSharp.NetWorks.Connection, new()
            {
                if (!this.initialized)
                {
                    this.initialized = true;
                    base.initPool<T>(this.maxNumConnections, this.bufferSize);
                }
                try
                {
                    this.clientSocket.Close();
                }
                catch (Exception)
                {
                }
                this.clientSocket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
                this.ip = ip;
                this.port = port;
                this.onConnected = onConnected;
                IPEndPoint point = new IPEndPoint(IPAddress.Parse(ip), port);
                this.arg.RemoteEndPoint = point;
                base.maxNumAcceptedClients.WaitOne();
                try
                {
                    uint structure = 0;
                    byte[] array = new byte[Marshal.SizeOf(structure) * 3];
                    BitConverter.GetBytes((uint)1).CopyTo(array, 0);
                    BitConverter.GetBytes((uint)500).CopyTo(array, Marshal.SizeOf(structure));
                    BitConverter.GetBytes((uint)500).CopyTo(array, (int)(Marshal.SizeOf(structure) * 2));
                    this.clientSocket.IOControl(IOControlCode.KeepAliveValues, array, null);
                }
                catch (Exception)
                {
                }
                this.clientSocket.ConnectAsync(this.arg);
                this.semaphore.WaitOne();
            }

            private void io_completed(object sender, SocketAsyncEventArgs e)
            {
                this.process_connect(e);
            }

            protected virtual void onConnectedFailed(string ip, ushort port)
            {
            }

            private void process_connect(SocketAsyncEventArgs e)
            {
                this.semaphore.Set();
                if ((e.SocketError == SocketError.Success) && this.clientSocket.Connected)
                {
                    FrameworkForCSharp.NetWorks.Connection connection = base.connectionPool.pop();
                    connection.master = this;
                    connection.socket = this.clientSocket;
                    if (this.onConnected != null)
                    {
                        this.onConnected(connection);
                    }
                    connection.start();
                }
                else
                {
                    base.maxNumAcceptedClients.Release();
                    Thread.Sleep(0x3e8);
                    this.onConnectedFailed(this.ip, this.port);
                }
            }

            public void registerDelegate(Action<FrameworkForCSharp.NetWorks.Connection> add, Action<FrameworkForCSharp.NetWorks.Connection> del)
            {
                this.add = add;
                this.del = del;
            }
        }
    }


    namespace FrameworkForCSharp.NetWorks
    {
        using System;
        using System.Net;
        using System.Net.Sockets;
        using System.Runtime.InteropServices;

        public class TcpServer : ConnectionManager
        {
            private Action<FrameworkForCSharp.NetWorks.Connection> add;
            private int bufferSize = 0x1000;
            private Action<FrameworkForCSharp.NetWorks.Connection> del;
            private Socket listenSocket;
            private int maxNumConnections = 0x3e8;

            public TcpServer(int maxNumConnections, int bufferSize = 0x1000)
            {
                if ((maxNumConnections < 1) || (bufferSize < NetworkMessage.HEAD_LEN))
                {
                    throw new ArgumentException("invalid maxNumConnections or bufferSize");
                }
                this.maxNumConnections = maxNumConnections;
                this.bufferSize = bufferSize;
            }

            private void accept_completed(object sender, SocketAsyncEventArgs e)
            {
                this.process_accept(e);
            }

            private void process_accept(SocketAsyncEventArgs e)
            {
                if (e.SocketError == SocketError.Success)
                {
                    FrameworkForCSharp.NetWorks.Connection connection = base.connectionPool.pop();
                    connection.renew();
                    connection.master = this;
                    connection.socket = e.AcceptSocket;
                    connection.start();
                }
                else
                {
                    base.maxNumAcceptedClients.Release();
                }
                this.start_accept(e);
            }

            public void registerDelegate(Action<FrameworkForCSharp.NetWorks.Connection> add, Action<FrameworkForCSharp.NetWorks.Connection> del)
            {
                this.add = add;
                this.del = del;
            }

            public void start<T>(ushort port, ushort backlog = 10) where T : FrameworkForCSharp.NetWorks.Connection, new()
            {
                this.listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint localEP = new IPEndPoint(IPAddress.Any, port);
                this.listenSocket.Bind(localEP);
                this.listenSocket.Listen(backlog);
                base.initPool<T>(this.maxNumConnections, this.bufferSize);
                try
                {
                    uint structure = 0;
                    byte[] array = new byte[Marshal.SizeOf(structure) * 3];
                    BitConverter.GetBytes((uint)1).CopyTo(array, 0);
                    BitConverter.GetBytes((uint)500).CopyTo(array, Marshal.SizeOf(structure));
                    BitConverter.GetBytes((uint)500).CopyTo(array, (int)(Marshal.SizeOf(structure) * 2));
                    this.listenSocket.IOControl(IOControlCode.KeepAliveValues, array, null);
                }
                catch (Exception)
                {
                }
                this.start_accept(null);
            }

            public void start<T>(string ip, ushort port, ushort backlog = 10) where T : FrameworkForCSharp.NetWorks.Connection, new()
            {
                this.listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint localEP = new IPEndPoint(IPAddress.Parse(ip), port);
                this.listenSocket.Bind(localEP);
                this.listenSocket.Listen(backlog);
                base.initPool<T>(this.maxNumConnections, this.bufferSize);
                try
                {
                    uint structure = 0;
                    byte[] array = new byte[Marshal.SizeOf(structure) * 3];
                    BitConverter.GetBytes((uint)1).CopyTo(array, 0);
                    BitConverter.GetBytes((uint)500).CopyTo(array, Marshal.SizeOf(structure));
                    BitConverter.GetBytes((uint)500).CopyTo(array, (int)(Marshal.SizeOf(structure) * 2));
                    this.listenSocket.IOControl(IOControlCode.KeepAliveValues, array, null);
                }
                catch (Exception)
                {
                }
                this.start_accept(null);
            }

            private void start_accept(SocketAsyncEventArgs arg)
            {
                if (arg == null)
                {
                    arg = new SocketAsyncEventArgs();
                    arg.Completed += new EventHandler<SocketAsyncEventArgs>(this.accept_completed);
                }
                else
                {
                    arg.AcceptSocket = null;
                }
                base.maxNumAcceptedClients.WaitOne();
                if (!this.listenSocket.AcceptAsync(arg))
                {
                    this.process_accept(arg);
                }
            }

            public void stop()
            {
                this.listenSocket.Close();
            }
        }
 *   }

*/
 
#endregion