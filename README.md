# DOTNETKONF'21 WORKSHOP

## INSTALLATION

### Create Selenium Environment

- `kubectl apply -f 1-selenium-hub.yaml`
- `kubectl apply -f 2-selenium-node.yaml`

### Install Flagger

_Check 1144. line for the Prometheus connection_

- `kubectl apply -f 3-flagger.yaml`

### Deploy Selenium API

_If you want to add new features, you should go to `selenium-api` folder and modify the code! After, you need to build and push the image again!_

- `kubectl apply -f 4-selenium-api.yaml`

### Deploy UI Project _(will be used for ui tests)_

- `kubectl apply -f 5-ui.yaml`

### Deploy Canary Object (Flagger Custom Resource)

_Don't forget to change `##### SLACK_WEBHOOK_ADDRESS #####`, `##### ISTIO_GATEWAY_NAME #####`, `##### HOST_NAME #####` variables!_

- `kubectl apply -f 6-ui-canary.yaml`

## SCENARIOS

Change the container version from `5-ui.yaml` file and watch the canaries object for changes: `kubectl describe canaries`. Also you can follow the UI test logs via `kubectl logs deployment/ui-tests-api -c ui-tests-api -f`.yaml`

1.0.0 version is zero point, everything is OK :) When you set the version as 1.0.1 canary object will block the revision because test scenario will be failed.
