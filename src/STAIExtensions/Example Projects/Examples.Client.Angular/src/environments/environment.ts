/**
 * @license
 * Copyright Akveo. All Rights Reserved.
 * Licensed under the MIT License. See License.txt in the project root for license information.
 */
// The file contents for the current environment will overwrite these during build.
// The build system defaults to the dev environment which uses `environment.ts`, but if you do
// `ng build --env=prod` then `environment.prod.ts` will be used instead.
// The list of which env maps to which file can be found in `.angular-cli.json`.

export const environment = {
  production: false,
  signalRHost: 'https://localhost:5001/STAIExtensionsHub',
  signalRAuthKey: '1a99436ef0e79d26ada7bb20e675a27d3fe13d91156624e9f50ec428d71e8495'
};
