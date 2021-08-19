declare global {
  interface External {
    sendMessage: (message: string) => void;
    receiveMessage: (delegate: (message: string) => void) => void;
  }

  type OptionalKeys<T> = { [K in keyof T]-?: {} extends Pick<T, K> ? K : never }[keyof T];
  type Defaults<T> = Required<Pick<T, OptionalKeys<T>>>;
}

if (typeof window.external.sendMessage !== 'function') {
  window.external.sendMessage = (message: string) => console.log(`(Emulating sendMessage): ${message}`);
}

if (typeof window.external.receiveMessage !== 'function') {
  window.external.receiveMessage = (delegate: (message: string) => void) => {
    const message = 'Simulating message from backend.';
    delegate(message);
  };
  window.external.receiveMessage((message: string) => console.log(`(Emulating receiveMessage): ${message}`));
}

import * as uuid from 'uuid';
import { IO, OS, App, Window } from './api';

export interface IPhoton<TData extends IPhotonData> {
  _id: string;
  data: TData;
}

export interface PhotonRequestData extends IPhotonData {
  ns: string;
  action: string;
  params?: { [name: string]: any };
}

export interface PhotonResponseData extends IPhotonData {
  return: any;
  message: string;
  status: PhotonStatus;
}

export interface IPhotonData {}

export enum PhotonStatus {
  Success,
  Error,
}

export class Photon<TData extends IPhotonData> implements IPhoton<TData> {
  readonly _id: string;

  constructor(data: TData) {
    this._id = uuid.v4();
    this.data = data;
  }

  data: TData;
}

export default class Photino {
  readonly io: IO;
  readonly os: OS;
  readonly app: App;
  readonly window: Window;

  constructor() {
    this.io = new IO(this);
    this.os = new OS(this);
    this.app = new App(this);
    this.window = new Window(this);
  }

  private sendData(data: PhotonRequestData): Promise<IPhoton<PhotonResponseData>> {
    const photon = new Photon(data);
    window.external.sendMessage(JSON.stringify(photon));

    return new Promise((resolve) => {
      const timeout = setTimeout(() => {
        throw new Error('Took to long for a response');
      }, 30000);

      window.external.receiveMessage((message) => {
        const retPhoton = JSON.parse(message) as IPhoton<PhotonResponseData>;
        if (retPhoton._id === photon._id) {
          clearTimeout(timeout);
          resolve(retPhoton);
        }
      });
    });
  }

  async send<TReturn>(data: PhotonRequestData): Promise<TReturn> {
    const res = await this.sendData(data);
    if (res.data.status !== PhotonStatus.Success) {
      throw new Error(res.data.message);
    }
    return res.data.return as TReturn;
  }
}
